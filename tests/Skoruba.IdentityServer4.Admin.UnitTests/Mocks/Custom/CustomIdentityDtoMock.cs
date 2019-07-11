using System;
using Bogus;
using Custom.BusinessLogic.Identity.Dtos;
using Skoruba.IdentityServer4.Admin.BusinessLogic.Identity.Dtos.Identity;

namespace Skoruba.IdentityServer4.Admin.UnitTests.Mocks.Custom
{
    public class CustomIdentityDtoMock<TKey, TUserDto, TUserDtoKey>
    {
        public static Faker<PersonDto<TKey, TUserDto, TUserDtoKey>> GetPersonFaker(TKey id)
        {
            var personFaker = new Faker<PersonDto<TKey, TUserDto, TUserDtoKey>>()
                .RuleFor(o => o.Id, id)
                .RuleFor(o => o.FirstName, f => Guid.NewGuid().ToString())
                .RuleFor(o => o.LastName, f => f.Internet.Email())
                .RuleFor(o => o.Gender, Gender.Female)
                .RuleFor(o => o.NationalCode, Guid.NewGuid().ToString());

            return personFaker;
        }


        public static PersonDto<TKey, TUserDto, TUserDtoKey> GenerateRandomPerson(TKey id = default(TKey))
        {
            var peson = GetPersonFaker(id).Generate();

            return peson;
        }
    }
}
