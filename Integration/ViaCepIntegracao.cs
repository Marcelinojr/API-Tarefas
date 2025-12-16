using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SistemaTarefa.Integration.Interfaces;
using SistemaTarefa.Integration.Refit;
using SistemaTarefa.Integration.Response;

namespace SistemaTarefa.Integration
{
    public class ViaCepIntegracao : IViaCepIntegracao
    {
        // Refit interface instance
        private readonly IViaCepIntegracaoRefit _viaCepIntegracaoRefit;
        
        // Constructor
        public ViaCepIntegracao(IViaCepIntegracaoRefit viaCepIntegracaoRefit)
        {
            _viaCepIntegracaoRefit = viaCepIntegracaoRefit;
        }

        // Method to get ViaCep data
        public async Task<ViaCepResponse> ObterDadosViaCep(string cep)
        {

            var responseData =await _viaCepIntegracaoRefit.ObterDadosViaCep(cep);

            if (responseData != null && responseData.IsSuccessStatusCode)
            {
                return responseData.Content;
            }
            return null;
        }
    }
}