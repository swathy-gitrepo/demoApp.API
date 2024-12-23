using DemoApp.API.Data;
using DemoApp.API.Models.Domain;
using DemoApp.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace DemoApp.API.Repositories.Implementation
{
    public class ContactsRepository : IContactsRepository
    {
        private readonly ApplicationDbContext dbContext;

        public ContactsRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Contact> createAsync(Contact contactDetails)
        {
            await dbContext.Contacts.AddAsync(contactDetails);
            await dbContext.SaveChangesAsync();
            return contactDetails;
        }

       
        public async Task<IEnumerable<Contact>> getAllContactsAsync(string? sortBy = null,string? sortDirection = null)
        {
            var contacts = dbContext.Contacts.AsQueryable();
            // Sorting
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                if (string.Equals(sortBy, "firstName", StringComparison.OrdinalIgnoreCase))
                {
                    var isAsc = string.Equals(sortDirection, "asc", StringComparison.OrdinalIgnoreCase)
                        ? true : false;


                    contacts = isAsc ? contacts.OrderBy(x => x.FirstName) : contacts.OrderByDescending(x => x.FirstName);
                }

                if (string.Equals(sortBy, "lastName", StringComparison.OrdinalIgnoreCase))
                {
                    var isAsc = string.Equals(sortDirection, "asc", StringComparison.OrdinalIgnoreCase)
                        ? true : false;

                    contacts = isAsc ? contacts.OrderBy(x => x.LastName) : contacts.OrderByDescending(x => x.LastName);
                }
                if (string.Equals(sortBy, "email", StringComparison.OrdinalIgnoreCase))
                {
                    var isAsc = string.Equals(sortDirection, "asc", StringComparison.OrdinalIgnoreCase)
                        ? true : false;

                    contacts = isAsc ? contacts.OrderBy(x => x.Email) : contacts.OrderByDescending(x => x.Email);
                }
                if (string.Equals(sortBy, "phoneNumber", StringComparison.OrdinalIgnoreCase))
                {
                    var isAsc = string.Equals(sortDirection, "asc", StringComparison.OrdinalIgnoreCase)
                        ? true : false;

                    contacts = isAsc ? contacts.OrderBy(x => x.PhoneNumber) : contacts.OrderByDescending(x => x.PhoneNumber);
                }
                if (string.Equals(sortBy, "address", StringComparison.OrdinalIgnoreCase))
                {
                    var isAsc = string.Equals(sortDirection, "asc", StringComparison.OrdinalIgnoreCase)
                        ? true : false;

                    contacts = isAsc ? contacts.OrderBy(x => x.Address) : contacts.OrderByDescending(x => x.Address);
                }

                if (string.Equals(sortBy, "city", StringComparison.OrdinalIgnoreCase))
                {
                    var isAsc = string.Equals(sortDirection, "asc", StringComparison.OrdinalIgnoreCase)
                        ? true : false;

                    contacts = isAsc ? contacts.OrderBy(x => x.City) : contacts.OrderByDescending(x => x.City);
                }
                if (string.Equals(sortBy, "state", StringComparison.OrdinalIgnoreCase))
                {
                    var isAsc = string.Equals(sortDirection, "asc", StringComparison.OrdinalIgnoreCase)
                        ? true : false;

                    contacts = isAsc ? contacts.OrderBy(x => x.State) : contacts.OrderByDescending(x => x.State);
                }
                if (string.Equals(sortBy, "country", StringComparison.OrdinalIgnoreCase))
                {
                    var isAsc = string.Equals(sortDirection, "asc", StringComparison.OrdinalIgnoreCase)
                        ? true : false;

                    contacts = isAsc ? contacts.OrderBy(x => x.Country) : contacts.OrderByDescending(x => x.Country);
                }
                if (string.Equals(sortBy, "postalCode", StringComparison.OrdinalIgnoreCase))
                {
                    var isAsc = string.Equals(sortDirection, "asc", StringComparison.OrdinalIgnoreCase)
                        ? true : false;

                    contacts = isAsc ? contacts.OrderBy(x => x.PostalCode) : contacts.OrderByDescending(x => x.PostalCode);
                }

            }
            return await contacts.ToListAsync();
        }

        public async Task<Contact?> getContactByEmailAndPhoneNumberAsync(string email, string phoneNum)
        {
            return await dbContext.Contacts.FirstOrDefaultAsync(x => x.Email == email && x.PhoneNumber == phoneNum);
        }

        public async Task<Contact?> updateContactAsync(Contact contactDetails)
        {
            var existingContact = await dbContext.Contacts.FirstOrDefaultAsync(x=>x.Id == contactDetails.Id);
            if (existingContact != null)
            {
                dbContext.Entry(existingContact).CurrentValues.SetValues(contactDetails);
                await dbContext.SaveChangesAsync();
                return contactDetails;
            }
            else
            {
                return null;
            }
        }
        public async Task<bool> deleteContactAsync(int Id)
        {
            var existingContact = await dbContext.Contacts.FirstOrDefaultAsync(x => x.Id == Id);
            if (existingContact != null)
            {
                dbContext.Contacts.Remove(existingContact);
                await dbContext.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<Contact?> getContactByIdAsync(int id)
        {
            return await dbContext.Contacts.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
