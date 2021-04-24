using DevIO.Business.Intefaces;
using DevIO.Business.Notificacoes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Linq;

namespace DevIO.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class MainController : ControllerBase
    {
        private readonly INotificador _notificador;
        public readonly IUser AppUser;

        protected Guid UsuarioId { get; set; }
        protected bool UsuarioAutenticado { get; set; }

        public MainController(INotificador notificador, IUser appUser)
        {
            _notificador = notificador;
            AppUser = appUser;

            if (AppUser.IsAuthenticated())
            {
                UsuarioId = AppUser.GetUserId();
                UsuarioAutenticado = true;
            }

        }

        protected bool operacaoValida() 
        {
            return !_notificador.TemNotificacao();
        }

        protected ActionResult customResponse(object result = null)
        {
            if(operacaoValida())
            {
                return Ok(new
                {
                    success = true,
                    data = result
                }); 
            }


            return BadRequest(new
            {
                success = false,
                errors = _notificador.ObterNotificacoes().Select(n => n.Mensagem)
            }); 
            
        }


        protected ActionResult customResponse(ModelStateDictionary modelState) 
        {
            if (!modelState.IsValid) notificarErroModelInvalida(modelState);
            return customResponse();
        }

        protected void notificarErroModelInvalida(ModelStateDictionary modelState)
        {
            var erros = modelState.Values.SelectMany(e => e.Errors);

            foreach (var erro in erros)
            {
                var errorMsg = erro.Exception == null ? erro.ErrorMessage : erro.Exception.Message;
                notificarErro(errorMsg);
            }
        }

        protected void notificarErro(string mensagem)
        {
            _notificador.Handle(new Notificacao(mensagem));

        }
    }
}
