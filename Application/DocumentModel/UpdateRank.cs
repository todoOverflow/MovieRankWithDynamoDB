using System;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using AutoMapper;
using MediatR;
namespace Application.DocumentModel
{
    public class UpdateRank
    {
        public class UpdateRankCommand : IRequest
        {
            public int UserId { get; set; }
            public string MovieName { get; set; }
            public int Ranking { get; set; }
        }

        public class Handler : IRequestHandler<UpdateRankCommand>
        {
            private static string TableName = "MovieRank";
            private readonly Table _table;
            public Handler(IAmazonDynamoDB client)
            {
                _table = Table.LoadTable(client, TableName);
            }
            public async Task<Unit> Handle(UpdateRankCommand request, CancellationToken cancellationToken)
            {
                var response = await _table.GetItemAsync(request.UserId, request.MovieName);
                if (response == null)
                {
                    throw new System.Exception($"{request.UserId} never ranked {request.MovieName}");
                }

                response["Ranking"] = request.Ranking;
                response["RankedDateTime"] = DateTime.UtcNow.ToString();
                await _table.UpdateItemAsync(response);
                return Unit.Value;
            }
        }
    }
}