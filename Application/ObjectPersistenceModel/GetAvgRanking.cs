using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Application.DatabaseModels;
using MediatR;
using System.Linq;
using System;
using Domain.Communication;

namespace Application.ObjectPersistenceModel
{
    public class GetAvgRanking
    {
        public class Query : IRequest<MovieAvgRankingResponse>
        {
            public string MovieName { get; set; }
        }

        public class Handler : IRequestHandler<Query, MovieAvgRankingResponse>
        {
            private readonly DynamoDBContext _dbContext;
            public Handler(IAmazonDynamoDB client)
            {
                _dbContext = new DynamoDBContext(client);
            }

            public async Task<MovieAvgRankingResponse> Handle(Query request, CancellationToken cancellationToken)
            {
                var operationConfig = new DynamoDBOperationConfig
                {
                    IndexName = "MovieName-index"
                };

                var response = await _dbContext.QueryAsync<MovieRank>(request.MovieName, operationConfig).GetRemainingAsync();

                var avgRanking = Math.Round(response.Select(x => x.Ranking).Average(), 2);

                return new MovieAvgRankingResponse
                {
                    MovieName = request.MovieName,
                    AvgRanking = avgRanking,
                };
            }
        }

    }
}