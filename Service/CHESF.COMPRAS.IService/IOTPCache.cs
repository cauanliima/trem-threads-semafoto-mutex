using System;

namespace CHESF.COMPRAS.IService
{
    public interface IOTPCache
    {
        void SetOTP(long cnpj, string otp, DateTime tempoExpiracao);
        string GetOTP(long cnpj);
        void RemoveOTP(long cnpj);
    }
}