using MediatR;
using PeopleHub.Application.Features.People.Commands;
using PeopleHub.Domain.Entities;
using PeopleHub.Domain.Interfaces;

namespace PeopleHub.Application.Features.People.Handlers
{
    public class DeletePersonHandler : IRequestHandler<DeletePersonCommand, bool>
    {
        private readonly IPersonRepository _personRepository;

        public DeletePersonHandler(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public async Task<bool> Handle(DeletePersonCommand request, CancellationToken cancellationToken)
        {
            Person? person = await _personRepository.GetByIdAsync(request.Id);
            if (person == null)
            {
                throw new ArgumentException("Pessoa n�o encontrada");
            }

            return await _personRepository.DeleteAsync(request.Id);
        }
    }
}