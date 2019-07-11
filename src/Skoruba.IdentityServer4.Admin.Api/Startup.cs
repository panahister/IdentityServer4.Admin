using System;
using System.Collections.Generic;
using Custom.BusinessLogic.Identity.Dtos;
using Custom.BusinessLogic.Identity.Mappers;
using Custom.EntityFramework.DbContexts;
using Custom.EntityFramework.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Skoruba.IdentityServer4.Admin.Api.Configuration;
using Skoruba.IdentityServer4.Admin.Api.Configuration.Authorization;
using Skoruba.IdentityServer4.Admin.Api.Configuration.Constants;
using Skoruba.IdentityServer4.Admin.Api.ExceptionHandling;
using Skoruba.IdentityServer4.Admin.Api.Helpers;
using Skoruba.IdentityServer4.Admin.Api.Mappers;
using Skoruba.IdentityServer4.Admin.BusinessLogic.Identity.Dtos.Identity;
using Skoruba.IdentityServer4.Admin.EntityFramework.Shared.DbContexts;
using Skoruba.IdentityServer4.Admin.EntityFramework.Shared.Entities.Identity;
using Swashbuckle.AspNetCore.Swagger;

namespace Skoruba.IdentityServer4.Admin.Api
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
            }

            Configuration = builder.Build();

            HostingEnvironment = env;
        }

        public IConfiguration Configuration { get; }

        public IHostingEnvironment HostingEnvironment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var adminApiConfiguration = Configuration.GetSection(nameof(AdminApiConfiguration)).Get<AdminApiConfiguration>();
            services.AddSingleton(adminApiConfiguration);

            services.AddDbContexts<AdminIdentityDbContext, IdentityServerConfigurationDbContext, IdentityServerPersistedGrantDbContext, AdminLogDbContext, CustomDbContext>(Configuration);
            services.AddScoped<ControllerExceptionFilterAttribute>();

            services.AddApiAuthentication<AdminIdentityDbContext, UserIdentity, UserIdentityRole>(adminApiConfiguration);
            services.AddAuthorizationPolicies();

            var profileTypes = new HashSet<Type>
            {
                typeof(IdentityMapperProfile<RoleDto<string>, string, UserRolesDto<RoleDto<string>, string, string>, string, UserClaimsDto<string>, UserClaimDto<string>, UserProviderDto<string>, UserProvidersDto<string>, UserChangePasswordDto<string>,RoleClaimDto<string>, RoleClaimsDto<string>>)
            };
            var customProfileTypes = new HashSet<Type>
            {
                typeof(CustomMapperProfile<PersonDto<long, UserDto<string>,string>,long,Person,UserIdentity,UserDto<string>,string,PepoleDto<PersonDto<long, UserDto<string>, string>, long, UserDto<string>, string>,long,string>)
            };

            services.AddAdminAspNetIdentityServices<AdminIdentityDbContext, IdentityServerPersistedGrantDbContext, UserDto<string>, string, RoleDto<string>, string, string, string,
                UserIdentity, UserIdentityRole, string, UserIdentityUserClaim, UserIdentityUserRole,
                UserIdentityUserLogin, UserIdentityRoleClaim, UserIdentityUserToken,
                UsersDto<UserDto<string>, string>, RolesDto<RoleDto<string>, string>, UserRolesDto<RoleDto<string>, string, string>,
                UserClaimsDto<string>, UserProviderDto<string>, UserProvidersDto<string>, UserChangePasswordDto<string>,
                RoleClaimsDto<string>, UserClaimDto<string>, RoleClaimDto<string>>(profileTypes);

            services.AddAdminServices<IdentityServerConfigurationDbContext, IdentityServerPersistedGrantDbContext, AdminLogDbContext>();

            services.AddMvcServices<UserDto<string>, string, RoleDto<string>, string, string, string,
                UserIdentity, UserIdentityRole, string, UserIdentityUserClaim, UserIdentityUserRole,
                UserIdentityUserLogin, UserIdentityRoleClaim, UserIdentityUserToken,
                UsersDto<UserDto<string>, string>, RolesDto<RoleDto<string>, string>, UserRolesDto<RoleDto<string>, string, string>,
                UserClaimsDto<string>, UserProviderDto<string>, UserProvidersDto<string>, UserChangePasswordDto<string>,
                RoleClaimsDto<string>>();

            //Mehdi
            services.AddCustomIdentityServices<CustomDbContext, Person, UserIdentity, PersonDto<long, UserDto<string>, string>, long, UserDto<string>,string, PepoleDto<PersonDto<long, UserDto<string>, string>, long, UserDto<string>, string>,long,string>(customProfileTypes);
             services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(ApiConfigurationConsts.ApiVersionV1, new Info { Title = ApiConfigurationConsts.ApiName, Version = ApiConfigurationConsts.ApiVersionV1 });

                options.AddSecurityDefinition("oauth2", new OAuth2Scheme
                {
                    Flow = "implicit",
                    AuthorizationUrl = $"{adminApiConfiguration.IdentityServerBaseUrl}/connect/authorize",
                    Scopes = new Dictionary<string, string> {
                        { adminApiConfiguration.OidcApiName, ApiConfigurationConsts.ApiName }
                    }
                });

                options.OperationFilter<AuthorizeCheckOperationFilter>();
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, AdminApiConfiguration adminApiConfiguration)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", ApiConfigurationConsts.ApiName);

                c.OAuthClientId(adminApiConfiguration.OidcSwaggerUIClientId);
                c.OAuthAppName(ApiConfigurationConsts.ApiName);
            });

            app.UseMvc();
        }
    }
}
