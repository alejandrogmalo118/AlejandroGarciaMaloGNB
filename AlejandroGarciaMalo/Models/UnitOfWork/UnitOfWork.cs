using AutoMapper;
using AlejandroGarciaMalo.Models.DBContext;
using AlejandroGarciaMalo.Models.Repository;
using AlejandroGarciaMalo.Models.Repository.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlejandroGarciaMalo.Models.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MyDbContext _context;

        public UnitOfWork(MyDbContext context)
        {
            _context = context;

            Rates = new RateRepository(_context);
            Transactions = new TransactionRepository(_context);
        }
        
        public IRateRepository Rates { get; }

        public ITransactionRepository Transactions { get; }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
