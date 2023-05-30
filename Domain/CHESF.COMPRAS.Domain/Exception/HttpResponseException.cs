namespace CHESF.COMPRAS.Domain.Exception
{
    public class HttpResponseException: System.Exception
    {
        public HttpResponseException(int status, string mensagem)
        {
            this.Status = status;
            this.Mensagem = mensagem;
        }
        
        public int Status { get; set; } = 500;
        public string Mensagem { get; set; }
    }
}