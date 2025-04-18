using UserRegistrationForm.Models;

namespace UserRegistrationForm.Repository
{
    public interface IContactRepository
    {
        Task<IEnumerable<Contact>> GetAllContacts();
        Task<Contact> GetContactById(int id);
        Task<int> AddContact(Contact contact);
        Task UpdateContact(Contact contact);
        Task DeleteContact(int id);
        Task<IEnumerable<State>> GetAllStates();
        Task<IEnumerable<City>> GetCitiesByState(int stateId);
    }
}
