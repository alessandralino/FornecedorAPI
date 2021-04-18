using DevIO.Api.Controllers;
using DevIO.Api.ViewModels;
using DevIO.Business.Intefaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DevIO.Api.Configuration
{
    [Route("api")]
    public class AuthController : MainController
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        public AuthController(INotificador notificador,
                              SignInManager<IdentityUser> signManager,
                              UserManager<IdentityUser> userManager) : base(notificador)
        {
            _signInManager = signManager;
            _userManager = userManager;
        }

        [HttpPost("nova-conta")]
        public async Task<ActionResult> Registrar(RegisterUserViewModel registerUser)
        {
            if (!ModelState.IsValid) return customResponse(ModelState);

            var user = new IdentityUser
            {
                UserName = registerUser.Email,
                Email = registerUser.Email,
                EmailConfirmed = true

            };

            var result = await _userManager.CreateAsync(user, registerUser.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                customResponse(registerUser);

            }

            foreach (var error in result.Errors)
            {
                notificarErro(error.Description);
            }

            return customResponse(registerUser);
        }

        [HttpPost("entrar")]
        public async Task<ActionResult> Login(LoginUserViewModel loginUser)
        {
            if (!ModelState.IsValid) return customResponse(ModelState);

            var result = await _signInManager.PasswordSignInAsync(loginUser.Email, loginUser.Password, false, true);

            if (result.Succeeded)
            {
                return customResponse(loginUser);
            }

            if (result.IsLockedOut)
            {
                notificarErro("Usuário temporariamente bloqueado por tentativas inválidas.");
                return customResponse(loginUser);
            }

            notificarErro("Usuário ou Senha incorretos.");
            return customResponse(loginUser);
        }
    }
}
