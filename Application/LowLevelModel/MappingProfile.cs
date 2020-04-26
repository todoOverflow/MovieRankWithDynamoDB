using AutoMapper;
using Domain.Communication;
using System;
using Amazon.DynamoDBv2.Model;
using System.Collections.Generic;

namespace Application.LowLevelModel
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
        }

        public static MovieRankResponse ToMovieRankResponse(GetItemResponse response)
        {
            return new MovieRankResponse()
            {
                MovieName = response.Item["MovieName"].S,
                Description = response.Item["Description"].S,
                Actors = response.Item["Actors"].SS,
                Ranking = Convert.ToInt32(response.Item["Ranking"].N),
                TimeRanked = response.Item["RankedDateTime"].S,
            };
        }

        public static MovieRankResponse ToMovieRankResponse(Dictionary<string, AttributeValue> item)
        {
            return new MovieRankResponse
            {
                MovieName = item["MovieName"].S,
                Description = item["Description"].S,
                Actors = item["Actors"].SS,
                Ranking = Convert.ToInt32(item["Ranking"].N),
                TimeRanked = item["RankedDateTime"].S,
            };
        }
    }
}