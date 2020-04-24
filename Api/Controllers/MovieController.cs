using System.Collections.Generic;
using System.Threading.Tasks;
using Application.ObjectPersistenceModel;
using Domain.Communication;
using Microsoft.AspNetCore.Mvc;
namespace Api.Controllers
{
    public class MovieController : BaseController
    {
        [HttpGet]
        public async Task<ActionResult<List<MovieRankResponse>>> GetAllMovie()
        {
            return await Mediator.Send(new GetAll.Query());
        }
    }
}