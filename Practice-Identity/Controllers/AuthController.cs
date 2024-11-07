using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Practice_Identity.Context;
using Practice_Identity.Dtos;
using Practice_Identity.Entities;
using Practice_Identity.Models;

namespace Practice_Identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ApplicationDbContext _applicationDbContext;

        public AuthController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _applicationDbContext = context;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            if (ModelState.IsValid)
            {
                var identityUser = new IdentityUser
                {
                    UserName = user.Email,
                    Email = user.Email,
                };

                var result = await _userManager.CreateAsync(identityUser, user.Password);

                if (result.Succeeded)
                {
                    user.Password = identityUser.PasswordHash;

                    _applicationDbContext.Users.Add(user);

                    await _applicationDbContext.SaveChangesAsync();


                    return Ok(new { message = "The registration was successful." });
                }

                else
                {
                    return BadRequest(result.Errors);
                }
            }

            return BadRequest(new { Messages = ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage) });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

                if (result.Succeeded)
                {
                    return Ok(new { message = "The login was successful." });
                }
                else
                {
                    return Unauthorized(new { message = "Wrong email or password" });
                }
            }
            return BadRequest(new { Messages = ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage) });
        }
    }
}

