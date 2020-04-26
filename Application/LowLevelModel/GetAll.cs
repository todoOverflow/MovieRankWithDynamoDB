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
    public class GetAll
    {
        public class Query : IRequest<List<MovieRankResponse>>
        {
            public int UserId { get; set; }
            public string MovieName { get; set; }
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
                var scanRequest = new ScanRequest
                {
                    TableName = TableName,
                };
                var response = await _client.ScanAsync(scanRequest);
                return response.Items.Select(MappingProfile.ToMovieRankResponse).ToList();
            }
        }
    }
}