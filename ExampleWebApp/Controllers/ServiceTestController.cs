using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExampleWebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExampleWebApp.Controllers
{
    [Route("service")]
    public class ServiceTestController : Controller
    {
        [HttpPost]
        [Route("test")]
        public ServiceCallResult Post( serviceSettings settings)
        {
            var result = new ServiceCallResult { Success = true };
            try
            {
                result.Data = ServiceCall.Call(settings);
            } catch(Exception ex)
            {
                result.Success = false;
                result.Data = ex.Message;
            }
            return result;
        }
    }
}