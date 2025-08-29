using System.ComponentModel.DataAnnotations;

namespace PeopleHub.Application.DTOs
{
    public class PersonDtoV1
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome � obrigat�rio")]
        [MaxLength(200, ErrorMessage = "O nome deve ter no m�ximo 200 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [MaxLength(1, ErrorMessage = "O sexo deve ter apenas 1 caractere")]
        public string? Sexo { get; set; }

        [EmailAddress(ErrorMessage = "E-mail inv�lido")]
        [MaxLength(200, ErrorMessage = "O e-mail deve ter no m�ximo 200 caracteres")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "A data de nascimento � obrigat�ria")]
        public DateTime DataNascimento { get; set; }

        [MaxLength(100, ErrorMessage = "A naturalidade deve ter no m�ximo 100 caracteres")]
        public string? Naturalidade { get; set; }

        [MaxLength(100, ErrorMessage = "A nacionalidade deve ter no m�ximo 100 caracteres")]
        public string? Nacionalidade { get; set; }

        [Required(ErrorMessage = "O CPF � obrigat�rio")]
        [MaxLength(14, ErrorMessage = "O CPF deve ter no m�ximo 14 caracteres")]
        public string CPF { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}