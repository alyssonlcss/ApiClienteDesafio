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
            CreateMap<ClientModel, ClientDetailDTO>();
            CreateMap<ClientCreateDTO, ClientModel>();
            CreateMap<AddressModel, AddressDTO>().ReverseMap();
            CreateMap<AddressCreateDTO, AddressModel>();
            CreateMap<ContactModel, ContactDTO>().ReverseMap();
            CreateMap<ContactCreateDTO, ContactModel>();
        }
    }
}