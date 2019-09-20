using AutoMapper;
using AlejandroGarciaMalo.Models.DBContext;
using AlejandroGarciaMalo.Models.Entities;
using AlejandroGarciaMalo.Models.JsonModels;
using AlejandroGarciaMalo.Models.Repository.Base;
using AlejandroGarciaMalo.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlejandroGarciaMalo.Models.Repository
{
    public class TransactionRepository : Repository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(MyDbContext context)
        : base(context)
        { }
        
    }
}
