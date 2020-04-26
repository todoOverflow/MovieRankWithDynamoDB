using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using AutoMapper;
using Domain.Communication;
using MediatR;

namespace Application.DocumentModel
{
    public class GetMovieByTitle
    {
        public class Query : IRequest<List<MovieRankResponse>>
        {
            public int UserId { get; set; }
            public string MovieTitle { get; set; }
        }
        public class Handler : IRequestHandler<Query, List<MovieRankResponse>>
        {
            private static string TableName = "MovieRank";
            private readonly Table _table;
            public Handler(IAmazonDynamoDB client)
            {
                _table = Table.LoadTable(client, TableName);
            }

            public async Task<List<MovieRankResponse>> Handle(Query request, CancellationToken cancellationToken)
            {
                var filter = new QueryFilter("UserId", QueryOperator.Equal, request.UserId);
                filter.AddCondition("MovieName", QueryOperator.BeginsWith, request.MovieTitle);

                var response = await _table.Query(filter).GetRemainingAsync();
                return response.Select(MappingProfile.ToMovieRankResponse).ToList();
            }
        }
    }
}