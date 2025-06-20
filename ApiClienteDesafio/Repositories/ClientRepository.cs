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

        public ClientModel GetById(int clientId) => clients.FirstOrDefault(c => c.ClientId == clientId);

        public void Add(ClientModel client)
        {
            client.ClientId = nextId++;
            clients.Add(client);
        }

        public void Update(ClientModel client)
        {
            var index = clients.FindIndex(c => c.ClientId == client.ClientId);
            if (index != -1)
                clients[index] = client;
        }

        public void Delete(int clientId)
        {
            var client = GetById(clientId);
            if (client != null)
                clients.Remove(client);
        }
    }
}