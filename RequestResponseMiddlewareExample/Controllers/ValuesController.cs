using Microsoft.AspNetCore.Mvc;

namespace RequestResponseMiddlewareTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // POST api/values
        [Route("Login")]
        [HttpPost]
        public JsonResult Login([FromBody] LoginRequest login)
        {
            if (login.Account != null)
            {
                var failed = new
                {
                    Message = "loginFailed"
                };

                return new JsonResult(failed);
            }

            var success = new
            {
                Message = "loginFailed"
            };

            return new JsonResult(success);
        }

        [Route("Test")]
        [HttpGet]
        public JsonResult Test()
        {
            return new JsonResult(new
            {
                IsSuccess = true
            });
        }
    }

    public class LoginRequest
    {
        public string Account { get; set; }
    }
}