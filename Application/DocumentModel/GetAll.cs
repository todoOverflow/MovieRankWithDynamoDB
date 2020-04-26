using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using AutoMapper;
using Domain.Communication;
using MediatR;

namespace Application.DocumentModel
{
    public class GetAll
    {
        public class Query : IRequest<List<MovieRankResponse>>
        {
        }
        public class Handler : IRequestHandler<Query, List<MovieRankResponse>>
        {
            private static string TableName = "MovieRank";
            private readonly Table _table;
            public Handler(IAmazonDynamoDB client)
            {
                _table = Table.LoadTable(client, TableName);
            }

            public async Task<List<MovieRankResponse>> Handle(Query request, CancellationToken cancellationToken)
            {
                var config = new ScanOperationConfig();
                var response = await _table.Scan(config).GetRemainingAsync();
                return response.Select(MappingProfile.ToMovieRankResponse).ToList();
            }
        }
    }
}