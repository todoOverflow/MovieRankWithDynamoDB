using System;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using MediatR;


namespace Application.ObjectPersistenceModel
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
            private readonly DynamoDBContext _dbContext;
            public Handler(IAmazonDynamoDB client)
            {
                _dbContext = new DynamoDBContext(client);
            }
            public async Task<Unit> Handle(UpdateRankCommand request, CancellationToken cancellationToken)
            {
                var response = await _dbContext.LoadAsync<MovieRank>(request.UserId, request.MovieName);
                if (response == null)
                {
                    throw new System.Exception($"{request.UserId} never ranked {request.MovieName}");
                }

                response.Ranking = request.Ranking;
                response.RankedDateTime = DateTime.UtcNow.ToString();
                await _dbContext.SaveAsync<MovieRank>(response);
                return Unit.Value;
            }
        }
    }
}