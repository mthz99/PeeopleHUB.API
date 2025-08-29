using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PeopleHub.API.Controllers.version1;
using PeopleHub.Application.DTOs;
using PeopleHub.Application.Features.People.Commands;
using PeopleHub.Application.Features.People.Queries;

namespace PeopleHub.Test.Controllers.Version1
{
    public class PeopleHubControllerV1Tests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly PeopleHubController _controller;

        public PeopleHubControllerV1Tests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new PeopleHubController(_mediatorMock.Object);
        }

        #region GetAll Tests

        [Fact]
        public async Task GetAll_WithExistingPeople_ShouldReturnOkWithPeopleList()
        {
            // Arrange
            IEnumerable<PersonDtoV1> expectedPeople = new List<PersonDtoV1>
            {
                new PersonDtoV1
                {
                    Id = 1,
                    Nome = "João Silva",
                    Sexo = "M",
                    Email = "joao@email.com",
                    DataNascimento = new DateTime(1990, 5, 15),
                    CPF = "12345678901",
                    CreatedAt = DateTime.UtcNow
                },
                new PersonDtoV1
                {
                    Id = 2,
                    Nome = "Maria Santos",
                    Sexo = "F",
                    Email = "maria@email.com",
                    DataNascimento = new DateTime(1985, 8, 22),
                    CPF = "98765432109",
                    CreatedAt = DateTime.UtcNow
                }
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetAllPeopleQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedPeople);

            // Act
            ActionResult<IEnumerable<PersonDtoV1>> result = await _controller.GetAll();

            // Assert
            result.Should().NotBeNull();
            OkObjectResult okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            IEnumerable<PersonDtoV1> actualPeople = okResult.Value.Should().BeAssignableTo<IEnumerable<PersonDtoV1>>().Subject;
            actualPeople.Should().HaveCount(2);
            actualPeople.Should().BeEquivalentTo(expectedPeople);

            _mediatorMock.Verify(m => m.Send(It.IsAny<GetAllPeopleQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetAll_WithNoPeople_ShouldReturnOkWithEmptyList()
        {
            // Arrange
            IEnumerable<PersonDtoV1> expectedPeople = new List<PersonDtoV1>();

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetAllPeopleQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedPeople);

            // Act
            ActionResult<IEnumerable<PersonDtoV1>> result = await _controller.GetAll();

            // Assert
            result.Should().NotBeNull();
            OkObjectResult okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            IEnumerable<PersonDtoV1> actualPeople = okResult.Value.Should().BeAssignableTo<IEnumerable<PersonDtoV1>>().Subject;
            actualPeople.Should().BeEmpty();

            _mediatorMock.Verify(m => m.Send(It.IsAny<GetAllPeopleQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetAll_WithException_ShouldReturnInternalServerError()
        {
            // Arrange
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetAllPeopleQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Database connection error"));

            // Act
            ActionResult<IEnumerable<PersonDtoV1>> result = await _controller.GetAll();

            // Assert
            result.Should().NotBeNull();
            ObjectResult objectResult = result.Result.Should().BeOfType<ObjectResult>().Subject;
            objectResult.StatusCode.Should().Be(500);
            dynamic responseValue = objectResult.Value!;
            string message = responseValue.GetType().GetProperty("message")?.GetValue(responseValue)?.ToString() ?? "";
            string details = responseValue.GetType().GetProperty("details")?.GetValue(responseValue)?.ToString() ?? "";
            message.Should().Be("Erro interno do servidor");
            details.Should().Be("Database connection error");

            _mediatorMock.Verify(m => m.Send(It.IsAny<GetAllPeopleQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        #endregion

        #region GetById Tests

        [Fact]
        public async Task GetById_WithExistingId_ShouldReturnOkWithPerson()
        {
            // Arrange
            int personId = 1;
            PersonDtoV1 expectedPerson = new PersonDtoV1
            {
                Id = personId,
                Nome = "João Silva",
                Sexo = "M",
                Email = "joao@email.com",
                DataNascimento = new DateTime(1990, 5, 15),
                CPF = "12345678901",
                CreatedAt = DateTime.UtcNow
            };

            _mediatorMock
                .Setup(m => m.Send(It.Is<GetPersonByIdQuery>(q => q.Id == personId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedPerson);

            // Act
            ActionResult<PersonDtoV1> result = await _controller.GetById(personId);

            // Assert
            result.Should().NotBeNull();
            OkObjectResult okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            PersonDtoV1 actualPerson = okResult.Value.Should().BeOfType<PersonDtoV1>().Subject;
            actualPerson.Should().BeEquivalentTo(expectedPerson);

            _mediatorMock.Verify(m => m.Send(It.Is<GetPersonByIdQuery>(q => q.Id == personId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetById_WithNonExistingId_ShouldReturnNotFound()
        {
            // Arrange
            int personId = 999;

            _mediatorMock
                .Setup(m => m.Send(It.Is<GetPersonByIdQuery>(q => q.Id == personId), It.IsAny<CancellationToken>()))
                .ReturnsAsync((PersonDtoV1?)null);

            // Act
            ActionResult<PersonDtoV1> result = await _controller.GetById(personId);

            // Assert
            result.Should().NotBeNull();
            NotFoundObjectResult notFoundResult = result.Result.Should().BeOfType<NotFoundObjectResult>().Subject;
            dynamic responseValue = notFoundResult.Value!;
            string message = responseValue.GetType().GetProperty("message")?.GetValue(responseValue)?.ToString() ?? "";
            message.Should().Be("Pessoa não encontrada");

            _mediatorMock.Verify(m => m.Send(It.Is<GetPersonByIdQuery>(q => q.Id == personId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetById_WithException_ShouldReturnInternalServerError()
        {
            // Arrange
            int personId = 1;

            _mediatorMock
                .Setup(m => m.Send(It.Is<GetPersonByIdQuery>(q => q.Id == personId), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Database connection error"));

            // Act
            ActionResult<PersonDtoV1> result = await _controller.GetById(personId);

            // Assert
            result.Should().NotBeNull();
            ObjectResult objectResult = result.Result.Should().BeOfType<ObjectResult>().Subject;
            objectResult.StatusCode.Should().Be(500);
            dynamic responseValue = objectResult.Value!;
            string message = responseValue.GetType().GetProperty("message")?.GetValue(responseValue)?.ToString() ?? "";
            string details = responseValue.GetType().GetProperty("details")?.GetValue(responseValue)?.ToString() ?? "";
            message.Should().Be("Erro interno do servidor");
            details.Should().Be("Database connection error");

            _mediatorMock.Verify(m => m.Send(It.Is<GetPersonByIdQuery>(q => q.Id == personId), It.IsAny<CancellationToken>()), Times.Once);
        }

        #endregion

        #region Create Tests

        [Fact]
        public async Task Create_WithValidData_ShouldReturnCreatedAtAction()
        {
            // Arrange
            PersonDtoV1 inputPerson = new PersonDtoV1
            {
                Nome = "João Silva",
                Sexo = "M",
                Email = "joao@email.com",
                DataNascimento = new DateTime(1990, 5, 15),
                Naturalidade = "São Paulo",
                Nacionalidade = "Brasileira",
                CPF = "12345678901"
            };

            PersonDtoV1 createdPerson = new PersonDtoV1
            {
                Id = 1,
                Nome = inputPerson.Nome,
                Sexo = inputPerson.Sexo,
                Email = inputPerson.Email,
                DataNascimento = inputPerson.DataNascimento,
                Naturalidade = inputPerson.Naturalidade,
                Nacionalidade = inputPerson.Nacionalidade,
                CPF = inputPerson.CPF,
                CreatedAt = DateTime.UtcNow
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<CreatePersonCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(createdPerson);

            // Act
            ActionResult<PersonDtoV1> result = await _controller.Create(inputPerson);

            // Assert
            result.Should().NotBeNull();
            CreatedAtActionResult createdResult = result.Result.Should().BeOfType<CreatedAtActionResult>().Subject;
            PersonDtoV1 actualPerson = createdResult.Value.Should().BeOfType<PersonDtoV1>().Subject;
            actualPerson.Should().BeEquivalentTo(createdPerson);
            createdResult.ActionName.Should().Be(nameof(PeopleHubController.GetById));
            createdResult.RouteValues.Should().ContainKey("id").WhoseValue.Should().Be(createdPerson.Id);

            _mediatorMock.Verify(m => m.Send(It.Is<CreatePersonCommand>(cmd => 
                cmd.Nome == inputPerson.Nome && 
                cmd.CPF == inputPerson.CPF && 
                cmd.Email == inputPerson.Email), 
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Create_WithInvalidData_ShouldReturnBadRequest()
        {
            // Arrange
            PersonDtoV1 inputPerson = new PersonDtoV1
            {
                Nome = "",
                Email = "invalid-email",
                DataNascimento = DateTime.UtcNow.AddDays(1), // Future date
                CPF = "invalid-cpf"
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<CreatePersonCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ArgumentException("Dados inválidos"));

            // Act
            ActionResult<PersonDtoV1> result = await _controller.Create(inputPerson);

            // Assert
            result.Should().NotBeNull();
            BadRequestObjectResult badRequestResult = result.Result.Should().BeOfType<BadRequestObjectResult>().Subject;
            dynamic responseValue = badRequestResult.Value!;
            string message = responseValue.GetType().GetProperty("message")?.GetValue(responseValue)?.ToString() ?? "";
            message.Should().Be("Dados inválidos");

            _mediatorMock.Verify(m => m.Send(It.IsAny<CreatePersonCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Create_WithDuplicateCpf_ShouldReturnBadRequest()
        {
            // Arrange
            PersonDtoV1 inputPerson = new PersonDtoV1
            {
                Nome = "João Silva",
                Email = "joao@email.com",
                DataNascimento = new DateTime(1990, 5, 15),
                CPF = "12345678901"
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<CreatePersonCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ArgumentException("CPF já está cadastrado"));

            // Act
            ActionResult<PersonDtoV1> result = await _controller.Create(inputPerson);

            // Assert
            result.Should().NotBeNull();
            BadRequestObjectResult badRequestResult = result.Result.Should().BeOfType<BadRequestObjectResult>().Subject;
            dynamic responseValue = badRequestResult.Value!;
            string message = responseValue.GetType().GetProperty("message")?.GetValue(responseValue)?.ToString() ?? "";
            message.Should().Be("CPF já está cadastrado");

            _mediatorMock.Verify(m => m.Send(It.IsAny<CreatePersonCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        #endregion

        #region Update Tests

        [Fact]
        public async Task Update_WithValidData_ShouldReturnOkWithUpdatedPerson()
        {
            // Arrange
            int personId = 1;
            PersonDtoV1 inputPerson = new PersonDtoV1
            {
                Nome = "João Silva Updated",
                Sexo = "M",
                Email = "joao.updated@email.com",
                DataNascimento = new DateTime(1990, 5, 15),
                Naturalidade = "São Paulo",
                Nacionalidade = "Brasileira",
                CPF = "12345678901"
            };

            PersonDtoV1 updatedPerson = new PersonDtoV1
            {
                Id = personId,
                Nome = inputPerson.Nome,
                Sexo = inputPerson.Sexo,
                Email = inputPerson.Email,
                DataNascimento = inputPerson.DataNascimento,
                Naturalidade = inputPerson.Naturalidade,
                Nacionalidade = inputPerson.Nacionalidade,
                CPF = inputPerson.CPF,
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                UpdatedAt = DateTime.UtcNow
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<UpdatePersonCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(updatedPerson);

            // Act
            ActionResult<PersonDtoV1> result = await _controller.Update(personId, inputPerson);

            // Assert
            result.Should().NotBeNull();
            OkObjectResult okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            PersonDtoV1 actualPerson = okResult.Value.Should().BeOfType<PersonDtoV1>().Subject;
            actualPerson.Should().BeEquivalentTo(updatedPerson);

            _mediatorMock.Verify(m => m.Send(It.Is<UpdatePersonCommand>(cmd => 
                cmd.Id == personId && 
                cmd.Nome == inputPerson.Nome && 
                cmd.Email == inputPerson.Email), 
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Update_WithNonExistingId_ShouldReturnBadRequest()
        {
            // Arrange
            int personId = 999;
            PersonDtoV1 inputPerson = new PersonDtoV1
            {
                Nome = "João Silva",
                Email = "joao@email.com",
                DataNascimento = new DateTime(1990, 5, 15),
                CPF = "12345678901"
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<UpdatePersonCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ArgumentException("Pessoa não encontrada"));

            // Act
            ActionResult<PersonDtoV1> result = await _controller.Update(personId, inputPerson);

            // Assert
            result.Should().NotBeNull();
            BadRequestObjectResult badRequestResult = result.Result.Should().BeOfType<BadRequestObjectResult>().Subject;
            dynamic responseValue = badRequestResult.Value!;
            string message = responseValue.GetType().GetProperty("message")?.GetValue(responseValue)?.ToString() ?? "";
            message.Should().Be("Pessoa não encontrada");

            _mediatorMock.Verify(m => m.Send(It.Is<UpdatePersonCommand>(cmd => cmd.Id == personId), It.IsAny<CancellationToken>()), Times.Once);
        }

        #endregion

        #region Delete Tests

        [Fact]
        public async Task Delete_WithExistingId_ShouldReturnNoContent()
        {
            // Arrange
            int personId = 1;

            _mediatorMock
                .Setup(m => m.Send(It.Is<DeletePersonCommand>(cmd => cmd.Id == personId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            ActionResult result = await _controller.Delete(personId);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<NoContentResult>();

            _mediatorMock.Verify(m => m.Send(It.Is<DeletePersonCommand>(cmd => cmd.Id == personId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Delete_WithNonExistingId_ShouldReturnNotFound()
        {
            // Arrange
            int personId = 999;

            _mediatorMock
                .Setup(m => m.Send(It.Is<DeletePersonCommand>(cmd => cmd.Id == personId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            // Act
            ActionResult result = await _controller.Delete(personId);

            // Assert
            result.Should().NotBeNull();
            NotFoundObjectResult notFoundResult = result.Should().BeOfType<NotFoundObjectResult>().Subject;
            dynamic responseValue = notFoundResult.Value!;
            string message = responseValue.GetType().GetProperty("message")?.GetValue(responseValue)?.ToString() ?? "";
            message.Should().Be("Pessoa não encontrada");

            _mediatorMock.Verify(m => m.Send(It.Is<DeletePersonCommand>(cmd => cmd.Id == personId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Delete_WithException_ShouldReturnInternalServerError()
        {
            // Arrange
            int personId = 1;

            _mediatorMock
                .Setup(m => m.Send(It.Is<DeletePersonCommand>(cmd => cmd.Id == personId), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Database connection error"));

            // Act
            ActionResult result = await _controller.Delete(personId);

            // Assert
            result.Should().NotBeNull();
            ObjectResult objectResult = result.Should().BeOfType<ObjectResult>().Subject;
            objectResult.StatusCode.Should().Be(500);
            dynamic responseValue = objectResult.Value!;
            string message = responseValue.GetType().GetProperty("message")?.GetValue(responseValue)?.ToString() ?? "";
            string details = responseValue.GetType().GetProperty("details")?.GetValue(responseValue)?.ToString() ?? "";
            message.Should().Be("Erro interno do servidor");
            details.Should().Be("Database connection error");

            _mediatorMock.Verify(m => m.Send(It.Is<DeletePersonCommand>(cmd => cmd.Id == personId), It.IsAny<CancellationToken>()), Times.Once);
        }

        #endregion
    }
}