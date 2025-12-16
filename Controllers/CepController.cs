using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaTarefa.Integration.Interfaces;
using SistemaTarefa.Integration.Response;

namespace SistemaTarefa.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CepController : ControllerBase
    {
        // Dependency Injection
        private readonly IViaCepIntegracao _viaCepIntegracao;


        // Constructor
        public CepController(IViaCepIntegracao viaCepIntegracao)
        {
            _viaCepIntegracao = viaCepIntegracao;
        }

        [HttpGet("{cep}")]
        public async Task<ActionResult<ViaCepResponse>> ListarDadosEndereco(string cep)
        {
            var responseData = await _viaCepIntegracao.ObterDadosViaCep(cep);
            if (responseData == null)
            {
                return BadRequest("CEP inválido ou não encontrado.");
            }

            return Ok(responseData);
        }
    }
}