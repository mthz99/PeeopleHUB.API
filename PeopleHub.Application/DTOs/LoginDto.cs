using System.ComponentModel.DataAnnotations;

namespace PeopleHub.Application.DTOs
{
    public class LoginDto
    {
        [Required(ErrorMessage = "O nome de usu�rio (CPF) � obrigat�rio")]
        [Display(Name = "CPF", Description = "Informe o CPF sem pontos ou tra�os")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "A senha � obrigat�ria")]
        [Display(Name = "Senha")]
        public string Password { get; set; } = string.Empty;
    }

    public class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
    }

    public class RegisterDto
    {
        [Required(ErrorMessage = "O nome � obrigat�rio")]
        [StringLength(200, ErrorMessage = "O nome deve ter no m�ximo 200 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [StringLength(1, ErrorMessage = "O sexo deve ter apenas 1 caractere")]
        [RegularExpression("^[MF]$", ErrorMessage = "O sexo deve ser M ou F")]
        public string? Sexo { get; set; }

        [EmailAddress(ErrorMessage = "Email inv�lido")]
        [StringLength(200, ErrorMessage = "O email deve ter no m�ximo 200 caracteres")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "A data de nascimento � obrigat�ria")]
        public DateTime DataNascimento { get; set; }

        [StringLength(100, ErrorMessage = "A naturalidade deve ter no m�ximo 100 caracteres")]
        public string? Naturalidade { get; set; }

        [StringLength(100, ErrorMessage = "A nacionalidade deve ter no m�ximo 100 caracteres")]
        public string? Nacionalidade { get; set; }

        [Required(ErrorMessage = "O CPF � obrigat�rio")]
        [StringLength(14, ErrorMessage = "O CPF deve ter no m�ximo 14 caracteres")]
        public string CPF { get; set; } = string.Empty;

        [Required(ErrorMessage = "A senha � obrigat�ria")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "A senha deve ter entre 6 e 100 caracteres")]
        public string Password { get; set; } = string.Empty;
    }

    public class RegisterResponseDto
    {
        public string Message { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}