using PeopleHub.Domain.Entities;

namespace PeopleHub.Domain.Interfaces
{
    public interface IPersonRepository
    {
        Task<Person?> GetByIdAsync(int id);
        Task<Person?> GetByCPFAsync(string cpf);
        Task<IEnumerable<Person>> GetAllAsync();
        Task<Person> AddAsync(Person person);
        Task<Person> UpdateAsync(Person person);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsByCPFAsync(string cpf, int? excludeId = null);
    }
}