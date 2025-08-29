using System.ComponentModel.DataAnnotations;

namespace PeopleHub.Application.DTOs
{
    public class PersonDtoV2 : PersonDtoV1
    {
        [Required(ErrorMessage = "O endereço é obrigatório na versão 2")]
        [MaxLength(500, ErrorMessage = "O endereço deve ter no máximo 500 caracteres")]
        public string Endereco { get; set; } = string.Empty;
    }
}