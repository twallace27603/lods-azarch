using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExampleWebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ExampleWebApp.Controllers
{
    [Route("service")]
    public class ServiceTestController : Controller
    {
        private IConfiguration configuration;
        public ServiceTestController(IConfiguration config)
        {
            configuration = config;
        }
        [HttpPost]
        [Route("test")]
        public ServiceCallResult Post( serviceSettings settings)
        {
            var result = new ServiceCallResult { Success = true };
            try
            {
                if(string.IsNullOrEmpty(settings.Address))
                {
                    settings.Address = configuration["address"];
                }
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