using MediatR;
using PeopleHub.Application.DTOs;

namespace PeopleHub.Application.Features.People.Queries
{
    public class GetPersonByIdQuery : IRequest<PersonDtoV1?>
    {
        public int Id { get; set; }

        public GetPersonByIdQuery(int id)
        {
            Id = id;
        }
    }
}