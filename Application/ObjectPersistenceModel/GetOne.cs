using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Application.DatabaseModels;
using AutoMapper;
using Domain.Communication;
using MediatR;
namespace Application.ObjectPersistenceModel
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
            private readonly DynamoDBContext _dbContext;
            private readonly IMapper _mapper;
            public Handler(IAmazonDynamoDB client, IMapper mapper)
            {
                _mapper = mapper;
                _dbContext = new DynamoDBContext(client);
            }

            public async Task<MovieRankResponse> Handle(Query request, CancellationToken cancellationToken)
            {
                var response = await _dbContext.LoadAsync<MovieRank>(request.UserId, request.MovieName);
                return _mapper.Map<MovieRank, MovieRankResponse>(response);
            }
        }
    }
}