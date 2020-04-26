using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using MediatR;

namespace Application.LowLevelModel
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
            private readonly IAmazonDynamoDB _client;

            public Handler(IAmazonDynamoDB client)
            {
                this._client = client;
            }
            public async Task<Unit> Handle(UpdateRankCommand request, CancellationToken cancellationToken)
            {
                var updateItemRequest = new UpdateItemRequest
                {
                    TableName = TableName,
                    Key = new Dictionary<string, AttributeValue>
                    {
                        {"UserId",new AttributeValue{N = request.UserId.ToString()}},
                        {"MovieName",new AttributeValue{S = request.MovieName}},
                    },
                    AttributeUpdates = new Dictionary<string, AttributeValueUpdate>
                    {
                        {"Ranking",new AttributeValueUpdate
                            {
                                Action = AttributeAction.PUT,
                                Value = new AttributeValue{N = request.Ranking.ToString()}
                            }
                        },
                        {"RankedDataTime",new AttributeValueUpdate
                            {
                                Action = AttributeAction.PUT,
                                Value = new AttributeValue{S= DateTime.UtcNow.ToString()}
                            }
                        },
                    },
                };

                await _client.UpdateItemAsync(updateItemRequest);
                return Unit.Value;
            }
        }

    }
}