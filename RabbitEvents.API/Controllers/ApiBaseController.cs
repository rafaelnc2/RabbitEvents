using Microsoft.AspNetCore.Mvc;
using RabbitEvents.Shared.Responses;

namespace RabbitEvents.API.Controllers;

public class ApiBaseController : ControllerBase
{
    protected JsonResult ApiResult<T>(ApiResponse<T> data)
    {
        return new JsonResult(data)
        {
            StatusCode = (int)data.StatusCode
        };
    }
}
