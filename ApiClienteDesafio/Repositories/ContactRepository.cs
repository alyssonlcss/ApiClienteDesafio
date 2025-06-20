using ApiClienteDesafio.Models;
using System.Collections.Generic;
using System.Linq;

namespace ApiClienteDesafio.Repositories
{
    public class ContactRepository
    {
        private static List<ContactModel> contacts = new List<ContactModel>();
        private static int nextId = 1;

        public List<ContactModel> GetAll() => contacts;

        public ContactModel GetByClientId(int clientId) => contacts.FirstOrDefault(c => c.ClientId == clientId);

        public void Add(int clientId, ContactModel contact)
        {
            // Só permite um Contact por ClientId
            if (contacts.Any(c => c.ClientId == clientId))
                return;
            contact.ContactId = nextId++;
            contact.ClientId = clientId;
            contacts.Add(contact);
        }

        public void Update(int clientId, ContactModel contact)
        {
            var index = contacts.FindIndex(c => c.ClientId == clientId);
            if (index != -1)
                contacts[index] = contact;
        }

        public void Delete(int clientId)
        {
            var contact = GetByClientId(clientId);
            if (contact != null)
                contacts.Remove(contact);
        }
    }
}