using System.ComponentModel.DataAnnotations;

namespace PeopleHub.Application.DTOs
{
    public class PersonDtoV2 : PersonDtoV1
    {
        [Required(ErrorMessage = "O endere�o � obrigat�rio na vers�o 2")]
        [MaxLength(500, ErrorMessage = "O endere�o deve ter no m�ximo 500 caracteres")]
        public string Endereco { get; set; } = string.Empty;
    }
}