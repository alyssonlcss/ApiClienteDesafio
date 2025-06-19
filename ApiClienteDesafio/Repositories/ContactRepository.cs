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

        public ContactModel GetById(int id) => contacts.FirstOrDefault(c => c.Id == id);

        public List<ContactModel> GetByClientId(int clientId) => contacts.Where(c => c.ClientId == clientId).ToList();

        public void Add(ContactModel contact)
        {
            contact.Id = nextId++;
            contacts.Add(contact);
        }

        public void Update(ContactModel contact)
        {
            var index = contacts.FindIndex(c => c.Id == contact.Id);
            if (index != -1)
                contacts[index] = contact;
        }

        public void Delete(int id)
        {
            var contact = GetById(id);
            if (contact != null)
                contacts.Remove(contact);
        }
    }
}