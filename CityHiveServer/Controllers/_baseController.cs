using CityHiveInfrastructure.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace CityHiveServer.Controllers
{
    public abstract class _baseController : ControllerBase
    {
        public _baseController()
        {
        }

        protected ActionResult<T> ReturnException<T>(Exception ex)
        {
            if (ex is ReturnToClientException)
            {
                return BadRequest(ex.Message);
            }
            else if (ex is ManagedException)
            {
                return BadRequest(ex.InnerException.Message);
            }
            return BadRequest("");
        }
        protected ActionResult ReturnException(Exception ex)
        {
            if (ex is ReturnToClientException)
            {
                return BadRequest(ex.Message);
            }
            else if (ex is ManagedException)
            {
                return BadRequest(ex.InnerException.Message);
            }
            return BadRequest("");
        }

        protected string getRequestHeader(string requestKey)
        {
            StringValues parameter;
            if (HttpContext.Request.Headers.TryGetValue(requestKey, out parameter))
                return parameter.ToString();
            return null;
        }
    }
}
