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

        public AddressModel GetByClientId(int clientId) => addresses.FirstOrDefault(a => a.ClientId == clientId);

        public void Add(int clientId, AddressModel address)
        {
            // Só permite um Address por ClientId
            if (addresses.Any(a => a.ClientId == clientId))
                return;
            address.AddressId = nextId++;
            address.ClientId = clientId;
            addresses.Add(address);
        }

        public void Update(int clientId, AddressModel address)
        {
            var index = addresses.FindIndex(a => a.ClientId == clientId);
            if (index != -1)
                addresses[index] = address;
        }

        public void Delete(int clientId)
        {
            var address = GetByClientId(clientId);
            if (address != null)
                addresses.Remove(address);
        }
    }
}