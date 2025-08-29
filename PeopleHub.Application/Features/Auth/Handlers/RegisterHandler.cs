using MediatR;
using PeopleHub.Application.DTOs;
using PeopleHub.Application.Features.Auth.Commands;
using PeopleHub.Domain.Entities;
using PeopleHub.Domain.Interfaces;

namespace PeopleHub.Application.Features.Auth.Handlers
{
    public class RegisterHandler : IRequestHandler<RegisterCommand, RegisterResponseDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPersonRepository _personRepository;
        private readonly IPasswordService _passwordService;

        public RegisterHandler(IUserRepository userRepository, IPersonRepository personRepository, IPasswordService passwordService)
        {
            _userRepository = userRepository;
            _personRepository = personRepository;
            _passwordService = passwordService;
        }

        public async Task<RegisterResponseDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            // Validar se CPF j� existe nas pessoas
            Person? existingPerson = await _personRepository.GetByCPFAsync(request.CPF);
            if (existingPerson != null)
            {
                throw new ArgumentException("J� existe uma pessoa cadastrada com este CPF");
            }

            // Validar se email j� existe nos usu�rios (se preenchido)
            if (!string.IsNullOrEmpty(request.Email))
            {
                User? existingUser = await _userRepository.GetByEmailAsync(request.Email);
                if (existingUser != null)
                {
                    throw new ArgumentException("J� existe um usu�rio cadastrado com este email");
                }
            }

            // Validar data de nascimento
            if (request.DataNascimento >= DateTime.Now.Date)
            {
                throw new ArgumentException("A data de nascimento deve ser anterior � data atual");
            }

            // Validar CPF
            if (!IsValidCpf(request.CPF))
            {
                throw new ArgumentException("CPF inv�lido");
            }

            try
            {
                // Criar registro da pessoa
                Person person = new Person
                {
                    Nome = request.Nome,
                    Sexo = request.Sexo,
                    Email = request.Email,
                    DataNascimento = request.DataNascimento,
                    Naturalidade = request.Naturalidade,
                    Nacionalidade = request.Nacionalidade,
                    CPF = request.CPF
                };

                Person createdPerson = await _personRepository.AddAsync(person);

                // Criar usu�rio usando o CPF como username
                string passwordHash = _passwordService.HashPassword(request.Password);
                
                User user = new User
                {
                    Username = request.CPF, // Usando CPF como username
                    Email = request.Email ?? $"{request.CPF}@peoplehub.com", // Se n�o tiver email, gera um padr�o
                    PasswordHash = passwordHash,
                    IsActive = true
                };

                User createdUser = await _userRepository.AddAsync(user);

                return new RegisterResponseDto
                {
                    Message = "Usu�rio criado com sucesso",
                    Username = createdUser.Username,
                    Email = createdUser.Email
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao criar usu�rio: {ex.Message}");
            }
        }

        private bool IsValidCpf(string cpf)
        {
            // Remove caracteres n�o num�ricos
            cpf = cpf.Replace(".", "").Replace("-", "").Replace(" ", "");

            // Verifica se tem 11 d�gitos
            if (cpf.Length != 11)
                return false;

            // Verifica se todos os d�gitos s�o iguais
            if (cpf.All(c => c == cpf[0]))
                return false;

            // Valida��o do primeiro d�gito verificador
            int sum = 0;
            for (int i = 0; i < 9; i++)
                sum += int.Parse(cpf[i].ToString()) * (10 - i);

            int remainder = sum % 11;
            int digit1 = remainder < 2 ? 0 : 11 - remainder;

            if (int.Parse(cpf[9].ToString()) != digit1)
                return false;

            // Valida��o do segundo d�gito verificador
            sum = 0;
            for (int i = 0; i < 10; i++)
                sum += int.Parse(cpf[i].ToString()) * (11 - i);

            remainder = sum % 11;
            int digit2 = remainder < 2 ? 0 : 11 - remainder;

            return int.Parse(cpf[10].ToString()) == digit2;
        }
    }
}