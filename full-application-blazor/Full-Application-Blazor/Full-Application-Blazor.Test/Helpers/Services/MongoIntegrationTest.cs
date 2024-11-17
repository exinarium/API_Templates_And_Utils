using Full_Application_Blazor.Test.MockData.Models;
using Full_Application_Blazor.Utils.Configuration;
using Microsoft.Extensions.Options;
using Mongo2Go;
using MongoDB.Driver;

namespace Full_Application_Blazor.Test.Helpers.Services
{
    public class MongoIntegrationTest
    {
        protected MongoDbRunner _runner;
        protected MongoClient _client;
        protected IOptions<Config> _options;
        protected IMongoDatabase _database;
        protected IMongoCollection<Student> _collection;
        protected string _databaseName = "Test";

        protected void CreateConnection()
        {
            _runner = MongoDbRunner.Start();
            _client = new MongoClient(_runner.ConnectionString);
            _options = Options.Create<Config>(new Config
            {
                DatabaseConfig = new DatabaseConfig
                {
                    ConnectionString = _runner.ConnectionString,
                    DatabaseName = _databaseName
                },
                EmailConfig = new EmailConfig()
            });

            _database = _client.GetDatabase(_databaseName);
            _collection = _database.GetCollection<Student>("student");
        }
    }
}
