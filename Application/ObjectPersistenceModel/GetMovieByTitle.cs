using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using AutoMapper;
using Domain.Communication;
using MediatR;
using Amazon.DynamoDBv2.DocumentModel;

namespace Application.ObjectPersistenceModel
{
    public class GetMovieByTitle
    {
        public class Query : IRequest<List<MovieRankResponse>>
        {
            public int UserId;
            public string MovieTitle;
        }
        public class Handler : IRequestHandler<Query, List<MovieRankResponse>>
        {
            private readonly DynamoDBContext _dbContext;
            private readonly IMapper _mapper;
            public Handler(IAmazonDynamoDB client, IMapper mapper)
            {
                _mapper = mapper;
                _dbContext = new DynamoDBContext(client);
            }

            public async Task<List<MovieRankResponse>> Handle(Query request, CancellationToken cancellationToken)
            {
                var conditions = new List<ScanCondition> { new ScanCondition("MovieName", ScanOperator.BeginsWith, request.MovieTitle) };

                var operationConfig = new DynamoDBOperationConfig
                {
                    QueryFilter = conditions,
                };

                var response = await _dbContext.QueryAsync<MovieRank>(request.UserId, operationConfig).GetRemainingAsync();
                return _mapper.Map<List<MovieRank>, List<MovieRankResponse>>(response);
            }
        }
    }
}