using PeopleHub.Application.DTOs;

namespace PeopleHub.Test.Helpers
{
    public static class TestDataFactory
    {
        #region PersonDtoV1 Factory Methods

        public static PersonDtoV1 CreateValidPersonDtoV1(int id = 1)
        {
            return new PersonDtoV1
            {
                Id = id,
                Nome = "João Silva",
                Sexo = "M",
                Email = "joao@email.com",
                DataNascimento = new DateTime(1990, 5, 15),
                Naturalidade = "São Paulo",
                Nacionalidade = "Brasileira",
                CPF = "12345678901",
                CreatedAt = DateTime.UtcNow
            };
        }

        public static PersonDtoV1 CreatePersonDtoV1WithCustomData(
            int id = 1,
            string nome = "João Silva",
            string? sexo = "M",
            string? email = "joao@email.com",
            DateTime? dataNascimento = null,
            string? naturalidade = "São Paulo",
            string? nacionalidade = "Brasileira",
            string cpf = "12345678901")
        {
            return new PersonDtoV1
            {
                Id = id,
                Nome = nome,
                Sexo = sexo,
                Email = email,
                DataNascimento = dataNascimento ?? new DateTime(1990, 5, 15),
                Naturalidade = naturalidade,
                Nacionalidade = nacionalidade,
                CPF = cpf,
                CreatedAt = DateTime.UtcNow
            };
        }

        public static PersonDtoV1 CreateInvalidPersonDtoV1()
        {
            return new PersonDtoV1
            {
                Nome = "", // Invalid: empty name
                Email = "invalid-email", // Invalid: wrong email format
                DataNascimento = DateTime.UtcNow.AddDays(1), // Invalid: future date
                CPF = "invalid-cpf" // Invalid: wrong CPF format
            };
        }

        public static IEnumerable<PersonDtoV1> CreatePersonDtoV1List(int count = 2)
        {
            List<PersonDtoV1> people = new List<PersonDtoV1>();
            
            for (int i = 1; i <= count; i++)
            {
                people.Add(new PersonDtoV1
                {
                    Id = i,
                    Nome = $"Pessoa {i}",
                    Sexo = i % 2 == 0 ? "F" : "M",
                    Email = $"pessoa{i}@email.com",
                    DataNascimento = new DateTime(1980 + i, 1, 1),
                    CPF = $"1234567890{i}",
                    CreatedAt = DateTime.UtcNow
                });
            }
            
            return people;
        }

        #endregion

        #region PersonDtoV2 Factory Methods

        public static PersonDtoV2 CreateValidPersonDtoV2(int id = 1)
        {
            return new PersonDtoV2
            {
                Id = id,
                Nome = "João Silva",
                Sexo = "M",
                Email = "joao@email.com",
                DataNascimento = new DateTime(1990, 5, 15),
                Naturalidade = "São Paulo",
                Nacionalidade = "Brasileira",
                CPF = "12345678901",
                Endereco = "Rua das Flores, 123, São Paulo, SP",
                CreatedAt = DateTime.UtcNow
            };
        }

        public static PersonDtoV2 CreatePersonDtoV2WithCustomData(
            int id = 1,
            string nome = "João Silva",
            string? sexo = "M",
            string? email = "joao@email.com",
            DateTime? dataNascimento = null,
            string? naturalidade = "São Paulo",
            string? nacionalidade = "Brasileira",
            string cpf = "12345678901",
            string endereco = "Rua das Flores, 123, São Paulo, SP")
        {
            return new PersonDtoV2
            {
                Id = id,
                Nome = nome,
                Sexo = sexo,
                Email = email,
                DataNascimento = dataNascimento ?? new DateTime(1990, 5, 15),
                Naturalidade = naturalidade,
                Nacionalidade = nacionalidade,
                CPF = cpf,
                Endereco = endereco,
                CreatedAt = DateTime.UtcNow
            };
        }

        public static PersonDtoV2 CreatePersonDtoV2WithoutAddress()
        {
            return new PersonDtoV2
            {
                Nome = "João Silva",
                Sexo = "M",
                Email = "joao@email.com",
                DataNascimento = new DateTime(1990, 5, 15),
                CPF = "12345678901",
                Endereco = "" // Empty address - invalid for V2
            };
        }

        public static PersonDtoV2 CreateInvalidPersonDtoV2()
        {
            return new PersonDtoV2
            {
                Nome = "", // Invalid: empty name
                Email = "invalid-email", // Invalid: wrong email format
                DataNascimento = DateTime.UtcNow.AddDays(1), // Invalid: future date
                CPF = "invalid-cpf", // Invalid: wrong CPF format
                Endereco = "Valid Address"
            };
        }

        #endregion

        #region LoginDto Factory Methods

        public static LoginDto CreateValidLoginDto()
        {
            return new LoginDto
            {
                Username = "12345678901",
                Password = "password123"
            };
        }

        public static LoginDto CreateLoginDtoWithCustomData(string username, string password)
        {
            return new LoginDto
            {
                Username = username,
                Password = password
            };
        }

        public static LoginDto CreateLoginDtoWithEmptyUsername()
        {
            return new LoginDto
            {
                Username = "",
                Password = "password123"
            };
        }

        public static LoginDto CreateLoginDtoWithEmptyPassword()
        {
            return new LoginDto
            {
                Username = "12345678901",
                Password = ""
            };
        }

        #endregion

        #region RegisterDto Factory Methods

        public static RegisterDto CreateValidRegisterDto()
        {
            return new RegisterDto
            {
                Nome = "João Silva",
                Sexo = "M",
                Email = "joao@email.com",
                DataNascimento = new DateTime(1990, 5, 15),
                Naturalidade = "São Paulo",
                Nacionalidade = "Brasileira",
                CPF = "12345678901",
                Password = "password123"
            };
        }

        public static RegisterDto CreateRegisterDtoWithCustomData(
            string nome = "João Silva",
            string? sexo = "M",
            string? email = "joao@email.com",
            DateTime? dataNascimento = null,
            string? naturalidade = "São Paulo",
            string? nacionalidade = "Brasileira",
            string cpf = "12345678901",
            string password = "password123")
        {
            return new RegisterDto
            {
                Nome = nome,
                Sexo = sexo,
                Email = email,
                DataNascimento = dataNascimento ?? new DateTime(1990, 5, 15),
                Naturalidade = naturalidade,
                Nacionalidade = nacionalidade,
                CPF = cpf,
                Password = password
            };
        }

        public static RegisterDto CreateInvalidRegisterDto()
        {
            return new RegisterDto
            {
                Nome = "", // Invalid: empty name
                Sexo = "M",
                Email = "invalid-email", // Invalid: wrong email format
                DataNascimento = DateTime.UtcNow.AddDays(1), // Invalid: future date
                Naturalidade = "São Paulo",
                Nacionalidade = "Brasileira",
                CPF = "invalid-cpf", // Invalid: wrong CPF format
                Password = "123" // Invalid: too short
            };
        }

        #endregion

        #region Response DTOs Factory Methods

        public static LoginResponseDto CreateValidLoginResponseDto()
        {
            return new LoginResponseDto
            {
                Token = "jwt-token-example",
                Username = "12345678901",
                ExpiresAt = DateTime.UtcNow.AddHours(1)
            };
        }

        public static RegisterResponseDto CreateValidRegisterResponseDto()
        {
            return new RegisterResponseDto
            {
                Message = "Usuário registrado com sucesso",
                Username = "12345678901",
                Email = "joao@email.com"
            };
        }

        #endregion

        #region Common Test Constants

        public static class TestConstants
        {
            public const string VALID_CPF = "12345678901";
            public const string INVALID_CPF = "invalid-cpf";
            public const string VALID_EMAIL = "test@email.com";
            public const string INVALID_EMAIL = "invalid-email";
            public const string VALID_PASSWORD = "password123";
            public const string SHORT_PASSWORD = "123";
            public const string VALID_ADDRESS = "Rua das Flores, 123, São Paulo, SP";
            public const string EMPTY_STRING = "";
            
            public static readonly DateTime VALID_BIRTH_DATE = new DateTime(1990, 5, 15);
            public static readonly DateTime FUTURE_DATE = DateTime.UtcNow.AddDays(1);
            
            public const string ERROR_MESSAGE_INVALID_CREDENTIALS = "Credenciais inválidas";
            public const string ERROR_MESSAGE_INVALID_DATA = "Dados inválidos";
            public const string ERROR_MESSAGE_PERSON_NOT_FOUND = "Pessoa não encontrada";
            public const string ERROR_MESSAGE_CPF_ALREADY_EXISTS = "CPF já está cadastrado";
            public const string ERROR_MESSAGE_REQUIRED_FIELDS = "Username (CPF) e senha são obrigatórios";
            public const string ERROR_MESSAGE_ADDRESS_REQUIRED_V2 = "O endereço é obrigatório na versão 2";
            public const string ERROR_MESSAGE_INTERNAL_SERVER = "Erro interno do servidor";
            public const string ERROR_MESSAGE_DATABASE_CONNECTION = "Database connection error";
        }

        #endregion
    }
}