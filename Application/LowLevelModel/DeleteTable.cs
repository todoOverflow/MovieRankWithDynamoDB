using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using MediatR;
namespace Application.LowLevelModel
{
    public class DeleteTable
    {
        public class Command : IRequest
        {
            public string TableName { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly IAmazonDynamoDB _client;
            public Handler(IAmazonDynamoDB client)
            {
                this._client = client;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var deleteTableRequest = new DeleteTableRequest
                {
                    TableName = request.TableName,
                };
                await _client.DeleteTableAsync(deleteTableRequest);
                return Unit.Value;
            }
        }
    }
}