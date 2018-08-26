using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using aachallenges.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace aachallenges.Controllers
{
    [Route("evaluate")]
    public class EvaluationController : Controller
    {
        private readonly IConfiguration Config;

        public EvaluationController(IConfiguration config)
        {
            Config = config;
        }

        [HttpPost]
        [Route("createsqltable")]
        public EvaluationResults CreateSqlTable(Parameters parms)
        {
            var results = new EvaluationResults();
            parms.Fix(Config);
            try
            {
                var context = new SqlContext(parms);
                context.CreateTable();
            }
            catch (Exception ex)
            {
                results.Code = ex.HResult;
                results.Message = $"Error: {ex.Message}";
                results.Passed = false;
            }
            return results;
        }

        [HttpPost]
        [Route("verifysqldata")]
        public EvaluationResults VerifySqlData(Parameters parms)
        {
            var results = new EvaluationResults();
            parms.Fix(Config);
            try
            {
                var context = new SqlContext(parms);
                results = context.VerifyDocumentStatus();
            }
            catch (Exception ex)
            {
                results.Code = ex.HResult;
                results.Message = $"Error: {ex.Message}";
                results.Passed = false;
            }
            return results;
        }
    }
}