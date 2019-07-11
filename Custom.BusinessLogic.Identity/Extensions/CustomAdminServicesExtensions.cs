using System;
using System.Collections.Generic;
using AutoMapper;
using Custom.BusinessLogic.Identity.Dtos;
using Custom.BusinessLogic.Identity.Mappers.Configuration;
using Custom.BusinessLogic.Identity.Services;
using Custom.BusinessLogic.Identity.Services.Interfaces;
using Custom.EntityFramework.DbContexts;
using Custom.EntityFramework.Entities;
using Custom.EntityFramework.Identity.Repositories;
using Custom.EntityFramework.Identity.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Skoruba.IdentityServer4.Admin.BusinessLogic.Identity.Dtos.Identity;
using Skoruba.IdentityServer4.Admin.BusinessLogic.Identity.Mappers.Configuration;
using Skoruba.IdentityServer4.Admin.BusinessLogic.Identity.Resources;
using Skoruba.IdentityServer4.Admin.BusinessLogic.Identity.Services;
using Skoruba.IdentityServer4.Admin.BusinessLogic.Identity.Services.Interfaces;
using Skoruba.IdentityServer4.Admin.EntityFramework.Identity.Repositories;
using Skoruba.IdentityServer4.Admin.EntityFramework.Identity.Repositories.Interfaces;
using Skoruba.IdentityServer4.Admin.EntityFramework.Interfaces;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Mehdi
    /// </summary>
    public static class CustomServicesExtensions
    {
        public static ICustomMapperConfigurationBuilder AddCustomIdentityMapping(this IServiceCollection services)
        {
            var builder = new CustomMapperConfigurationBuilder();

            services.AddSingleton<IConfigurationProvider>(sp => new MapperConfiguration(cfg =>
            {
                foreach (var profileType in builder.ProfileTypes)
                    cfg.AddProfile(profileType);
            }));

            services.AddScoped<IMapper>(sp => new Mapper(sp.GetRequiredService<IConfigurationProvider>(), sp.GetService));

            return builder;
        }


        public static IServiceCollection AddCustomIdentityServices<ICustomDbContext, TPerson, TUser, TPersonDto, TPersonDtoKey, TUserDto, TUserDtoKey, TPepoleDto, TKey, TUserKey>(
                        this IServiceCollection services, HashSet<Type> profileTypes)
            where ICustomDbContext : CustomDbContext
            where TPerson : Person
            where TUser : IdentityUser<TUserKey>
            where TUserKey : IEquatable<TUserKey>
            where TKey : IEquatable<TKey>
            where TPersonDto : PersonDto<TKey, TUserDto, TUserDtoKey>
            where TUserDto : UserDto<TUserDtoKey>
            where TPepoleDto : PepoleDto<TPersonDto, TKey, TUserDto, TUserDtoKey>
        {
            //Repositories
            services.AddTransient<ICustomIdentityRepository<TPerson, TKey>, CustomIdentityRepository<ICustomDbContext, TPerson, TKey>>();

            //Services
            services.AddTransient<ICustomIdentityService<TPersonDto, TPersonDtoKey, TPerson, TUserDto, TUserDtoKey,
            TKey, TPepoleDto>,CustomIdentityService<TPersonDto, TPersonDtoKey, TPerson, TUserDto, TUserDtoKey,TKey, TPepoleDto>>();


            //Resources
            services.AddScoped<IIdentityServiceResources, IdentityServiceResources>();

            //Register mapping
            services.AddCustomIdentityMapping()
                .UseCustomMappingProfile<TPersonDto, TPersonDtoKey, TPerson, TUser, TUserDto, TUserDtoKey, TPepoleDto, TKey, TUserKey>()
                .AddProfilesType(profileTypes);

            return services;
        }
    }
}
