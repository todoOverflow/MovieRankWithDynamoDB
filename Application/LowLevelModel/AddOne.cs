using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using MediatR;

namespace Application.LowLevelModel
{
    public class AddOne
    {
        public class AddCommand : IRequest
        {
            public int UserId { get; set; }
            public string MovieName { get; set; }
            public string Description { get; set; }
            public List<string> Actors { get; set; }
            public int Ranking { get; set; }
        }

        public class Handler : IRequestHandler<AddCommand>
        {
            private const string TableName = "MovieRank";
            private readonly IAmazonDynamoDB _client;
            public Handler(IAmazonDynamoDB client)
            {
                this._client = client;
            }

            public async Task<Unit> Handle(AddCommand request, CancellationToken cancellationToken)
            {
                var putItemRequest = new PutItemRequest
                {
                    TableName = TableName,
                    Item = new Dictionary<string, AttributeValue>
                    {
                        {"UserId",new AttributeValue{N = request.UserId.ToString()}},
                        {"MovieName",new AttributeValue{S = request.MovieName}},
                        {"Description",new AttributeValue{S = request.Description}},
                        {"Actors",new AttributeValue{SS = request.Actors}},
                        {"Ranking",new AttributeValue{N = request.Ranking.ToString()}},
                        {"Ranking",new AttributeValue{ S = DateTime.UtcNow.ToString()}},
                    }
                };
                await _client.PutItemAsync(putItemRequest);
                return Unit.Value;
            }
        }
    }
}