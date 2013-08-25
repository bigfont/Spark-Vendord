using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vendord.Models;
using Vendord.ViewModels;

namespace Vendord.App_Start
{
    public class AutoMapperConfig
    {
        public static void RegisterMaps()
        {
            Mapper.CreateMap<ProductCreateViewModel, Product>();
        }
    }
}