using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataEntrySystem.Services.AutoMapperConfig
{
    public static class AutoMapperConfig
    {
        public static MapperConfiguration Register()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AllowNullCollections = true;

                cfg.CreateMap<Contracts.Document, DAL.Models.Document>();
                cfg.CreateMap<DAL.Models.Document, Contracts.Document>();
            });

            return config;
        }
    }
}
