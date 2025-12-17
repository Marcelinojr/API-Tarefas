using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SistemaTarefa.DTOs;

namespace SistemaTarefa.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDTO> CreateToken(AuthDTO dto);
        Task<AuthResponseDTO> UpdateToken(AuthDTO dto);
        Task<bool> ValidateToken(AuthLoginDTO dto);
        Task<AuthResponseDTO> Authenticate(AuthLoginDTO dto);

    }

    
}