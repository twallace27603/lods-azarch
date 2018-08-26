using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aachallenges.Models
{
    public class EvaluationResults
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public Boolean Passed { get; set; }
        public List<DocumentData> Data { get; set; }

        public EvaluationResults()
        {
            Code = 0;
            Message = "Passed";
            Passed = true;
            Data = new List<DocumentData>();
        }
    }


}
