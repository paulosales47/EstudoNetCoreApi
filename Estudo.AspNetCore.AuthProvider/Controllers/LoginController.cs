using Alura.ListaLeitura.Seguranca;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Estudo.AspNetCore.AuthProvider.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly SignInManager<Usuario> _signInManager;
        private readonly IConfiguration _configuration;

        public LoginController(SignInManager<Usuario> signInManager, IConfiguration configuration)
        {
            _signInManager = signInManager;
            _configuration = configuration;
        }

        [HttpPost("Authorize")]
        public async Task<IActionResult> Authorize([FromBody] LoginModel model)
        {
            if (ModelState.IsValid)
            {
                SignInResult result = await _signInManager.PasswordSignInAsync(model.Login, model.Password, true, true);

                if (result.Succeeded)
                {
                    var direitos = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, model.Login),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    };

                    var chaveToken = _configuration.GetSection("Configuracao").GetSection("ChaveToken").Value;
                    var chave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(chaveToken));

                    var credenciais = new SigningCredentials(chave, SecurityAlgorithms.HmacSha512);

                    var token = new JwtSecurityToken
                    (
                        issuer: "Alura.WebApp",
                        audience: "Postman",
                        claims: direitos,
                        signingCredentials: credenciais,
                        expires: DateTime.Now.AddMinutes(20)
                    );

                    string tokenString = new JwtSecurityTokenHandler().WriteToken(token);
                    return Ok(tokenString);

                }
                return Unauthorized();
            }
            return BadRequest();
        }
    }
}