using AutoMapper;
using ApiClienteDesafio.Models;
using ApiClienteDesafio.DTOs;

namespace ApiClienteDesafio.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ClientModel, ClientDTO>().ReverseMap();
            CreateMap<AddressModel, AddressDTO>().ReverseMap();
            CreateMap<ContactModel, ContactDTO>().ReverseMap();
        }
    }
}