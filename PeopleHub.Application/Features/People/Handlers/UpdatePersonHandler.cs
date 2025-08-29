using MediatR;
using PeopleHub.Application.DTOs;
using PeopleHub.Application.Features.People.Commands;
using PeopleHub.Domain.Entities;
using PeopleHub.Domain.Interfaces;

namespace PeopleHub.Application.Features.People.Handlers
{
    public class UpdatePersonHandler : IRequestHandler<UpdatePersonCommand, PersonDtoV1>
    {
        private readonly IPersonRepository _personRepository;

        public UpdatePersonHandler(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public async Task<PersonDtoV1> Handle(UpdatePersonCommand request, CancellationToken cancellationToken)
        {
            Person? existingPerson = await _personRepository.GetByIdAsync(request.Id);
            if (existingPerson == null)
            {
                throw new ArgumentException("Pessoa não encontrada");
            }

            // Validar se CPF já existe para outro registro
            if (await _personRepository.ExistsByCPFAsync(request.CPF, request.Id))
            {
                throw new ArgumentException("CPF já está em uso por outra pessoa");
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

            // Atualizar dados
            existingPerson.Nome = request.Nome;
            existingPerson.Sexo = request.Sexo;
            existingPerson.Email = request.Email;
            existingPerson.DataNascimento = request.DataNascimento;
            existingPerson.Naturalidade = request.Naturalidade;
            existingPerson.Nacionalidade = request.Nacionalidade;
            existingPerson.CPF = request.CPF;
            existingPerson.Endereco = request.Endereco;

            Person updatedPerson = await _personRepository.UpdateAsync(existingPerson);

            return new PersonDtoV1
            {
                Id = updatedPerson.Id,
                Nome = updatedPerson.Nome,
                Sexo = updatedPerson.Sexo,
                Email = updatedPerson.Email,
                DataNascimento = updatedPerson.DataNascimento,
                Naturalidade = updatedPerson.Naturalidade,
                Nacionalidade = updatedPerson.Nacionalidade,
                CPF = updatedPerson.CPF,
                CreatedAt = updatedPerson.CreatedAt,
                UpdatedAt = updatedPerson.UpdatedAt
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