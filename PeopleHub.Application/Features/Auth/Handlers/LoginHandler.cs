using MediatR;
using PeopleHub.Application.DTOs;
using PeopleHub.Application.Features.Auth.Commands;
using PeopleHub.Domain.Entities;
using PeopleHub.Domain.Interfaces;

namespace PeopleHub.Application.Features.Auth.Handlers
{
    public class LoginHandler : IRequestHandler<LoginCommand, LoginResponseDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;

        public LoginHandler(IUserRepository userRepository, IJwtService jwtService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
        }

        public async Task<LoginResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            // Validação básica de formato
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            {
                throw new UnauthorizedAccessException("Usuário ou senha inválidos");
            }

            // Limpa o CPF/Username removendo caracteres especiais
            string cleanUsername = request.Username.Replace(".", "").Replace("-", "").Replace(" ", "");
			
			// Caso o CPF tenha pontuação, atualiza o username na request
			if (cleanUsername.Length == 11 && cleanUsername.All(char.IsDigit))
			{
				request.Username = cleanUsername;
			}

            User? user = await _userRepository.GetByUsernameAsync(request.Username);
            if (user == null)
            {
                throw new UnauthorizedAccessException("Usuário ou senha inválidos");
            }

            if (!await _userRepository.ValidateUserAsync(request.Username, request.Password))
            {
                throw new UnauthorizedAccessException("Usuário ou senha inválidos");
            }

            string token = _jwtService.GenerateToken(user);
            DateTime expiresAt = DateTime.UtcNow.AddHours(1);

            return new LoginResponseDto
            {
                Token = token,
                Username = user.Username,
                ExpiresAt = expiresAt
            };
        }
    }
}