using System;
using Microsoft.AspNetCore.Mvc;
using RequestResponseMiddlewareTest.Model;

namespace RequestResponseMiddlewareTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [Route("Login")]
        [HttpPost]
        public JsonResult Login([FromBody] LoginRequest login)
        {
            if (login.Account != null)
            {
                var failed = new
                {
                    Message = "LoginFailed"
                };

                return new JsonResult(failed);
            }

            var success = new
            {
                Message = "LoginSuccess"
            };

            return new JsonResult(success);
        }

        [Route("Init")]
        [HttpGet]
        public JsonResult Init()
        {
            return new JsonResult(new
            {
                IsSuccess = true
            });
        }

        [Route("Error")]
        [HttpGet]
        public JsonResult Error()
        {
            throw new Exception("Unexpected Exception");
        }
    }
}