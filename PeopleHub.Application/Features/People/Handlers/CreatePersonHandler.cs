using MediatR;
using PeopleHub.Application.DTOs;
using PeopleHub.Application.Features.People.Commands;
using PeopleHub.Domain.Entities;
using PeopleHub.Domain.Interfaces;

namespace PeopleHub.Application.Features.People.Handlers
{
    public class CreatePersonHandler : IRequestHandler<CreatePersonCommand, PersonDtoV1>
    {
        private readonly IPersonRepository _personRepository;

        public CreatePersonHandler(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public async Task<PersonDtoV1> Handle(CreatePersonCommand request, CancellationToken cancellationToken)
        {
            // Validar se CPF já existe
            if (await _personRepository.ExistsByCPFAsync(request.CPF))
            {
                throw new ArgumentException("CPF já está em uso");
            }

            // Validar CPF
            if (!IsValidCPF(request.CPF))
            {
                throw new ArgumentException("CPF inválido");
            }

            // Validar data de nascimento
            if (request.DataNascimento >= DateTime.Now)
            {
                throw new ArgumentException("Data de nascimento deve ser anterior à data atual");
            }

            Person person = new Person
            {
                Nome = request.Nome,
                Sexo = request.Sexo,
                Email = request.Email,
                DataNascimento = request.DataNascimento,
                Naturalidade = request.Naturalidade,
                Nacionalidade = request.Nacionalidade,
                CPF = request.CPF,
                Endereco = request.Endereco
            };

            Person createdPerson = await _personRepository.AddAsync(person);

            return new PersonDtoV1
            {
                Id = createdPerson.Id,
                Nome = createdPerson.Nome,
                Sexo = createdPerson.Sexo,
                Email = createdPerson.Email,
                DataNascimento = createdPerson.DataNascimento,
                Naturalidade = createdPerson.Naturalidade,
                Nacionalidade = createdPerson.Nacionalidade,
                CPF = createdPerson.CPF,
                CreatedAt = createdPerson.CreatedAt,
                UpdatedAt = createdPerson.UpdatedAt
            };
        }

        private static bool IsValidCPF(string cpf)
        {
            cpf = cpf.Replace(".", "").Replace("-", "");
            
            if (cpf.Length != 11)
                return false;

            if (cpf.All(c => c == cpf[0]))
                return false;

            int[] sequence = cpf.Select(c => int.Parse(c.ToString())).ToArray();

            // Primeiro dígito verificador
            int sum = 0;
            for (int i = 0; i < 9; i++)
                sum += sequence[i] * (10 - i);

            int remainder = sum % 11;
            int digit1 = remainder < 2 ? 0 : 11 - remainder;

            if (sequence[9] != digit1)
                return false;

            // Segundo dígito verificador
            sum = 0;
            for (int i = 0; i < 10; i++)
                sum += sequence[i] * (11 - i);

            remainder = sum % 11;
            int digit2 = remainder < 2 ? 0 : 11 - remainder;

            return sequence[10] == digit2;
        }
    }
}