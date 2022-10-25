using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using CHESF.COMPRAS.API.Config.Security;
using CHESF.COMPRAS.API.Scheduler;
using CHESF.COMPRAS.API.Scheduler.Settings;
using CHESF.COMPRAS.IRepository;
using CHESF.COMPRAS.IRepository.Base;
using CHESF.COMPRAS.IRepository.UnitOfWork;
using CHESF.COMPRAS.IService;
using CHESF.COMPRAS.Repository;
using CHESF.COMPRAS.Repository.Base;
using CHESF.COMPRAS.Repository.Context;
using CHESF.COMPRAS.Repository.UnitOfWork;
using CHESF.COMPRAS.Service;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Quartz;

namespace CHESF.COMPRAS.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private const string PastaWkhtmltopdf = "HTML2PDF";

        public IConfiguration Configuration { get; }

        private static void CriarPastaWkhtmltopdf()
        {
            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + PastaWkhtmltopdf))
            {
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + PastaWkhtmltopdf);
            }
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.TypeNameHandling = TypeNameHandling.None;
                    options.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });

            services.AddCors();
            services.AddHttpContextAccessor();

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.ASCII.GetBytes(Configuration.GetSection("Jwt").GetSection("Secret").Value)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            #region DADOS DE CONTEXT E IoC
            string? base64FirebaseKey = Environment.GetEnvironmentVariable("BASE64_FIREBASE_KEY");

            if (base64FirebaseKey != null)
            {
                var keyBytes = Convert.FromBase64String(base64FirebaseKey);
                FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromStream(new MemoryStream(keyBytes))
                });
            }


            services.AddDbContext<ComprasContext>(options => options
                .UseSqlServer(Environment.GetEnvironmentVariable("EEDITAL_CONNECTION"))
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

            services.AddDbContext<SGNFContext>(options => options
                .UseSqlServer(Environment.GetEnvironmentVariable("SGNF_CONNECTION"))
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

            services.AddScoped(typeof(IEComprasUnitOfWork), typeof(EcomprasUnitOfWork));
            services.AddScoped(typeof(ISGNFUnitOfWork), typeof(SGNFUnitOfWork));
            services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));

            //Repository
            services.AddTransient<ILicitacaoRepository, LicitacaoRepository>();
            services.AddTransient<IAnexoRepository, AnexoRepository>();
            services.AddTransient<IUsuarioRepository, UsuarioRepository>();
            services.AddTransient<IContratoRepository, ContratoRepository>();
            services.AddTransient<IContratoFonecedorRepository, ContratoFornecedorRepository>();
            services.AddTransient<INotaFiscalRepository, NotaFiscalRepository>();
            services.AddTransient<IDispositivoRepository, DispositivoRepository>();
            services.AddTransient<IDispositivoMetadadoRepository, DispositivoMetadadoRepository>();
            services.AddTransient<INotificacaoRepository, NotificacaoRepository>();
            services.AddTransient<INotificacaoDispositivoRepository, NotificacaoDispositivoRepository>();

            //Service
            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<ILicitacaoService, LicitacaoService>();
            services.AddTransient<IAnexoService, AnexoService>();
            services.AddTransient<INotificationService, NotificationService>().AddOptions();
            services.AddTransient<ILoginService, LoginService>();
            services.AddTransient<IContratoService, ContratoService>();
            services.AddTransient<IGerarNotificacaoPagamentoService, GerarNotificacaoPagamentoService>();

            services.AddScoped<DispositivoUidAttribute>();

            #endregion

            #region SWAGGER

            services.AddSwaggerGen(c =>
            {
                const string securityDefinition = "ApiKey";
                const string dispositivoUidDefinition = "DispositivoUID";

                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "COMPRAS API", Version = "v1", Description = "Métodos da API do COMPRAS"
                });

                c.AddSecurityDefinition(securityDefinition, new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Name = "X-API-KEY",
                    Type = SecuritySchemeType.ApiKey
                });
                
                c.AddSecurityDefinition(dispositivoUidDefinition, new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Name = "X-DISPOSITIVO-UID",
                    Type = SecuritySchemeType.ApiKey
                });
                
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Name = "X-DISPOSITIVO-UID",
                            Type = SecuritySchemeType.ApiKey,
                            In = ParameterLocation.Header,
                            Reference = new OpenApiReference
                                { Type = ReferenceType.SecurityScheme, Id = dispositivoUidDefinition }
                        },
                        new List<string>()
                    }
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Name = "X-API-KEY",
                            Type = SecuritySchemeType.ApiKey,
                            In = ParameterLocation.Header,
                            Reference = new OpenApiReference
                                { Type = ReferenceType.SecurityScheme, Id = securityDefinition }
                        },
                        new List<string>()
                    }
                });

                var jwtSecurityScheme = new OpenApiSecurityScheme
                {
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Name = "JWT Authentication",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Description = "Insira a chave JWT (sem o Bearer)",

                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };

                c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { jwtSecurityScheme, Array.Empty<string>() }
                });

                var caminhoAplicacao =
                    PlatformServices.Default.Application.ApplicationBasePath;
                var nomeAplicacao =
                    PlatformServices.Default.Application.ApplicationName;
            });

            #endregion
            
            services.AddQuartz(q =>
            {
                q.UseMicrosoftDependencyInjectionScopedJobFactory();
                q.AddJobAndTrigger<JobNotificacaoPagamento>(Configuration);
            });

            services.AddQuartzHostedService(
                q => q.WaitForJobsToComplete = true);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var basePath = Environment.GetEnvironmentVariable("CONTEXTO");

            basePath = !string.IsNullOrEmpty(basePath) ? $"{basePath}/api" : "/api";

            app.UsePathBase(basePath);
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            app.UseHttpsRedirection();
            app.UseCors(options =>
                options.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());

            app.UseStaticFiles();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"{basePath ?? ""}/swagger/v1/swagger.json", "COMPRAS-API v1");
                c.RoutePrefix = "docs";
            });


            var supportedCultures = new[] { new CultureInfo("pt-BR") };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("pt-BR", "pt-BR"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseForwardedHeaders();
        }
    }
}