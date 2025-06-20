using ApiClienteDesafio.Models;

namespace ApiClienteDesafio.Utils
{
    public static class AddressUtils
    {
        public static void ApplyViaCepData(AddressModel address, AddressModel viaCepAddress)
        {
            address.Street = viaCepAddress.Street;
            address.Neighborhood = viaCepAddress.Neighborhood;
            address.City = viaCepAddress.City;
            address.State = viaCepAddress.State;
        }
    }
}
