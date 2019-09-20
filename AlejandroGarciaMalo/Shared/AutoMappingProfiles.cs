using AutoMapper;
using AlejandroGarciaMalo.Models.Entities;
using AlejandroGarciaMalo.Models.JsonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlejandroGarciaMalo.Shared
{
    /// <summary>
    /// Profiles for automapper
    /// </summary>
    public class AutoMappingProfiles : Profile
    {
        public AutoMappingProfiles()
        {
            CreateMap<JsonRate, Rate>().ForMember(destination => destination.RateValue,
               opts => opts.MapFrom(source => source.Rate));
            CreateMap<JsonTransaction, Transaction>();
        }
    }
}
