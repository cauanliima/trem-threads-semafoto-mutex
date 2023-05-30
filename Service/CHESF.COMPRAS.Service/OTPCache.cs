using System;
using CHESF.COMPRAS.IService;
using Microsoft.Extensions.Caching.Memory;

namespace CHESF.COMPRAS.Service
{
    public class OTPCache : IOTPCache
    {
        private readonly IMemoryCache _memoryCache;

        public OTPCache(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        private string GetCacheKey(long cnpj)
        {
            return $"OTP_{cnpj}";
        }

        public void SetOTP(long cnpj, string otp, DateTime tempoExpiracao)
        {
            string cacheKey = GetCacheKey(cnpj);
            _memoryCache.Set(cacheKey, otp, tempoExpiracao);
        }

        public string GetOTP(long cnpj)
        {
            string cacheKey = GetCacheKey(cnpj);
            return _memoryCache.Get<string>(cacheKey);
        }

        public void RemoveOTP(long cnpj)
        {
            string cacheKey = GetCacheKey(cnpj);
            _memoryCache.Remove(cacheKey);
        }
    }
}