using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Domain.Communication;
using MediatR;

namespace Application.LowLevelModel
{
    public class GetOne
    {
        public class Query : IRequest<MovieRankResponse>
        {
            public int UserId { get; set; }
            public string MovieName { get; set; }
        }
        public class Handler : IRequestHandler<Query, MovieRankResponse>
        {
            private const string TableName = "MovieRank";
            private readonly IAmazonDynamoDB _client;
            public Handler(IAmazonDynamoDB client)
            {
                this._client = client;

            }

            public async Task<MovieRankResponse> Handle(Query request, CancellationToken cancellationToken)
            {
                var getItemRequest = new GetItemRequest
                {
                    TableName = TableName,
                    Key = new Dictionary<string, AttributeValue>
                    {
                        {"UserId",new AttributeValue{N = request.UserId.ToString()}},
                        {"MovieName",new AttributeValue{S = request.MovieName}},
                    }
                };
                var getItemResponse = await _client.GetItemAsync(getItemRequest);
                return MappingProfile.ToMovieRankResponse(getItemResponse);
            }
        }
    }
}