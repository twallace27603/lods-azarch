using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aachallenges.Models
{
    public class Parameters
    {
        public string SqlConnection { get; set; }
        public string StorageConnection { get; set; }

        public void Fix(IConfiguration config)
        {
            SqlConnection = fixStringProperty(config["ConnectionStrings:sqlConnection"], SqlConnection);
            StorageConnection = fixStringProperty(config["ConnectionStrings:storageConnection"], StorageConnection);
        }
        private string fixStringProperty(string replace, string prop)
        {
            return (string.IsNullOrEmpty(prop) || (prop == "-1")) ? replace : prop;
        }
        private int fixIntProperty(int prop, string replace)
        {
            return ((prop == 0) || (prop == -1)) ? int.Parse(replace) : prop;
        }
    }
}
