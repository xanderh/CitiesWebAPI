using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using CitiesWebAPI.Filters;
using CitiesWebAPI.Models;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace CitiesWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private DataContext _context;
        private SignInManager<User> _signInManager;
        private UserManager<User> _userManager;
        private IPasswordHasher<User> _hasher;
        private IConfigurationRoot _config;
        private ILogger<AuthController> _logger;

        public AuthController(DataContext context, 
            SignInManager<User> signInManager, 
            UserManager<User> userManager, 
            IPasswordHasher<User> hasher,
            IConfigurationRoot config,
            ILogger<AuthController> logger)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
            _hasher = hasher;
            _config = config;
            _logger = logger;
        }

        [HttpPost("login")]
        [ValidateModel]
        public async Task<IActionResult> Login([FromBody]CredentialModel model)
        {
            try
            {
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);
                if (result.Succeeded) return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception thrown while logging in: {e}");
            }

            return BadRequest("Failed to log in");
        }

        [ValidateModel]
        [HttpPost("token")]
        public async Task<IActionResult> CreateToken([FromBody]CredentialModel model)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(model.UserName);
                if(user != null)
                {
                    if(_hasher.VerifyHashedPassword(user, user.PasswordHash, model.Password) == PasswordVerificationResult.Success)
                    {
                        var claims = new[]
                        {
                            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                        };

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
                        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                        var token = new JwtSecurityToken(
                            issuer: _config["Tokens: Issuer"],
                            audience: _config["Tokens:Audience"],
                            claims: claims,
                            expires: DateTime.Now.AddMinutes(5),
                            signingCredentials: creds
                            );
                        return Ok(new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo
                        });
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception thrown while creating JWT: {e}");
            }
            return BadRequest("Failed to generate token");
        }
    }
}