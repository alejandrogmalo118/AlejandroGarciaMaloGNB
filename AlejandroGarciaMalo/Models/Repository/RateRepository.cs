using AutoMapper;
using AlejandroGarciaMalo.Models.DBContext;
using AlejandroGarciaMalo.Models.Entities;
using AlejandroGarciaMalo.Models.Repository.Base;
using AlejandroGarciaMalo.Models.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlejandroGarciaMalo.Models.Repository
{
    public class RateRepository : Repository<Rate>, IRateRepository
    {
        public RateRepository(MyDbContext context)
        : base(context)
        {
        }
    }
}
