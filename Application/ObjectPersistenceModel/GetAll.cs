using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using AutoMapper;
using Domain.Communication;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.ObjectPersistenceModel
{
    public class GetAll
    {
        public class Query : IRequest<List<MovieRankResponse>>
        {
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
                var conditions = new List<ScanCondition>();
                var response = await _dbContext.ScanAsync<MovieRank>(conditions, operationConfig: null).GetRemainingAsync();
                return _mapper.Map<List<MovieRank>, List<MovieRankResponse>>(response);
            }
        }
    }
}