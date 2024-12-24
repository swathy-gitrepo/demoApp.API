using DemoApp.API.Models.Domain;

namespace DemoApp.API.Repositories.Interface
{
    public interface IContactsRepository
    {
        Task<IEnumerable<Contact>> getAllContactsAsync(string? sortBy = null,string? sortDirection = null);
        Task<Contact> createAsync(Contact contactDetails);

        Task<Contact?> getContactByIdAsync(int id);
        Task<Contact?> getContactByEmailAndPhoneNumberAsync(string email,string phoneNum);

        Task<Contact?> updateContactAsync(Contact contactDetails);
        Task<bool> deleteContactAsync(int Id);

        Task<bool> isduplicateContact(Contact contact);
    }
}
