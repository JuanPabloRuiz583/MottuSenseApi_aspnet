using Microsoft.AspNetCore.Mvc;
using Sprint.Data;
using Sprint.Dtos;
using Sprint.Models;
using Sprint.Services;
using System;
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
        /// Atualiza um cliente existente.
        /// </summary>
        /// <param name="id">ID do cliente a ser atualizado.</param>
        /// <param name="clienteDto">Objeto cliente com os dados atualizados.</param>
        /// <remarks>
        /// Exemplo de requisição:
        ///
        ///     PUT /api/Cliente/1
        ///     {
        ///         "id": 1,
        ///         "nome": "João da Silva Atualizado",
        ///         "email": "joao@email.com",
        ///         "senha": "novasenha123"
        ///     }
        /// </remarks>
        /// <returns>O cliente atualizado.</returns>
        /// <response code="200">Retorna o cliente atualizado.</response>
        /// <response code="400">Se o corpo da requisição for inválido ou IDs não coincidirem.</response>
        /// <response code="404">Cliente não encontrado.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Cliente))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Update(long id, [FromBody] ClienteDTO clienteDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var (cliente, error) = _clienteService.Update(id, clienteDto);
            if (error == "ID do corpo não corresponde ao da URL")
                return BadRequest();
            if (error == "Cliente não encontrado")
                return NotFound();

            return Ok(cliente);
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
    }
}
