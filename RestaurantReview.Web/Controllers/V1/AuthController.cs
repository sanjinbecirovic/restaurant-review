using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RestaurantReview.DataAccess.Entities;
using RestaurantReview.Web.Extensions;
using RestaurantReview.Web.Infrastructure;
using RestaurantReview.Web.Infrastructure.Interfaces;
using RestaurantReview.Web.Models.Request;

namespace RestaurantReview.Web.Controllers.V1
{
    [AllowAnonymous]
    public class AuthController : ApiControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IJwtTokenService jwtTokenService;

        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, IJwtTokenService jwtTokenService, IMapper mapper)
            : base(mapper)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.jwtTokenService = jwtTokenService;
        }

        [HttpPost("register")]
        [ProducesResponseType(typeof(JwtToken), 200)]
        public async Task<IActionResult> Register(RegisterRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if email exists
            var user = await this.userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                ModelState.AddModelError(nameof(model.Email), "User with given e-mail already exists.");
                return BadRequest(ModelState);
            }

            user = new User
            {
                Email = model.Email,
                UserName = model.Email.Split('@')[0]
            };

            var createResult = await this.userManager.CreateAsync(user, model.Password);
            if (!string.IsNullOrEmpty(model.Role))
            {
                await this.userManager.AddToRoleAsync(user, model.Role);
            }

            if (!createResult.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, createResult.Errors);
            }

            var token = await this.jwtTokenService.GenerateTokenAsync(user);
            return Ok(new JwtToken
            {
                Token = this.jwtTokenService.WriteToken(token),
                ValidTo = token.ValidTo
            });
        }

        [Authorize]
        [HttpPost("lockout")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Lockout(LockoutRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await this.userManager.FindByIdAsync(User.GetUserId());
            var roles = await this.userManager.GetRolesAsync(user);

            if (!roles.Contains(UserRoles.Admin))
            {
                return Unauthorized();
            }

            var userToLockout = await this.userManager.FindByIdAsync(model.UserId);
            if (model.Enabled) await this.userManager.SetLockoutEndDateAsync(userToLockout, DateTimeOffset.UtcNow.AddMonths(1));
            else await this.userManager.SetLockoutEndDateAsync(userToLockout, null);

            return Ok();
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(JwtToken), 200)]
        public async Task<IActionResult> Login(LoginRequestModel model)
        {
            // Try to load user by username or email
            var user = await this.userManager.FindByEmailAsync(model.Username);
            if (user == null)
            {
                user = await this.userManager.FindByNameAsync(model.Username);
            }

            // If user doesn't exist return error
            if (user == null)
            {
                ModelState.AddModelError(nameof(model.Username), "User with a given username or email doesn't exist.");
                return BadRequest(ModelState);
            }

            // Try to login user
            var signInResult = await this.signInManager.CheckPasswordSignInAsync(user, model.Password, false);

            if (!signInResult.Succeeded)
            {
                if (signInResult.IsLockedOut)
                {
                    ModelState.AddModelError("IsLockedOut", "You have been locked by the administrator.");
                    return BadRequest(ModelState);
                }
                else
                {
                    ModelState.AddModelError(nameof(model.Password), "Given password is not correct.");
                    return BadRequest(ModelState);
                }
            }

            var token = await this.jwtTokenService.GenerateTokenAsync(user);
            return Ok(new JwtToken
            {
                Token = this.jwtTokenService.WriteToken(token),
                ValidTo = token.ValidTo
            });
        }
    }
}
