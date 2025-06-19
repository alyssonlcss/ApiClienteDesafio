using ApiClienteDesafio.Models;
using System.Collections.Generic;
using System.Linq;

namespace ApiClienteDesafio.Repositories
{
    public class AddressRepository
    {
        private static List<AddressModel> addresses = new List<AddressModel>();
        private static int nextId = 1;

        public List<AddressModel> GetAll() => addresses;

        public AddressModel GetById(int id) => addresses.FirstOrDefault(a => a.Id == id);

        public List<AddressModel> GetByClientId(int clientId) => addresses.Where(a => a.ClientId == clientId).ToList();

        public void Add(AddressModel address)
        {
            address.Id = nextId++;
            addresses.Add(address);
        }

        public void Update(AddressModel address)
        {
            var index = addresses.FindIndex(a => a.Id == address.Id);
            if (index != -1)
                addresses[index] = address;
        }

        public void Delete(int id)
        {
            var address = GetById(id);
            if (address != null)
                addresses.Remove(address);
        }
    }
}