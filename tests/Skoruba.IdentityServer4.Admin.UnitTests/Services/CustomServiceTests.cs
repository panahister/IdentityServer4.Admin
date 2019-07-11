using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Skoruba.IdentityServer4.Admin.BusinessLogic.Identity.Dtos.Identity;
using Skoruba.IdentityServer4.Admin.BusinessLogic.Identity.Mappers;
using Skoruba.IdentityServer4.Admin.BusinessLogic.Identity.Resources;
using Skoruba.IdentityServer4.Admin.BusinessLogic.Identity.Services;
using Custom.BusinessLogic.Identity.Services.Interfaces;
using Skoruba.IdentityServer4.Admin.EntityFramework.Identity.Repositories;
using Custom.EntityFramework.Identity.Repositories.Interfaces;
using Skoruba.IdentityServer4.Admin.EntityFramework.Shared.DbContexts;
using Skoruba.IdentityServer4.Admin.EntityFramework.Shared.Entities.Identity;
using Skoruba.IdentityServer4.Admin.UnitTests.Mocks;
using Xunit;
using Custom.BusinessLogic.Identity.Dtos;
using Custom.EntityFramework.Entities;
using Custom.EntityFramework.DbContexts;
using Custom.EntityFramework.Identity.Repositories;
using Custom.BusinessLogic.Identity.Services;
using Custom.BusinessLogic.Identity.Mappers;
using Skoruba.IdentityServer4.Admin.UnitTests.Mocks.Custom;

namespace Skoruba.IdentityServer4.Admin.UnitTests.Services
{
    public class CustomServiceTests
    {
        public CustomServiceTests()
        {
            var customDatabaseName = Guid.NewGuid().ToString();

            _dbCustomContextOptions = new DbContextOptionsBuilder<CustomDbContext>()
                .UseInMemoryDatabase(customDatabaseName)
                .Options;

            var databaseName = Guid.NewGuid().ToString();
            _dbContextOptions = new DbContextOptionsBuilder<AdminIdentityDbContext>()
               .UseInMemoryDatabase(databaseName)
               .Options;

            _storeOptions = new ConfigurationStoreOptions();
            _operationalStore = new OperationalStoreOptions();
        }

        private readonly DbContextOptions<CustomDbContext> _dbCustomContextOptions;
        private readonly ConfigurationStoreOptions _storeOptions;
        private readonly OperationalStoreOptions _operationalStore;

        private readonly DbContextOptions<AdminIdentityDbContext> _dbContextOptions;

        private ICustomIdentityRepository<Person, long> GetCustomIdentityRepository(CustomDbContext dbContext,
            IMapper mapper)
        {
            return new CustomIdentityRepository<CustomDbContext, Person, long>(dbContext, mapper);
        }

        private ICustomIdentityService<PersonDto<long, UserDto<string>, string>, long, Person, UserDto<string>, string, long, PepoleDto<PersonDto<long, UserDto<string>, string>, long, UserDto<string>, string>>
            GetCustomIdentityService(ICustomIdentityRepository<Person, long> customIdentityRepository,
            IIdentityServiceResources identityServiceResources,
            IMapper mapper)
        {
            return new CustomIdentityService<PersonDto<long, UserDto<string>, string>, long, Person, UserDto<string>, string, long,
               PepoleDto<PersonDto<long, UserDto<string>, string>, long, UserDto<string>, string>>(customIdentityRepository, identityServiceResources, mapper);

        }

        private IMapper GetMapper()
        {
            return new MapperConfiguration(cfg => cfg.AddProfile<IdentityMapperProfile<UserDto<string>, string, RoleDto<string>, string, UserIdentity, UserIdentityRole, string,
                        UserIdentityUserClaim, UserIdentityUserRole, UserIdentityUserLogin, UserIdentityRoleClaim,
                        UserIdentityUserToken,
                        UsersDto<UserDto<string>, string>, RolesDto<RoleDto<string>, string>, UserRolesDto<RoleDto<string>, string, string>,
                        UserClaimsDto<string>, UserProviderDto<string>, UserProvidersDto<string>,
                        RoleClaimsDto<string>, UserClaimDto<string>, RoleClaimDto<string>>
                >())
                .CreateMapper();
        }
        private IMapper GetCustomMapper()
        {
            return new MapperConfiguration(cfg => cfg.AddProfile<CustomMapperProfile<PersonDto<long, UserDto<string>, string>, long, Person, IdentityUser<string>, UserDto<string>, string, PepoleDto<PersonDto<long, UserDto<string>, string>, long, UserDto<string>, string>, long, string>
                >())
                .CreateMapper();
        }

        private UserManager<UserIdentity> GetTestUserManager(AdminIdentityDbContext context)
        {
            var testUserManager = IdentityMock.TestUserManager(new UserStore<UserIdentity, UserIdentityRole, AdminIdentityDbContext, string, UserIdentityUserClaim, UserIdentityUserRole, UserIdentityUserLogin, UserIdentityUserToken, UserIdentityRoleClaim>(context, new IdentityErrorDescriber()));

            return testUserManager;
        }

        private RoleManager<UserIdentityRole> GetTestRoleManager(AdminIdentityDbContext context)
        {
            var testRoleManager = IdentityMock.TestRoleManager(new RoleStore<UserIdentityRole, AdminIdentityDbContext, string, UserIdentityUserRole, UserIdentityRoleClaim>(context, new IdentityErrorDescriber()));

            return testRoleManager;
        }

        [Fact]
        public async Task AddPersonAsync()
        {
            using (var customContext = new CustomDbContext(_dbCustomContextOptions))
            using (var context = new AdminIdentityDbContext(_dbContextOptions))
            {
                var testUserManager = GetTestUserManager(context);
                var testRoleManager = GetTestRoleManager(context);
                var mapper = GetMapper();
                var CustomMapper = GetCustomMapper();

                var customIdentityRepository = GetCustomIdentityRepository(customContext, CustomMapper);
                var localizerIdentityResource = new IdentityServiceResources();
                var customIdentityService = GetCustomIdentityService(customIdentityRepository, localizerIdentityResource, CustomMapper);

                //Generate random new user and person
                var userDto = IdentityDtoMock<string>.GenerateRandomUser("1");
                var personDto = CustomIdentityDtoMock<long, UserDto<string>, string>.GenerateRandomPerson(1);
                personDto.UserId = userDto.Id;
                personDto.User = userDto;
                await customIdentityService.CreatePersonAsync(personDto);

                //Get new person
                var person = await customContext.Pepole.Where(x => x.FirstName == personDto.FirstName).SingleOrDefaultAsync();
                personDto.Id = person.Id;

                var newPersonDto = await customIdentityService.GetPersonAsync(personDto.Id);

                //Assert new person
                personDto.ShouldBeEquivalentTo(newPersonDto);
            }
        }

        [Fact]
        public async Task GetPersonAsync()
        {
            using (var customContext = new CustomDbContext(_dbCustomContextOptions))
            using (var context = new AdminIdentityDbContext(_dbContextOptions))
            {
                var testUserManager = GetTestUserManager(context);
                var testRoleManager = GetTestRoleManager(context);
                var mapper = GetMapper();
                var CustomMapper = GetCustomMapper();

                var customIdentityRepository = GetCustomIdentityRepository(customContext, CustomMapper);
                var localizerIdentityResource = new IdentityServiceResources();
                var customIdentityService = GetCustomIdentityService(customIdentityRepository, localizerIdentityResource, CustomMapper);

                //Generate random new user and person
                var userDto = IdentityDtoMock<string>.GenerateRandomUser("1");
                var personDto = CustomIdentityDtoMock<long, UserDto<string>, string>.GenerateRandomPerson(1);
                personDto.UserId = userDto.Id;
                personDto.User = userDto;
                await customIdentityService.CreatePersonAsync(personDto);

                var pepole = await customIdentityService.GetPersonAsync();

                pepole.Pepole.Count.ShouldBeEquivalentTo(1);
            }
        }

        [Fact]
        public async Task DeletePersonAsync()
        {
            using (var customContext = new CustomDbContext(_dbCustomContextOptions))
            using (var context = new AdminIdentityDbContext(_dbContextOptions))
            {
                var testUserManager = GetTestUserManager(context);
                var testRoleManager = GetTestRoleManager(context);
                var mapper = GetMapper();
                var CustomMapper = GetCustomMapper();

                var customIdentityRepository = GetCustomIdentityRepository(customContext, CustomMapper);
                var localizerIdentityResource = new IdentityServiceResources();
                var customIdentityService = GetCustomIdentityService(customIdentityRepository, localizerIdentityResource, CustomMapper);

                //Generate random new user and person
                var userDto = IdentityDtoMock<string>.GenerateRandomUser("1");
                var personDto = CustomIdentityDtoMock<long, UserDto<string>, string>.GenerateRandomPerson(1);
                personDto.UserId = userDto.Id;
                personDto.User = userDto;
                await customIdentityService.CreatePersonAsync(personDto);

                //Get new person
                var newPersonDto = await customIdentityService.GetPersonAsync(personDto.Id);

                var deleteStatus = await customIdentityService.DeletePersonAsync(personDto.Id);
                deleteStatus.ShouldBeEquivalentTo(1);

                var pepole = await customIdentityService.GetPersonAsync();
                //Assert new user
                pepole.Pepole.Count.ShouldBeEquivalentTo(0);
            }
        }

        [Fact]
        public async Task UpdatePersonAsync()
        {
            using (var customContext = new CustomDbContext(_dbCustomContextOptions))
            using (var context = new AdminIdentityDbContext(_dbContextOptions))
            {
                var testUserManager = GetTestUserManager(context);
                var testRoleManager = GetTestRoleManager(context);
                var mapper = GetMapper();
                var CustomMapper = GetCustomMapper();

                var customIdentityRepository = GetCustomIdentityRepository(customContext, CustomMapper);
                var localizerIdentityResource = new IdentityServiceResources();
                var customIdentityService = GetCustomIdentityService(customIdentityRepository, localizerIdentityResource, CustomMapper);

                //Generate random new user and peron
                var userDto = IdentityDtoMock<string>.GenerateRandomUser("1");
                var personDto = CustomIdentityDtoMock<long, UserDto<string>, string>.GenerateRandomPerson(1);
                personDto.UserId = userDto.Id;
                personDto.User = userDto;
                await customIdentityService.CreatePersonAsync(personDto);

                //Get new person
                var newPersonDto = await customIdentityService.GetPersonAsync(personDto.Id);
                newPersonDto.FirstName = "Mehdi";
                newPersonDto.LastName = "Panahi";
                var updatedStatus = await customIdentityService.UpdatePersonAsync(newPersonDto);

                var person = await customContext.Pepole.Where(x => x.FirstName == newPersonDto.FirstName && x.LastName == newPersonDto.LastName).SingleOrDefaultAsync();

                //Assert new user
                person.FirstName.ShouldBeEquivalentTo(newPersonDto.FirstName);
            }
        }
    }
}
