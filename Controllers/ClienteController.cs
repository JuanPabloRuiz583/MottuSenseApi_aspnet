using Microsoft.AspNetCore.Mvc;
using Sprint.Data;
using Sprint.Dtos;
using Sprint.Models;
using Sprint.Services;
using System;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace Sprint.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteService _clienteService;

        public ClienteController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        /// <summary>
        /// Obtém uma lista de todos os clientes.
        /// </summary>
        /// <remarks>
        /// Exemplo de requisição:
        ///
        ///     GET /api/Cliente
        ///
        /// </remarks>
        /// <returns>Uma lista de clientes.</returns>
        /// <response code="200">Retorna a lista completa de clientes.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Cliente>))]
        public IActionResult GetAll()
        {
            var clientes = _clienteService.GetAll();
            return Ok(clientes);
        }

        /// <summary>
        /// Retorna um cliente específico por ID.
        /// </summary>
        /// <param name="id">ID do cliente.</param>
        /// <remarks>
        /// Exemplo de requisição:
        ///
        ///     GET /api/Cliente/1
        ///
        /// </remarks>
        /// <returns>O cliente encontrado ou NotFound.</returns>
        /// <response code="200">Retorna o cliente.</response>
        /// <response code="404">Cliente não encontrado.</response>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Cliente))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetById(long id)
        {
            var cliente = _clienteService.GetById(id);
            if (cliente == null) return NotFound();
            return Ok(cliente);
        }


        /// <summary>
        /// Cria um novo cliente.
        /// </summary>
        /// <param name="clienteDto">Objeto do cliente a ser criado.</param>
        /// <remarks>
        /// O ID do cliente é gerado automaticamente.
        /// Exemplo de requisição:
        ///
        ///     POST /api/Cliente
        ///     {
        ///         "nome": "João da Silva",
        ///         "email": "joao@email.com",
        ///         "senha": "senha1234"
        ///     }
        /// </remarks>
        /// <returns>O cliente recém-criado, incluindo o ID.</returns>
        /// <response code="201">Retorna o cliente recém-criado.</response>
        /// <response code="400">Se o cliente for nulo ou inválido.</response>
        /// <response code="409">Se já existir um cliente com o mesmo e-mail.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Cliente))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public IActionResult Create([FromBody] ClienteDTO clienteDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var (cliente, error) = _clienteService.Create(clienteDto);
            if (error == "um cliente com esse email ja existe")
                return Conflict(new { message = error });
            if (cliente == null)
                return BadRequest();

            return CreatedAtAction(nameof(GetById), new { id = cliente.Id }, cliente);
        }

       

        /// <summary>
        /// Remove um cliente pelo ID.
        /// </summary>
        /// <param name="id">ID do cliente a ser removido.</param>
        /// <remarks>
        /// Exemplo de requisição:
        ///
        ///     DELETE /api/Cliente/1
        ///
        /// </remarks>
        /// <response code="204">Cliente removido com sucesso.</response>
        /// <response code="404">Cliente não encontrado.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(long id)
        {
            var deleted = _clienteService.Delete(id);
            if (!deleted) return NotFound();
            return NoContent();
        }









        /// <summary>
        /// Realiza o login do cliente e retorna um token JWT.
        /// </summary>
        /// <param name="loginDto">Credenciais de login (email e senha).</param>
        /// <remarks>
        /// Exemplo de requisição:
        ///
        ///     POST /api/Cliente/login
        ///     {
        ///         "email": "joao@email.com",
        ///         "senha": "senha1234"
        ///     }
        ///
        /// Exemplo de resposta (200):
        /// {
        ///     "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
        ///     "cliente": {
        ///         "id": 1,
        ///         "nome": "João da Silva",
        ///         "email": "joao@email.com"
        ///     }
        /// }
        /// </remarks>
        /// <returns>Token JWT e dados do cliente autenticado.</returns>
        /// <response code="200">Login realizado com sucesso. Retorna o token JWT e os dados do cliente.</response>
        /// <response code="401">Email ou senha inválidos.</response>
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDTO loginDto)
        {
            var cliente = _clienteService.Authenticate(loginDto.Email, loginDto.Senha);
            if (cliente == null)
                return Unauthorized(new { message = "Email ou senha inválidos" });

            // Geração do token JWT
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("minha-chave-super-secreta-1234567890"); // Troque por uma chave forte e segura
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim(ClaimTypes.NameIdentifier, cliente.Id.ToString()),
            new Claim(ClaimTypes.Email, cliente.Email)
        }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
                        return Ok(new
            {
                token = tokenHandler.WriteToken(token),
                cliente = new { cliente.Id, cliente.Nome, cliente.Email }
            });
        }
    }
}
