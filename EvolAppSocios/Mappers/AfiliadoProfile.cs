using AutoMapper;
using EvolApp.Shared.DTOs;
using EvolApp.Shared.Models;

namespace EvolAppSocios.Mappers;

public class AfiliadoProfile : Profile
{
    public AfiliadoProfile()
    {
        CreateMap<AfiliadoDto, Afiliado>();
        CreateMap<Afiliado, AfiliadoDto>();
    }
}
