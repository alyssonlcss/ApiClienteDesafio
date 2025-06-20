using AutoMapper;
using ApiClienteDesafio.Models;
using ApiClienteDesafio.DTOs;
using ApiClienteDesafio.Integration;

namespace ApiClienteDesafio.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ClientModel, ClientDTO>().ReverseMap();
            CreateMap<ClientModel, ClientDetailDTO>().ReverseMap();
            CreateMap<ClientCreateDTO, ClientModel>();
            CreateMap<AddressModel, AddressDTO>().ReverseMap();
            CreateMap<AddressCreateDTO, AddressModel>();
            CreateMap<ContactModel, ContactDTO>().ReverseMap();
            CreateMap<ContactCreateDTO, ContactModel>();

            // Mapeamento ViaCepResponse -> AddressModel
            CreateMap<ViaCepResponse, AddressModel>()
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Logradouro))
                .ForMember(dest => dest.Neighborhood, opt => opt.MapFrom(src => src.Bairro))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Localidade))
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.Uf));
        }
    }
}