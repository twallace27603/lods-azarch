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
        [HttpPost]
        [Route("loadqueue")]
        public async Task<EvaluationResults> LoadQueue(Parameters parms)
        {
            var results = new EvaluationResults();
            parms.Fix(Config);
            try
            {
                var context = new StorageContext(parms);
                results = await context.LoadQueue("process");
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
        [Route("verifyqueue")]
        public async Task<EvaluationResults> VerifyQueue(Parameters parms)
        {
            var results = new EvaluationResults();
            parms.Fix(Config);
            try
            {
                var context = new StorageContext(parms);
                results = await context.CheckQueue("process");
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
        [Route("verifytable")]
        public async Task<EvaluationResults> VerifyTable(Parameters parms)
        {
            var results = new EvaluationResults();
            parms.Fix(Config);
            try
            {
                var context = new StorageContext(parms);
                results = await context.CheckTable("history");
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
        [Route("loadtable")]
        public async Task<EvaluationResults> LoadTable(Parameters parms)
        {
            var results = new EvaluationResults();
            parms.Fix(Config);
            try
            {
                var context = new StorageContext(parms);
                results = await context.LoadTable("history");
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