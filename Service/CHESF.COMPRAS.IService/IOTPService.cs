namespace CHESF.COMPRAS.IService
{
    public interface IOTPService
    {
        string gerarOTP(long cnpj);
        bool validarOTP(long cnpj, string otp);
    }
}