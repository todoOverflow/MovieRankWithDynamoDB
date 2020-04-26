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
    public class GetMovieByTitle
    {
        public class Query : IRequest<List<MovieRankResponse>>
        {
            public int UserId { get; set; }
            public string MovieTitle { get; set; }
        }

        public class Handler : IRequestHandler<Query, List<MovieRankResponse>>
        {
            private const string TableName = "MovieRank";
            private readonly IAmazonDynamoDB _client;
            public Handler(IAmazonDynamoDB client)
            {
                this._client = client;
            }

            public async Task<List<MovieRankResponse>> Handle(Query request, CancellationToken cancellationToken)
            {
                var queryReqeust = new QueryRequest
                {
                    TableName = TableName,
                    KeyConditionExpression = "UserId = :userId and begins_with(MovieName, :movieTitle)",
                    ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                    {
                        {":userId",new AttributeValue{N = request.UserId.ToString()}},
                        {":movieTitle",new AttributeValue{S= request.MovieTitle}}
                    }
                };

                var response = await _client.QueryAsync(queryReqeust);
                return response.Items.Select(MappingProfile.ToMovieRankResponse).ToList();
            }
        }

    }
}