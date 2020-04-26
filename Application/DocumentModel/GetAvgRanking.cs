using System;
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
    public class GetAvgRanking
    {
        public class Query : IRequest<MovieAvgRankingResponse>
        {
            public string MovieName { get; set; }
        }

        public class Handler : IRequestHandler<Query, MovieAvgRankingResponse>
        {
            private static string TableName = "MovieRank";
            private readonly Table _table;
            public Handler(IAmazonDynamoDB client)
            {
                _table = Table.LoadTable(client, TableName);
            }

            public async Task<MovieAvgRankingResponse> Handle(Query request, CancellationToken cancellationToken)
            {
                var filter = new QueryFilter("MovieName", QueryOperator.Equal, request.MovieName);
                var config = new QueryOperationConfig
                {
                    IndexName = "MovieName-index",
                    Filter = filter,
                };
                var response = await _table.Query(config).GetRemainingAsync();
                var avgRanking = Math.Round(response.Select(x => x["Ranking"].AsInt()).Average(), 2);
                return new MovieAvgRankingResponse
                {
                    MovieName = request.MovieName,
                    AvgRanking = avgRanking,
                };
            }
        }
    }
}