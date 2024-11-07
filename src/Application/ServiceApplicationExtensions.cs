﻿using Application.UseCase.Pagamentos;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;

namespace Application
{
    public static class ServiceApplicationExtensions
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services)
        {
           
            services.AddScoped<IPagamentoUseCase, PagamentoUseCase>();

            var config = new MapperConfiguration(cfg =>
            {
                      
            });

            IMapper mapper = config.CreateMapper();

            services.AddSingleton(mapper);

            return services;
        }

        public static string GetEnumDescription(this Enum value)
        {
            if (value == null) { return ""; }

            DescriptionAttribute attribute = value.GetType()
                    .GetField(value.ToString())
                    ?.GetCustomAttributes(typeof(DescriptionAttribute), false)
                    .SingleOrDefault() as DescriptionAttribute;
            return attribute == null ? value.ToString() : attribute.Description;
        }
    }
}
