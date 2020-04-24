using Application.DatabaseModels;
using AutoMapper;
using Domain.Communication;

namespace Application.ObjectPersistenceModel
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<MovieRank, MovieRankResponse>();
        }
    }
}