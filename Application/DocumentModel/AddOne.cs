using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using MediatR;

namespace Application.DocumentModel
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
            private static string TableName = "MovieRank";
            private readonly Table _table;
            public Handler(IAmazonDynamoDB client)
            {
                _table = Table.LoadTable(client, TableName);
            }
            public async Task<Unit> Handle(AddCommand command, CancellationToken cancellationToken)
            {
                var entity = new Document
                {

                    ["UserId"] = command.UserId,
                    ["MovieName"] = command.MovieName,
                    ["Description"] = command.Description,
                    ["Ranking"] = command.Ranking,
                    ["Actors"] = command.Actors,
                    ["RankedDateTime"] = DateTime.UtcNow.ToString(),
                };
                await _table.PutItemAsync(entity);
                return Unit.Value;
            }
        }
    }
}