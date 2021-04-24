using System;
using System.Collections.Generic;
using System.Security.Claims;
using DevIO.Business.Intefaces;
using Microsoft.AspNetCore.Http;

namespace DevIO.Api.Extensions
{
    public class AspNetUser : IUser
    {

        private readonly IHttpContextAccessor _accessor;
        /* 
         * Ao implementar IUser, que é uma interface que se encontra na camada de negócio,
         * e injetar a dependência de IHttpContextAccessor obtemos informações do usuário em qualquer ponto
         * da aplicação sem gerar dependência do ASP.NET Core caso fosse necessário. 
         * Ao utilizar essa abordagem estou aplicando o Princípio da Inversão de Dependência. 
         */

        public AspNetUser(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public string Name => _accessor.HttpContext.User.Identity.Name;

        public Guid GetUserId()
        {
            return IsAuthenticated() ? Guid.Parse(_accessor.HttpContext.User.GetUserId()) : Guid.Empty;
        }

        public string GetUserEmail()
        {
            return IsAuthenticated() ? _accessor.HttpContext.User.GetUserEmail() : "";
        }

        public bool IsAuthenticated()
        {
            return _accessor.HttpContext.User.Identity.IsAuthenticated;
        }

        public bool IsInRole(string role)
        {
            return _accessor.HttpContext.User.IsInRole(role);
        }

        public IEnumerable<Claim> GetClaimsIdentity()
        {
            return _accessor.HttpContext.User.Claims;
        }
    }

    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserId(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentException(nameof(principal));
            }

            var claim = principal.FindFirst(ClaimTypes.NameIdentifier);
            return claim?.Value;
        }

        public static string GetUserEmail(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentException(nameof(principal));
            }

            var claim = principal.FindFirst(ClaimTypes.Email);
            return claim?.Value;
        }
    }
}