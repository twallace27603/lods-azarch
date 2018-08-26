using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace aachallenges.Models
{
    public class SqlContext
    {
        private SqlConnection connection;
        public SqlContext(Parameters parms)
        {
            connection = new SqlConnection(parms.SqlConnection);
        }

        public void CreateTable()
        {
            string SQL = "IF NOT EXISTS(SELECT * FROM sys.tables WHERE name = 'DocumentStatus') CREATE TABLE dbo.DocumentStatus(ID VARCHAR(300) NOT NULL PRIMARY KEY, DocumentName VARCHAR(100), DocumentStatus VARCHAR(100) DEFAULT('Uploaded'), Processed DATETIME2 NOT NULL);";
            var cmd = new SqlCommand(SQL, connection);
            connection.Open();
            try
            {
                cmd.ExecuteNonQuery();
            }
            finally
            {
                connection.Close();
            }
        }

        public EvaluationResults VerifyDocumentStatus()
        {
            var results = new EvaluationResults();
            try
            {
                string SQL = "SELECT * FROM dbo.DocumentStatus;";
                var cmd = new SqlCommand(SQL, connection);
                connection.Open();
                try
                {
                    var rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        results.Data.Add(new DocumentData
                        {
                            ID = rdr["ID"].ToString(),
                            Name = rdr["DocumentName"].ToString(),
                            Processed = (DateTime)rdr["Processed"],
                            Status = rdr["DocumentStatus"].ToString()
                        });
                    }
                    results.Passed = results.Data.Count > 0;
                    results.Code = results.Data.Count;
                    results.Message = results.Data.Count > 0 ? "Successfully generated document status records." : "No errors occurred but no document records exist.";
                }
                finally
                {
                    connection.Close();
                }

            }
            catch (Exception ex)
            {
                results.Passed = false;
                results.Code = ex.HResult;
                results.Message = $"Error: {ex.Message}";
            }

            return results;
        }
    }
}
