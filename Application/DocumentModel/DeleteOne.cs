using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using AutoMapper;
using Domain.Communication;
using MediatR;

namespace Application.DocumentModel
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
            private static string TableName = "MovieRank";
            private readonly Table _table;
            public Handler(IAmazonDynamoDB client)
            {
                _table = Table.LoadTable(client, TableName);
            }
            public async Task<Unit> Handle(DeleteCommand request, CancellationToken cancellationToken)
            {
                await _table.DeleteItemAsync(request.UserId, request.MovieName);
                return Unit.Value;
            }
        }
    }
}