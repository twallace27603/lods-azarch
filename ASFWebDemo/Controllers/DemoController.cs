using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ASFWebDemo.Controllers
{
    [Route("evaluation")]
    [ApiController]
    public class DemoController : ControllerBase
    {
        [HttpGet]
        [Route("connect")]
        public EvaluationResult Connect()
        {
            return new EvaluationResult { Message = "Connected to Serice Fabric node.", Code = 0, Passed = true };
        }
    }

    public class EvaluationResult
    {
        public string Message { get; set; }
        public bool Passed { get; set; }
        public int Code { get; set; }
    }
}