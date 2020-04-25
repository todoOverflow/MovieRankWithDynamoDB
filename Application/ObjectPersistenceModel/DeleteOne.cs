using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Application.DatabaseModels;
using MediatR;

namespace Application.ObjectPersistenceModel
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
            private readonly DynamoDBContext _dbContext;
            public Handler(IAmazonDynamoDB client)
            {
                _dbContext = new DynamoDBContext(client);
            }

            public async Task<Unit> Handle(DeleteCommand request, CancellationToken cancellationToken)
            {
                await _dbContext.DeleteAsync<MovieRank>(request.UserId, request.MovieName);
                return Unit.Value;
            }
        }
    }
}