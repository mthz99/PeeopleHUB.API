using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PeopleHub.API.Controllers;
using PeopleHub.Application.DTOs;
using PeopleHub.Application.Features.Auth.Commands;

namespace PeopleHub.Test.Controllers
{
    public class AuthControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new AuthController(_mediatorMock.Object);
        }

        #region Login Tests

        [Fact]
        public async Task Login_WithValidCredentials_ShouldReturnOkWithToken()
        {
            // Arrange
            LoginDto loginDto = new LoginDto
            {
                Username = "12345678901",
                Password = "password123"
            };

            LoginResponseDto expectedResponse = new LoginResponseDto
            {
                Token = "jwt-token-example",
                Username = "12345678901",
                ExpiresAt = DateTime.UtcNow.AddHours(1)
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<LoginCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResponse);

            // Act
            ActionResult<LoginResponseDto> result = await _controller.Login(loginDto);

            // Assert
            result.Should().NotBeNull();
            OkObjectResult okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            LoginResponseDto actualResponse = okResult.Value.Should().BeOfType<LoginResponseDto>().Subject;
            actualResponse.Token.Should().Be(expectedResponse.Token);
            actualResponse.Username.Should().Be(expectedResponse.Username);
            actualResponse.ExpiresAt.Should().Be(expectedResponse.ExpiresAt);

            _mediatorMock.Verify(m => m.Send(It.Is<LoginCommand>(cmd => 
                cmd.Username == loginDto.Username && 
                cmd.Password == loginDto.Password), 
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Login_WithEmptyUsername_ShouldReturnBadRequest()
        {
            // Arrange
            LoginDto loginDto = new LoginDto
            {
                Username = "",
                Password = "password123"
            };

            // Act
            ActionResult<LoginResponseDto> result = await _controller.Login(loginDto);

            // Assert
            result.Should().NotBeNull();
            BadRequestObjectResult badRequestResult = result.Result.Should().BeOfType<BadRequestObjectResult>().Subject;
            dynamic responseValue = badRequestResult.Value!;
            string message = responseValue.GetType().GetProperty("message")?.GetValue(responseValue)?.ToString() ?? "";
            message.Should().Be("Username (CPF) e senha s�o obrigat�rios");

            _mediatorMock.Verify(m => m.Send(It.IsAny<LoginCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Login_WithEmptyPassword_ShouldReturnBadRequest()
        {
            // Arrange
            LoginDto loginDto = new LoginDto
            {
                Username = "12345678901",
                Password = ""
            };

            // Act
            ActionResult<LoginResponseDto> result = await _controller.Login(loginDto);

            // Assert
            result.Should().NotBeNull();
            BadRequestObjectResult badRequestResult = result.Result.Should().BeOfType<BadRequestObjectResult>().Subject;
            dynamic responseValue = badRequestResult.Value!;
            string message = responseValue.GetType().GetProperty("message")?.GetValue(responseValue)?.ToString() ?? "";
            message.Should().Be("Username (CPF) e senha s�o obrigat�rios");

            _mediatorMock.Verify(m => m.Send(It.IsAny<LoginCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Login_WithInvalidCredentials_ShouldReturnUnauthorized()
        {
            // Arrange
            LoginDto loginDto = new LoginDto
            {
                Username = "12345678901",
                Password = "wrongpassword"
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<LoginCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new UnauthorizedAccessException("Credenciais inv�lidas"));

            // Act
            ActionResult<LoginResponseDto> result = await _controller.Login(loginDto);

            // Assert
            result.Should().NotBeNull();
            UnauthorizedObjectResult unauthorizedResult = result.Result.Should().BeOfType<UnauthorizedObjectResult>().Subject;
            dynamic responseValue = unauthorizedResult.Value!;
            string message = responseValue.GetType().GetProperty("message")?.GetValue(responseValue)?.ToString() ?? "";
            message.Should().Be("Credenciais inv�lidas");

            _mediatorMock.Verify(m => m.Send(It.IsAny<LoginCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Login_WithArgumentException_ShouldReturnBadRequest()
        {
            // Arrange
            LoginDto loginDto = new LoginDto
            {
                Username = "invalid-cpf",
                Password = "password123"
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<LoginCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ArgumentException("CPF inv�lido"));

            // Act
            ActionResult<LoginResponseDto> result = await _controller.Login(loginDto);

            // Assert
            result.Should().NotBeNull();
            BadRequestObjectResult badRequestResult = result.Result.Should().BeOfType<BadRequestObjectResult>().Subject;
            dynamic responseValue = badRequestResult.Value!;
            string message = responseValue.GetType().GetProperty("message")?.GetValue(responseValue)?.ToString() ?? "";
            message.Should().Be("CPF inv�lido");

            _mediatorMock.Verify(m => m.Send(It.IsAny<LoginCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Login_WithGenericException_ShouldReturnInternalServerError()
        {
            // Arrange
            LoginDto loginDto = new LoginDto
            {
                Username = "12345678901",
                Password = "password123"
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<LoginCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Database connection error"));

            // Act
            ActionResult<LoginResponseDto> result = await _controller.Login(loginDto);

            // Assert
            result.Should().NotBeNull();
            ObjectResult objectResult = result.Result.Should().BeOfType<ObjectResult>().Subject;
            objectResult.StatusCode.Should().Be(500);
            dynamic responseValue = objectResult.Value!;
            string message = responseValue.GetType().GetProperty("message")?.GetValue(responseValue)?.ToString() ?? "";
            string details = responseValue.GetType().GetProperty("details")?.GetValue(responseValue)?.ToString() ?? "";
            message.Should().Be("Erro interno do servidor");
            details.Should().Be("Database connection error");

            _mediatorMock.Verify(m => m.Send(It.IsAny<LoginCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        #endregion

        #region Register Tests

        [Fact]
        public async Task Register_WithValidData_ShouldReturnCreatedAtAction()
        {
            // Arrange
            RegisterDto registerDto = new RegisterDto
            {
                Nome = "Jo�o Silva",
                Sexo = "M",
                Email = "joao@email.com",
                DataNascimento = new DateTime(1990, 5, 15),
                Naturalidade = "S�o Paulo",
                Nacionalidade = "Brasileira",
                CPF = "12345678901",
                Password = "password123"
            };

            RegisterResponseDto expectedResponse = new RegisterResponseDto
            {
                Message = "Usu�rio registrado com sucesso",
                Username = "12345678901",
                Email = "joao@email.com"
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<RegisterCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResponse);

            // Act
            ActionResult<RegisterResponseDto> result = await _controller.Register(registerDto);

            // Assert
            result.Should().NotBeNull();
            CreatedAtActionResult createdResult = result.Result.Should().BeOfType<CreatedAtActionResult>().Subject;
            RegisterResponseDto actualResponse = createdResult.Value.Should().BeOfType<RegisterResponseDto>().Subject;
            actualResponse.Message.Should().Be(expectedResponse.Message);
            actualResponse.Username.Should().Be(expectedResponse.Username);
            actualResponse.Email.Should().Be(expectedResponse.Email);
            createdResult.ActionName.Should().Be(nameof(AuthController.Login));

            _mediatorMock.Verify(m => m.Send(It.Is<RegisterCommand>(cmd => 
                cmd.Nome == registerDto.Nome && 
                cmd.CPF == registerDto.CPF && 
                cmd.Email == registerDto.Email &&
                cmd.Password == registerDto.Password), 
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Register_WithDuplicateCpf_ShouldReturnBadRequest()
        {
            // Arrange
            RegisterDto registerDto = new RegisterDto
            {
                Nome = "Jo�o Silva",
                Sexo = "M",
                Email = "joao@email.com",
                DataNascimento = new DateTime(1990, 5, 15),
                Naturalidade = "S�o Paulo",
                Nacionalidade = "Brasileira",
                CPF = "12345678901",
                Password = "password123"
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<RegisterCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ArgumentException("CPF j� est� cadastrado"));

            // Act
            ActionResult<RegisterResponseDto> result = await _controller.Register(registerDto);

            // Assert
            result.Should().NotBeNull();
            BadRequestObjectResult badRequestResult = result.Result.Should().BeOfType<BadRequestObjectResult>().Subject;
            dynamic responseValue = badRequestResult.Value!;
            string message = responseValue.GetType().GetProperty("message")?.GetValue(responseValue)?.ToString() ?? "";
            message.Should().Be("CPF j� est� cadastrado");

            _mediatorMock.Verify(m => m.Send(It.IsAny<RegisterCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Register_WithInvalidData_ShouldReturnBadRequest()
        {
            // Arrange
            RegisterDto registerDto = new RegisterDto
            {
                Nome = "",
                Sexo = "M",
                Email = "invalid-email",
                DataNascimento = DateTime.UtcNow.AddDays(1), // Future date
                Naturalidade = "S�o Paulo",
                Nacionalidade = "Brasileira",
                CPF = "invalid-cpf",
                Password = "123" // Too short
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<RegisterCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ArgumentException("Dados inv�lidos"));

            // Act
            ActionResult<RegisterResponseDto> result = await _controller.Register(registerDto);

            // Assert
            result.Should().NotBeNull();
            BadRequestObjectResult badRequestResult = result.Result.Should().BeOfType<BadRequestObjectResult>().Subject;
            dynamic responseValue = badRequestResult.Value!;
            string message = responseValue.GetType().GetProperty("message")?.GetValue(responseValue)?.ToString() ?? "";
            message.Should().Be("Dados inv�lidos");

            _mediatorMock.Verify(m => m.Send(It.IsAny<RegisterCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Register_WithGenericException_ShouldReturnInternalServerError()
        {
            // Arrange
            RegisterDto registerDto = new RegisterDto
            {
                Nome = "Jo�o Silva",
                Sexo = "M",
                Email = "joao@email.com",
                DataNascimento = new DateTime(1990, 5, 15),
                Naturalidade = "S�o Paulo",
                Nacionalidade = "Brasileira",
                CPF = "12345678901",
                Password = "password123"
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<RegisterCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Database connection error"));

            // Act
            ActionResult<RegisterResponseDto> result = await _controller.Register(registerDto);

            // Assert
            result.Should().NotBeNull();
            ObjectResult objectResult = result.Result.Should().BeOfType<ObjectResult>().Subject;
            objectResult.StatusCode.Should().Be(500);
            dynamic responseValue = objectResult.Value!;
            string message = responseValue.GetType().GetProperty("message")?.GetValue(responseValue)?.ToString() ?? "";
            string details = responseValue.GetType().GetProperty("details")?.GetValue(responseValue)?.ToString() ?? "";
            message.Should().Be("Erro interno do servidor");
            details.Should().Be("Database connection error");

            _mediatorMock.Verify(m => m.Send(It.IsAny<RegisterCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        #endregion
    }
}