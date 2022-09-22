using System;
using Microsoft.EntityFrameworkCore;

namespace CHESF.COMPRAS.IRepository.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        DbContext Context { get; }
        void Commit();
        void DetachAll();
    }
}