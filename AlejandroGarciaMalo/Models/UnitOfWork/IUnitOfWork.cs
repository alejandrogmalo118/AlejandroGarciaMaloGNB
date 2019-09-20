using AlejandroGarciaMalo.Models.Repository.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace AlejandroGarciaMalo.Models.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IRateRepository Rates { get; }
        ITransactionRepository Transactions { get; }

        int Complete();
    }
}
