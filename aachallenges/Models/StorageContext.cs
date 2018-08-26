using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Queue;
using System.Threading;

namespace aachallenges.Models
{
    public class StorageContext
    {
        CloudStorageAccount account;
        public StorageContext(Parameters parms)
        {
            account = CloudStorageAccount.Parse(parms.StorageConnection);
        }

        public async Task<EvaluationResults> UploadSamples(string containerName)
        {
            var results = new EvaluationResults();
            string payload = "This is a sample document.  This isn't really exciting and you are probably pretty disappointed if you opened this file.";
            try
            {
                var client = account.CreateCloudBlobClient();
                var container = client.GetContainerReference(containerName);
                if (!await container.ExistsAsync())
                {
                    results.Passed = false;
                    results.Code = -1;
                    results.Message = $"Container {containerName} does not exist.";
                    return results;
                }
                for (int i = 0; i < 3; i++)
                {
                    var blob = container.GetBlockBlobReference($"{Guid.NewGuid()}.txt");
                    await blob.UploadTextAsync(payload);
                    results.Message = $"Successfully uploaded 3 documents to {containerName}";
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

        public async Task<EvaluationResults> LoadQueue(string queueName)
        {
            var results = new EvaluationResults() {Message = $"Successfully send 3 messages to the {queueName} queue" };
            try
            {
                var client = account.CreateCloudQueueClient();
                var queue = client.GetQueueReference(queueName);
                if (!await queue.ExistsAsync())
                {
                    results.Passed = false;
                    results.Code = -1;
                    results.Message = $"Storage queue {queueName} was not found.";
                    return results;
                }

                for(int i = 0; i < 3; i++)
                {
                    await queue.AddMessageAsync(new CloudQueueMessage($"Sample message {i}"));
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


        public async Task<EvaluationResults> CheckQueue(string queueName)
        {
            var results = new EvaluationResults();
            try
            {
                var client = account.CreateCloudQueueClient();
                var queue = client.GetQueueReference(queueName);
                if (!await queue.ExistsAsync())
                {
                    results.Passed = false;
                    results.Code = -1;
                    results.Message = $"Storage queue {queueName} was not found.";
                    return results;
                }
                var retry = 0;
                while (retry < 3 && results.Data.Count == 0)
                {
                    var msg = await queue.GetMessageAsync();
                    while (msg != null)
                    {
                        results.Data.Add(new DocumentData { Name = msg.AsString });
                        msg = await queue.GetMessageAsync();
                    }
                    if (results.Data.Count == 0)
                    {
                        Thread.Sleep(15000);
                        retry++;

                    }
                }
                if (results.Data.Count > 0)
                {
                    results.Message = $"{results.Data.Count} messages are in the queue.";
                }
                else
                {
                    results.Passed = false;
                    results.Code = -1;
                    results.Message = "No errors occured but there are no messages in the queue.";
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

        public async Task<EvaluationResults> LoadTable(string tableName)
        {
            var results = new EvaluationResults { Message = $"Successfully uploaded 3 items to the {tableName} table." };
            //TODO: Add logic for storage table check
            try
            {
                var client = account.CreateCloudTableClient();
                var table = client.GetTableReference(tableName);
                if (!await table.ExistsAsync())
                {
                    results.Passed = false;
                    results.Code = -1;
                    results.Message = $"Table {tableName} does not exist.";
                    return results;
                }
                var batch = new TableBatchOperation();
                var partition = Guid.NewGuid().ToString();
                for (int i = 0; i < 3; i++)
                {
                    batch.Add(TableOperation.Insert(new DocumentEntity
                    {
                        PartitionKey = partition,
                        ID = $"Document {i}",
                        Name = $"Document {i}",
                        Processed = DateTime.Now,
                        Status = "Auto-update"
                    }));
                }
                await table.ExecuteBatchAsync(batch);
            }
            catch (Exception ex)
            {
                results.Passed = false;
                results.Code = ex.HResult;
                results.Message = $"Error: {ex.Message}";
            }
            return results;
        }


        public async Task<EvaluationResults> CheckTable(string tableName)
        {
            var results = new EvaluationResults();
            //TODO: Add logic for storage table check
            try
            {
                var client = account.CreateCloudTableClient();
                var table = client.GetTableReference(tableName);
                if (!await table.ExistsAsync())
                {
                    results.Passed = false;
                    results.Code = -1;
                    results.Message = $"Table {tableName} does not exist.";
                    return results;
                }
                var query = new TableQuery();
                var entities = await table.ExecuteQuerySegmentedAsync<DocumentEntity>(new TableQuery<DocumentEntity>(), new TableContinuationToken());
                foreach (var entity in entities)
                {
                    results.Data.Add(entity.AsDocumentData());
                }
                if (results.Data.Count > 0)
                {
                    results.Message = $"There are {results.Data.Count} rows in the {tableName} table.";
                }
                else
                {
                    results.Passed = false;
                    results.Code = -1;
                    results.Message = $"There were no errors, but there are no rows in the {tableName} table.";
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

    public class DocumentEntity : TableEntity
    {
        public DocumentEntity() : base() { this.PartitionKey = "blank"; }
        public DocumentEntity(string rowKey, string partitionKey) : base(partitionKey, rowKey) { }
        public DocumentEntity(DocumentData data)
        {
            this.ID = data.ID;
            this.Name = data.Name;
            this.Processed = data.Processed;
            this.Status = data.Status;
            this.PartitionKey = this.Name.Substring(0, 1);
        }
        public string ID
        {
            get => this.RowKey; set => this.RowKey = value;
        }
        public string Name { get; set; }
        public DateTime Processed { get; set; }
        public string Status { get; set; }

        public DocumentData AsDocumentData()
        {
            return new DocumentData
            {
                ID = this.ID,
                Name = this.Name,
                Processed = this.Processed,
                Status = this.Status
            };
        }
    }
}
