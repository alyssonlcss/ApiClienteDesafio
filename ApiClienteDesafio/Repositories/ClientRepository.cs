using ApiClienteDesafio.Models;
using System.Collections.Generic;
using System.Linq;

namespace ApiClienteDesafio.Repositories
{
    public class ClientRepository
    {
        private static List<ClientModel> clients = new List<ClientModel>();
        private static int nextId = 1;

        public List<ClientModel> GetAll() => clients;

        public ClientModel GetById(int id) => clients.FirstOrDefault(c => c.Id == id);

        public void Add(ClientModel client)
        {
            client.Id = nextId++;
            clients.Add(client);
        }

        public void Update(ClientModel client)
        {
            var index = clients.FindIndex(c => c.Id == client.Id);
            if (index != -1)
                clients[index] = client;
        }

        public void Delete(int id)
        {
            var client = GetById(id);
            if (client != null)
                clients.Remove(client);
        }
    }
}