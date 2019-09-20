
using AutoMapper;
using AlejandroGarciaMalo.Models.DBContext;
using AlejandroGarciaMalo.Models.Repository;
using AlejandroGarciaMalo.Models.Repository.Base;
using AlejandroGarciaMalo.Models.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AlejandroGarciaMalo.Controllers
{
    public class BaseController : Controller
    {
        /// <summary>
        /// Logger
        /// </summary>
        protected readonly ILogger Logger;
        protected readonly IMapper _mapper;
        protected MyDbContext _dbContext;

        public BaseController(ILogger<BaseController> logger, IMapper mapper)
        {
            Logger = logger;
            _mapper = mapper;
            _dbContext = new MyDbContext();
        }

    }
}
