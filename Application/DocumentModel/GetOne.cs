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
    public class GetOne
    {
        public class Query : IRequest<MovieRankResponse>
        {
            public int UserId { get; set; }
            public string MovieName { get; set; }
        }
        public class Handler : IRequestHandler<Query, MovieRankResponse>
        {
            private static string TableName = "MovieRank";
            private readonly Table _table;
            public Handler(IAmazonDynamoDB client)
            {
                _table = Table.LoadTable(client, TableName);
            }

            public async Task<MovieRankResponse> Handle(Query request, CancellationToken cancellationToken)
            {
                var response = await _table.GetItemAsync(request.UserId, request.MovieName);
                return MappingProfile.ToMovieRankResponse(response);
            }
        }
    }
}