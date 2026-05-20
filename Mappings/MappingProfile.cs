using AutoMapper;
using VetApi.DTOs;
using VetApi.Models;

namespace VetApi.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Tutor, TutorDto>();
        CreateMap<CreateTutorDto, Tutor>();
        CreateMap<UpdateTutorDto, Tutor>()
            .ForMember(d => d.AtualizadoEm, o => o.MapFrom(_ => DateTime.UtcNow));

        CreateMap<Pet, PetDto>()
            .ForMember(d => d.TutorNome, o => o.MapFrom(s => s.Tutor.Nome));
        CreateMap<CreatePetDto, Pet>();
        CreateMap<UpdatePetDto, Pet>()
            .ForMember(d => d.AtualizadoEm, o => o.MapFrom(_ => DateTime.UtcNow));

        CreateMap<Consulta, ConsultaDto>()
            .ForMember(d => d.PetNome, o => o.MapFrom(s => s.Pet.Nome))
            .ForMember(d => d.TutorNome, o => o.MapFrom(s => s.Pet.Tutor.Nome));
        CreateMap<CreateConsultaDto, Consulta>();
        CreateMap<UpdateConsultaDto, Consulta>()
            .ForMember(d => d.AtualizadoEm, o => o.MapFrom(_ => DateTime.UtcNow));

        CreateMap<Vacinacao, VacinacaoDto>()
            .ForMember(d => d.PetNome, o => o.MapFrom(s => s.Pet.Nome));
        CreateMap<CreateVacinacaoDto, Vacinacao>();

        CreateMap<Exame, ExameDto>()
            .ForMember(d => d.PetNome, o => o.MapFrom(s => s.Pet.Nome));
        CreateMap<CreateExameDto, Exame>();
        CreateMap<UpdateExameDto, Exame>()
            .ForMember(d => d.AtualizadoEm, o => o.MapFrom(_ => DateTime.UtcNow));
    }
}
