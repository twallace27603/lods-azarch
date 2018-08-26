using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aachallenges.Models
{
    public class DocumentData
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public DateTime Processed { get; set; }
        public string Status { get; set; }
    }
}
