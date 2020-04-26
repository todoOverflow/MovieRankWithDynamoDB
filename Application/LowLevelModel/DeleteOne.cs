using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using MediatR;
namespace Application.LowLevelModel
{
    public class DeleteOne
    {
        public class DeleteCommand : IRequest
        {
            public int UserId { get; set; }
            public string MovieName { get; set; }
        }
        public class Handler : IRequestHandler<DeleteCommand>
        {
            private const string TableName = "MovieRank";
            private readonly IAmazonDynamoDB _client;
            public Handler(IAmazonDynamoDB client)
            {
                this._client = client;
            }

            public async Task<Unit> Handle(DeleteCommand request, CancellationToken cancellationToken)
            {
                var deleteItemRequest = new DeleteItemRequest
                {
                    TableName = TableName,
                    Key = new Dictionary<string, AttributeValue>
                    {
                        {"UserId",new AttributeValue{N = request.UserId.ToString()}},
                        {"MovieName",new AttributeValue{S = request.MovieName}},
                    }
                };
                await _client.DeleteItemAsync(deleteItemRequest);
                return Unit.Value;
            }
        }
    }
}