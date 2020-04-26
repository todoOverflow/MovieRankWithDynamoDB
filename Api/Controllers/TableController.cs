using System.Threading.Tasks;
using Application.LowLevelModel;
using MediatR;
using Microsoft.AspNetCore.Mvc;
namespace Api.Controllers
{
    public class TableController : BaseController
    {
        [HttpPost]
        [Route("create/{tableName}")]
        public async Task<ActionResult<Unit>> CreateDynamoDBTable(string tableName)
        {
            return await Mediator.Send(new CreateTable.Command { TableName = tableName });
        }

        [HttpDelete]
        [Route("delete/{tableName}")]
        public async Task<ActionResult<Unit>> DeleteDynamoDBTable(string tableName)
        {
            return await Mediator.Send(new DeleteTable.Command { TableName = tableName });
        }
    }
}