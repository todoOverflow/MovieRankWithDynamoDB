using AutoMapper;
using Domain.Communication;
using System;
using Amazon.DynamoDBv2.DocumentModel;

namespace Application.DocumentModel
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
        }

        public static MovieRankResponse ToMovieRankResponse(Document doc)
        {
            return new MovieRankResponse()
            {
                MovieName = doc["MovieName"],
                Description = doc["Description"],
                Actors = doc["Actors"].AsListOfString(),
                Ranking = Convert.ToInt32(doc["Ranking"]),
                TimeRanked = doc["RankedDateTime"],
            };
        }
    }
}