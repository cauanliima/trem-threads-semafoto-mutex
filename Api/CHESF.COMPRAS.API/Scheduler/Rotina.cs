using System.Threading.Tasks;
using Quartz;

namespace CHESF.COMPRAS.API.Scheduler
{
    public abstract class Rotina : IJob
    {
        

        protected Rotina()
        {
            
        }

        protected abstract string NomeRotina { get; }

        public async Task Execute(IJobExecutionContext context)
        {
            
            await ProcessarRotina(context);
        }

        protected abstract Task ProcessarRotina(IJobExecutionContext context);

    }
}