using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ServerContainerManager.API.Extensions;
using ServerContainerManager.API.Models.Requests.ContainersController;
using ServerContainerManager.API.Models.Responses.ContainersController;
using ServerContainerManager.API.Models.Responses.Extensions;
using ServerContainerManager.Application.Consts;
using ServerContainerManager.Application.Queries.Abstraction;
using ServerContainerManager.Application.Queries.GetContainerList;

namespace ServerContainerManager.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ContainersController(ILogger<ContainersController> logger) : ControllerBase
    {
        private readonly ILogger<ContainersController> _logger = logger;

        [HttpGet]
        [Authorize(Roles = UserRoles.Member)]
        public async Task<Results<Ok<GetContainerListResponse>, ProblemHttpResult>> GetContainers(
            [FromQuery] GetContainersRequest request,
            [FromServices] IQueryHandler<GetContainerListQuery, GetContainerListQueryResult> queryHandler,
            CancellationToken cancellationToken = default)
        {
            var query = new GetContainerListQuery
            {
                UserId = User.GetUserId(),
                Skip = request.Skip,
                Take = request.Take,
                SortBy = request.SortBy,
                Order = request.Order
            };

            var result = await queryHandler.HandleAsync(query, cancellationToken);

            if (result.IsError)
                return result.Errors.ToProblemHttpResult();

            return TypedResults.Ok(result.Value.ToContract());
        }
    }
}
