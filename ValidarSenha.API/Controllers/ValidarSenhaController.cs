using System;
using System.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ValidarSenha.Core.Services;
using ValidarSenha.Core.Model;
using Microsoft.Net.Http.Headers;

namespace ValidarSenha.API.Controllers
{
    [ApiController]
    [Route("ValidarSenha")]
    public class ValidarSenhaController : ControllerBase
    {
        private readonly ILogger<ValidarSenhaController> _logger;
        private readonly ValidaSenha _validaSenha;

        public ValidarSenhaController(ILogger<ValidarSenhaController> logger)
        {
            _logger = logger;
            _validaSenha = new ValidaSenha();
        }

        [HttpGet]
        [Route("validar/{senha}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Get(string senha)
        {
            SenhaResponse response =  new SenhaResponse();
            SecureString secureString = new SecureString();
            foreach (char ch in senha) secureString.AppendChar(ch);
            senha = string.Empty;
            try
            {
                response = _validaSenha.ExecutaValidacao(secureString);
                return Ok(response);
            }
            catch(Exception ex)
            {
                response.senhaValida = false;
                response.mensagem = ex.Message.ToString();
                return StatusCode(500, response);

            }
        }
    }
}
