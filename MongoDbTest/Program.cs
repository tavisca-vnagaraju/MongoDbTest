using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

//MongoDB.Driver  
using MongoDB.Bson;
using MongoDB.Driver;

namespace AppTest
{
    public class Students
    {
        public ObjectId Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public string Age { get; set; }
    }


    public class Program
    {
        protected static IMongoClient _client;
        protected static IMongoDatabase _database;

        public void InsertOneProduct()
        {
            _client = new MongoClient();
            _database = _client.GetDatabase("txs");
            var _collection = _database.GetCollection<Object>("product");
            _collection.InsertOne(new
            {
                id = 1,
                name = "product1",
                price = 3517,
                status = "active",
            });
            Console.WriteLine("Inserted One");

        }
        public void InsertManyProducts()
        {
            _client = new MongoClient();
            _database = _client.GetDatabase("txs");
            var _collection = _database.GetCollection<Object>("product");
            for (int i = 1; i < 200000; i++)
            {
                _collection.InsertOne(new
                {
                    id = i,
                    name = "product" + i,
                    price = i * 2 + 8,
                    status = "active",
                });
            }
            Console.WriteLine("Inserted");
        }
        public void GetProductByName(string name)
        {
            Stopwatch stopWatch = new Stopwatch();

            _client = new MongoClient();
            _database = _client.GetDatabase("txs");
            var _collection = _database.GetCollection<Object>("product");
            
            var filter = Builders<Object>.Filter.Eq("name", name);
            var tasks = new Task[]
            {

                Task.Factory.StartNew(() => Find(_collection, filter)),
                Task.Factory.StartNew(() => Find(_collection, filter)),
                Task.Factory.StartNew(() => Find(_collection, filter)),
                Task.Factory.StartNew(() => Find(_collection, filter)),
                Task.Factory.StartNew(() => Find(_collection, filter))
            };
            stopWatch.Start();
            Task.WaitAll(tasks);
            stopWatch.Stop();
            Console.WriteLine("MongoDb Filter By Name- Time:" + stopWatch.ElapsedMilliseconds);

            //foreach (var item in all.ToEnumerable())
            //{
            //    var json = item.ToJson();
            //    Console.WriteLine(json);
            //}
            

        }
    public static void Find(IMongoCollection<Object> collection, FilterDefinition<Object> filter)
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        collection.Find(filter);
        stopwatch.Stop();
        Console.WriteLine("Stop MongoDB Indivudual Time Ticks: " + stopwatch.ElapsedMilliseconds);
        stopwatch.Reset();

    }
    public static void Main(string[] args)
        {
            Program p = new Program();
         //   p.InsertManyProducts();
            
            //p.CRUDwithMongoDb();
            p.GetProductByName("product10000");


            //Hold the screen by logic  
            Console.WriteLine("Press any key to trminated the program");
            Console.ReadKey();
        }
    }
}