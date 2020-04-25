using Application.DatabaseModels;
using AutoMapper;
using Domain.Communication;
using System;
using static Application.ObjectPersistenceModel.AddOne;

namespace Application.ObjectPersistenceModel
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<MovieRank, MovieRankResponse>();
            CreateMap<AddCommand, MovieRank>()
                .ForMember(mr => mr.RankedDateTime, opt => opt.MapFrom(ac => DateTime.UtcNow.ToString()));
        }
    }
}