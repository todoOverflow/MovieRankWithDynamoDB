using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using MediatR;

namespace Application.LowLevelModel
{
    public class CreateTable
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
                var createTableRequest = new CreateTableRequest
                {
                    TableName = request.TableName,
                    AttributeDefinitions = new List<AttributeDefinition>
                    {
                        new AttributeDefinition{ AttributeName = "Id", AttributeType = "N"}
                    },
                    KeySchema = new List<KeySchemaElement>()
                    {
                        new KeySchemaElement { AttributeName="Id", KeyType = "Hash"}
                    },
                    ProvisionedThroughput = new ProvisionedThroughput { ReadCapacityUnits = 1, WriteCapacityUnits = 1 }
                };
                await _client.CreateTableAsync(createTableRequest);
                return Unit.Value;
            }
        }

    }
}