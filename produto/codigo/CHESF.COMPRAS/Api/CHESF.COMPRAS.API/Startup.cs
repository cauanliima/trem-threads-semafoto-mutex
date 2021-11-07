using System;
using System.Globalization;
using System.IO;
using System.Text;
using CHESF.BSV.Repository.Context;
using CHESF.COMPRAS.IRepository;
using CHESF.COMPRAS.IRepository.Base;
using CHESF.COMPRAS.IRepository.UnitOfWork;
using CHESF.COMPRAS.IService;
using CHESF.COMPRAS.Repository;
using CHESF.COMPRAS.Repository.Base;
using CHESF.COMPRAS.Repository.UnitOfWork;
using CHESF.COMPRAS.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

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
            // services.AddHttpClient<AuthController>("SISAUTH", c =>
            // {
            //     c.BaseAddress = new Uri(Configuration["SISAUTH_URL"] + "/auth");
            //     c.DefaultRequestHeaders.Add("Accept", "*/*");
            //     c.DefaultRequestHeaders.Add("User-Agent", "HttpClientFactory-Sample");
            // });

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

            services.AddDbContext<ComprasContext>(options => options
                .UseSqlServer(Environment.GetEnvironmentVariable("EEDITAL_CONNECTION"))
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));

            services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));


            //Repository
            services.AddTransient<ILicitacaoRepository, LicitacaoRepository>();
            services.AddTransient<IAnexoRepository, AnexoRepository>();

            //Service
            services.AddTransient<ILicitacaoService, LicitacaoService>();
            services.AddTransient<IAnexoService, AnexoService>();

            #endregion

            #region SWAGGER

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "COMPRAS API", Version = "v1", Description = "Métodos da API do COMPRAS"
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