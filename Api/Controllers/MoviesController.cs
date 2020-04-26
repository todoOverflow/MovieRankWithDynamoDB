using System.Collections.Generic;
using System.Threading.Tasks;
//using Application.ObjectPersistenceModel;
using Application.DocumentModel;
using Domain.Communication;
using MediatR;
using Microsoft.AspNetCore.Mvc;
namespace Api.Controllers
{
    public class MoviesController : BaseController
    {
        [HttpGet]
        public async Task<ActionResult<List<MovieRankResponse>>> GetAllMovie()
        {
            return await Mediator.Send(new GetAll.Query());
        }

        [HttpGet]
        [Route("user/{userId}/{movieName}")]
        public async Task<ActionResult<MovieRankResponse>> GetOneMovie(int userId, string movieName)
        {
            return await Mediator.Send(new GetOne.Query { UserId = userId, MovieName = movieName });
        }

        [HttpPost]
        [Route("user/{userId}")]
        public async Task<Unit> AddMovie(int userId, AddOne.AddCommand command)
        {
            command.UserId = userId;
            return await Mediator.Send(command);
        }

        [HttpDelete]
        [Route("user/{userId}")]
        public async Task<Unit> DeleteMovie(int userId, DeleteOne.DeleteCommand command)
        {
            command.UserId = userId;
            return await Mediator.Send(command);
        }

        [HttpPatch]
        [Route("{userId}")]
        public async Task<Unit> UpdateMovieRank(int userId, UpdateRank.UpdateRankCommand command)
        {
            command.UserId = userId;
            return await Mediator.Send(command);
        }

        [HttpGet]
        [Route("{movieName}/ranking")]
        public async Task<ActionResult<MovieAvgRankingResponse>> GetMovieAvgRanking(string movieName)
        {
            return await Mediator.Send(new GetAvgRanking.Query { MovieName = movieName });
        }

        [HttpGet]
        [Route("user/{userId}/rankedMovies/{movieTitle}")]
        public async Task<ActionResult<List<MovieRankResponse>>> GetUserRankedMoviesByTitle(int userId, string movieTitle)
        {
            return await Mediator.Send(new GetMovieByTitle.Query { UserId = userId, MovieTitle = movieTitle });
        }

    }
}