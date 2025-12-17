using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SistemaTarefa.DTOs;
using SistemaTarefa.Models;
using SistemaTarefa.Repositories.Interfaces;
using SistemaTarefa.Services.Interfaces;

namespace SistemaTarefa.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDTO>> Login([FromBody] AuthLoginDTO dto)
        {
            if (dto == null)
            {
                return BadRequest("Invalid request payload.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var auth = await _authService.Authenticate(dto);

                if (auth == null)
                {
                    return Unauthorized("Invalid username or password.");
                }

                return Ok(auth);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message});
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while processing the login request.");
            }
        }
    }
}