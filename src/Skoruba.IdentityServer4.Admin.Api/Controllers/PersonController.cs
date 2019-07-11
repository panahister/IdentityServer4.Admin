using System;
using System.Threading.Tasks;
using AutoMapper;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Skoruba.IdentityServer4.Admin.Api.Configuration.Constants;
using Skoruba.IdentityServer4.Admin.Api.Dtos.Users;
using Skoruba.IdentityServer4.Admin.Api.ExceptionHandling;
using Skoruba.IdentityServer4.Admin.Api.Helpers.Localization;
using Skoruba.IdentityServer4.Admin.BusinessLogic.Identity.Dtos.Identity;
using Skoruba.IdentityServer4.Admin.BusinessLogic.Identity.Services.Interfaces;
using Custom.BusinessLogic.Identity.Dtos;
using Custom.BusinessLogic.Identity.Services.Interfaces;
using Custom.EntityFramework.Entities;

namespace Skoruba.IdentityServer4.Admin.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [TypeFilter(typeof(ControllerExceptionFilterAttribute))]
    [Produces("application/json")]
    [Authorize(AuthenticationSchemes = IdentityServerAuthenticationDefaults.AuthenticationScheme, Policy = AuthorizationConsts.AdministrationPolicy)]
    public class PersonController<TPersonDto, TPersonDtoKey, TPerson, TUserDto, TUserDtoKey,
            TKey, TPepoleDto> : ControllerBase

            where TUserDto : UserDto<TUserDtoKey>, new()
          where TPersonDto : PersonDto<TKey, TUserDto, TUserDtoKey>, new()
            where TPerson : Person
            where TKey : IEquatable<TKey>
            where TPepoleDto : PepoleDto<TPersonDto, TKey, TUserDto, TUserDtoKey>
    {
        private readonly ICustomIdentityService<TPersonDto, TPersonDtoKey, TPerson, TUserDto, TUserDtoKey, TKey, TPepoleDto> _customIdentityService;


        private readonly IMapper _mapper;

        public PersonController(ICustomIdentityService<TPersonDto, TPersonDtoKey, TPerson, TUserDto, TUserDtoKey, TKey, TPepoleDto> customIdentityService , IMapper mapper)
        {
            _customIdentityService = customIdentityService;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TPersonDto>> Get(TKey id)
        {
            var user = await _customIdentityService.GetPersonAsync(Convert.ToInt64( id));
           
            return Ok(user);
        }

        [HttpGet]
        public async Task<ActionResult<TPepoleDto>> Get( int page = 1, int pageSize = 10)
        {
            var usersDto = await _customIdentityService.GetPersonAsync( page, pageSize);

            return Ok(usersDto);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]TPersonDto person)
        {
            await _customIdentityService.CreatePersonAsync(person);

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody]TPersonDto person)
        {
            await _customIdentityService.UpdatePersonAsync(person);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(TKey id)
        {
            await _customIdentityService.DeletePersonAsync(Convert.ToInt64(id));

            return Ok();
        }

    }
}