using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Application.DatabaseModels;
using AutoMapper;
using MediatR;

namespace Application.ObjectPersistenceModel
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
            private readonly DynamoDBContext _dbContext;
            private readonly IMapper _mapper;
            public Handler(IAmazonDynamoDB client, IMapper mapper)
            {
                _mapper = mapper;
                _dbContext = new DynamoDBContext(client);
            }
            public async Task<Unit> Handle(AddCommand request, CancellationToken cancellationToken)
            {
                var entity = _mapper.Map<AddCommand, MovieRank>(request);
                await _dbContext.SaveAsync<MovieRank>(entity);
                return Unit.Value;
            }
        }
    }
}