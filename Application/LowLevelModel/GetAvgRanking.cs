using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Domain.Communication;
using MediatR;

namespace Application.LowLevelModel
{
    public class GetAvgRanking
    {
        public class Query : IRequest<MovieAvgRankingResponse>
        {
            public string MovieName { get; set; }
        }

        public class Handler : IRequestHandler<Query, MovieAvgRankingResponse>
        {
            private const string TableName = "MovieRank";
            private readonly IAmazonDynamoDB _client;
            public Handler(IAmazonDynamoDB client)
            {
                this._client = client;
            }

            public async Task<MovieAvgRankingResponse> Handle(Query request, CancellationToken cancellationToken)
            {
                var queryReqeust = new QueryRequest
                {
                    TableName = TableName,
                    IndexName = "MovieName-index",
                    KeyConditionExpression = "MovieName = :movieName",
                    ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                    {
                        {":movieName",new AttributeValue{S= request.MovieName}}
                    }
                };

                var response = await _client.QueryAsync(queryReqeust);
                var avgRanking = Math.Round(response.Items.Select(x => Convert.ToInt32(x["Ranking"].N)).Average());
                return new MovieAvgRankingResponse
                {
                    MovieName = request.MovieName,
                    AvgRanking = avgRanking
                };
            }
        }

    }
}