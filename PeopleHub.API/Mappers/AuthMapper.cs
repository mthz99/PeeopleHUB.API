using PeopleHub.Application.DTOs;
using PeopleHub.Application.Features.Auth.Commands;

namespace PeopleHub.API.Mappers
{
    public static class AuthMapper
    {
        public static LoginCommand ToLoginCommand(this LoginDto dto) => new()
        {
            Username = dto.Username,
            Password = dto.Password
        };

        public static RegisterCommand ToRegisterCommand(this RegisterDto dto) => new()
        {
            Nome = dto.Nome,
            Sexo = dto.Sexo,
            Email = dto.Email,
            DataNascimento = dto.DataNascimento,
            Naturalidade = dto.Naturalidade,
            Nacionalidade = dto.Nacionalidade,
            CPF = dto.CPF,
            Password = dto.Password
        };
    }
}