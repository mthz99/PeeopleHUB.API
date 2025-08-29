using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeopleHub.Application.DTOs;
using PeopleHub.Application.Features.People.Commands;
using PeopleHub.Application.Features.People.Queries;
using PeopleHub.API.Mappers;

namespace PeopleHub.API.Controllers.version1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    public class PeopleHubController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PeopleHubController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Obtém todas as pessoas
        /// </summary>
        /// <returns>Lista de pessoas</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PersonDtoV1>>> GetAll()
        {
            return await ExecuteWithErrorHandling(async () =>
            {
                IEnumerable<PersonDtoV1> result = await _mediator.Send(new GetAllPeopleQuery());
                return Ok(result);
            });
        }

        /// <summary>
        /// Obtém uma pessoa por ID
        /// </summary>
        /// <param name="id">ID da pessoa</param>
        /// <returns>Dados da pessoa</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<PersonDtoV1>> GetById(int id)
        {
            return await ExecuteWithErrorHandling(async () =>
            {
                PersonDtoV1? result = await _mediator.Send(new GetPersonByIdQuery(id));
                return result == null ? NotFound(new { message = "Pessoa não encontrada" }) : Ok(result);
            });
        }

        /// <summary>
        /// Cria uma nova pessoa (CRUD Administrativo - SEM criar usuário)
        /// Para criar conta de usuário com acesso ao sistema, use POST /api/Auth/register
        /// </summary>
        /// <param name="personDto">Dados da pessoa</param>
        /// <returns>Pessoa criada</returns>
        [HttpPost]
        public async Task<ActionResult<PersonDtoV1>> Create([FromBody] PersonDtoV1 personDto)
        {
            return await ExecuteWithErrorHandling(async () =>
            {
                CreatePersonCommand command = personDto.ToCreateCommand();
                PersonDtoV1 result = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            });
        }

        /// <summary>
        /// Atualiza uma pessoa existente
        /// </summary>
        /// <param name="id">ID da pessoa</param>
        /// <param name="personDto">Dados atualizados da pessoa</param>
        /// <returns>Pessoa atualizada</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<PersonDtoV1>> Update(int id, [FromBody] PersonDtoV1 personDto)
        {
            return await ExecuteWithErrorHandling(async () =>
            {
                UpdatePersonCommand command = personDto.ToUpdateCommand(id);
                PersonDtoV1 result = await _mediator.Send(command);
                return Ok(result);
            });
        }

        /// <summary>
        /// Remove uma pessoa
        /// </summary>
        /// <param name="id">ID da pessoa</param>
        /// <returns>Resultado da operação</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            return await ExecuteWithErrorHandling(async () =>
            {
                bool result = await _mediator.Send(new DeletePersonCommand(id));
                return result ? NoContent() : NotFound(new { message = "Pessoa não encontrada" });
            });
        }

        #region Private Helper Methods

        private async Task<ActionResult> ExecuteWithErrorHandling(Func<Task<ActionResult>> action)
        {
            try
            {
                return await action();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno do servidor", details = ex.Message });
            }
        }

        #endregion
    }
}
