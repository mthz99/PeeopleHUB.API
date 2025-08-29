using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PeopleHub.API.Controllers.version2;
using PeopleHub.Application.DTOs;
using PeopleHub.Application.Features.People.Commands;
using PeopleHub.Application.Features.People.Queries;

namespace PeopleHub.Test.Controllers.Version2
{
    public class PeopleHubControllerV2Tests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly PeopleHubController _controller;

        public PeopleHubControllerV2Tests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new PeopleHubController(_mediatorMock.Object);
        }

        #region GetAll Tests

        [Fact]
        public async Task GetAll_WithExistingPeople_ShouldReturnOkWithPeopleListIncludingAddress()
        {
            // Arrange
            IEnumerable<PersonDtoV1> basePeople = new List<PersonDtoV1>
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
                .ReturnsAsync(basePeople);

            // Act
            ActionResult<IEnumerable<PersonDtoV2>> result = await _controller.GetAll();

            // Assert
            result.Should().NotBeNull();
            OkObjectResult okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            IEnumerable<PersonDtoV2> actualPeople = okResult.Value.Should().BeAssignableTo<IEnumerable<PersonDtoV2>>().Subject;
            actualPeople.Should().HaveCount(2);
            
            PersonDtoV2[] peopleArray = actualPeople.ToArray();
            peopleArray[0].Id.Should().Be(1);
            peopleArray[0].Nome.Should().Be("João Silva");
            peopleArray[0].Endereco.Should().Be(""); // Default empty address
            peopleArray[1].Id.Should().Be(2);
            peopleArray[1].Nome.Should().Be("Maria Santos");
            peopleArray[1].Endereco.Should().Be(""); // Default empty address

            _mediatorMock.Verify(m => m.Send(It.IsAny<GetAllPeopleQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetAll_WithNoPeople_ShouldReturnOkWithEmptyList()
        {
            // Arrange
            IEnumerable<PersonDtoV1> basePeople = new List<PersonDtoV1>();

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetAllPeopleQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(basePeople);

            // Act
            ActionResult<IEnumerable<PersonDtoV2>> result = await _controller.GetAll();

            // Assert
            result.Should().NotBeNull();
            OkObjectResult okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            IEnumerable<PersonDtoV2> actualPeople = okResult.Value.Should().BeAssignableTo<IEnumerable<PersonDtoV2>>().Subject;
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
            ActionResult<IEnumerable<PersonDtoV2>> result = await _controller.GetAll();

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
        public async Task GetById_WithExistingId_ShouldReturnOkWithPersonIncludingAddress()
        {
            // Arrange
            int personId = 1;
            PersonDtoV1 basePerson = new PersonDtoV1
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
                .ReturnsAsync(basePerson);

            // Act
            ActionResult<PersonDtoV2> result = await _controller.GetById(personId);

            // Assert
            result.Should().NotBeNull();
            OkObjectResult okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            PersonDtoV2 actualPerson = okResult.Value.Should().BeOfType<PersonDtoV2>().Subject;
            actualPerson.Id.Should().Be(basePerson.Id);
            actualPerson.Nome.Should().Be(basePerson.Nome);
            actualPerson.Endereco.Should().Be(""); // Default empty address

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
            ActionResult<PersonDtoV2> result = await _controller.GetById(personId);

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
            ActionResult<PersonDtoV2> result = await _controller.GetById(personId);

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
        public async Task Create_WithValidDataIncludingAddress_ShouldReturnCreatedAtAction()
        {
            // Arrange
            PersonDtoV2 inputPerson = new PersonDtoV2
            {
                Nome = "João Silva",
                Sexo = "M",
                Email = "joao@email.com",
                DataNascimento = new DateTime(1990, 5, 15),
                Naturalidade = "São Paulo",
                Nacionalidade = "Brasileira",
                CPF = "12345678901",
                Endereco = "Rua das Flores, 123, São Paulo, SP"
            };

            PersonDtoV1 createdBasePerson = new PersonDtoV1
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
                .ReturnsAsync(createdBasePerson);

            // Act
            ActionResult<PersonDtoV2> result = await _controller.Create(inputPerson);

            // Assert
            result.Should().NotBeNull();
            CreatedAtActionResult createdResult = result.Result.Should().BeOfType<CreatedAtActionResult>().Subject;
            PersonDtoV2 actualPerson = createdResult.Value.Should().BeOfType<PersonDtoV2>().Subject;
            actualPerson.Id.Should().Be(createdBasePerson.Id);
            actualPerson.Nome.Should().Be(inputPerson.Nome);
            actualPerson.Endereco.Should().Be(inputPerson.Endereco);
            createdResult.ActionName.Should().Be(nameof(PeopleHubController.GetById));
            createdResult.RouteValues.Should().ContainKey("id").WhoseValue.Should().Be(createdBasePerson.Id);

            _mediatorMock.Verify(m => m.Send(It.Is<CreatePersonCommand>(cmd => 
                cmd.Nome == inputPerson.Nome && 
                cmd.CPF == inputPerson.CPF && 
                cmd.Email == inputPerson.Email &&
                cmd.Endereco == inputPerson.Endereco), 
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Create_WithMissingAddress_ShouldReturnBadRequest()
        {
            // Arrange
            PersonDtoV2 inputPerson = new PersonDtoV2
            {
                Nome = "João Silva",
                Sexo = "M",
                Email = "joao@email.com",
                DataNascimento = new DateTime(1990, 5, 15),
                CPF = "12345678901",
                Endereco = "" // Empty address - required in V2
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<CreatePersonCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ArgumentException("O endereço é obrigatório na versão 2"));

            // Act
            ActionResult<PersonDtoV2> result = await _controller.Create(inputPerson);

            // Assert
            result.Should().NotBeNull();
            BadRequestObjectResult badRequestResult = result.Result.Should().BeOfType<BadRequestObjectResult>().Subject;
            dynamic responseValue = badRequestResult.Value!;
            string message = responseValue.GetType().GetProperty("message")?.GetValue(responseValue)?.ToString() ?? "";
            message.Should().Be("O endereço é obrigatório na versão 2");

            _mediatorMock.Verify(m => m.Send(It.IsAny<CreatePersonCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Create_WithInvalidData_ShouldReturnBadRequest()
        {
            // Arrange
            PersonDtoV2 inputPerson = new PersonDtoV2
            {
                Nome = "",
                Email = "invalid-email",
                DataNascimento = DateTime.UtcNow.AddDays(1), // Future date
                CPF = "invalid-cpf",
                Endereco = "Valid Address"
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<CreatePersonCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ArgumentException("Dados inválidos"));

            // Act
            ActionResult<PersonDtoV2> result = await _controller.Create(inputPerson);

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
            PersonDtoV2 inputPerson = new PersonDtoV2
            {
                Nome = "João Silva",
                Email = "joao@email.com",
                DataNascimento = new DateTime(1990, 5, 15),
                CPF = "12345678901",
                Endereco = "Rua das Flores, 123, São Paulo, SP"
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<CreatePersonCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ArgumentException("CPF já está cadastrado"));

            // Act
            ActionResult<PersonDtoV2> result = await _controller.Create(inputPerson);

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
        public async Task Update_WithValidDataIncludingAddress_ShouldReturnOkWithUpdatedPerson()
        {
            // Arrange
            int personId = 1;
            PersonDtoV2 inputPerson = new PersonDtoV2
            {
                Nome = "João Silva Updated",
                Sexo = "M",
                Email = "joao.updated@email.com",
                DataNascimento = new DateTime(1990, 5, 15),
                Naturalidade = "São Paulo",
                Nacionalidade = "Brasileira",
                CPF = "12345678901",
                Endereco = "Rua das Flores, 456, São Paulo, SP"
            };

            PersonDtoV1 updatedBasePerson = new PersonDtoV1
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
                .ReturnsAsync(updatedBasePerson);

            // Act
            ActionResult<PersonDtoV2> result = await _controller.Update(personId, inputPerson);

            // Assert
            result.Should().NotBeNull();
            OkObjectResult okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            PersonDtoV2 actualPerson = okResult.Value.Should().BeOfType<PersonDtoV2>().Subject;
            actualPerson.Id.Should().Be(personId);
            actualPerson.Nome.Should().Be(inputPerson.Nome);
            actualPerson.Email.Should().Be(inputPerson.Email);
            actualPerson.Endereco.Should().Be(inputPerson.Endereco);

            _mediatorMock.Verify(m => m.Send(It.Is<UpdatePersonCommand>(cmd => 
                cmd.Id == personId && 
                cmd.Nome == inputPerson.Nome && 
                cmd.Email == inputPerson.Email &&
                cmd.Endereco == inputPerson.Endereco), 
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Update_WithMissingAddress_ShouldReturnBadRequest()
        {
            // Arrange
            int personId = 1;
            PersonDtoV2 inputPerson = new PersonDtoV2
            {
                Nome = "João Silva Updated",
                Email = "joao.updated@email.com",
                DataNascimento = new DateTime(1990, 5, 15),
                CPF = "12345678901",
                Endereco = "" // Empty address - required in V2
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<UpdatePersonCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ArgumentException("O endereço é obrigatório na versão 2"));

            // Act
            ActionResult<PersonDtoV2> result = await _controller.Update(personId, inputPerson);

            // Assert
            result.Should().NotBeNull();
            BadRequestObjectResult badRequestResult = result.Result.Should().BeOfType<BadRequestObjectResult>().Subject;
            dynamic responseValue = badRequestResult.Value!;
            string message = responseValue.GetType().GetProperty("message")?.GetValue(responseValue)?.ToString() ?? "";
            message.Should().Be("O endereço é obrigatório na versão 2");

            _mediatorMock.Verify(m => m.Send(It.IsAny<UpdatePersonCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Update_WithNonExistingId_ShouldReturnBadRequest()
        {
            // Arrange
            int personId = 999;
            PersonDtoV2 inputPerson = new PersonDtoV2
            {
                Nome = "João Silva",
                Email = "joao@email.com",
                DataNascimento = new DateTime(1990, 5, 15),
                CPF = "12345678901",
                Endereco = "Rua das Flores, 123, São Paulo, SP"
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<UpdatePersonCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ArgumentException("Pessoa não encontrada"));

            // Act
            ActionResult<PersonDtoV2> result = await _controller.Update(personId, inputPerson);

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