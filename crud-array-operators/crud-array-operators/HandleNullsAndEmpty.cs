using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crud_array_operators
{
    class HandleNullsAndEmpty
    {
        //static void Main(string[] args)
        //{
        //    var client = new MongoClient();
        //    var database = client.GetDatabase("mongodb");
        //    database.DropCollection("inventory");
        //    var collection = database.GetCollection<BsonDocument>("inventory");

        //    SetUpData(collection);
        //    //This will return both , value is null and does not exist
        //    //var filter = Builders<BsonDocument>.Filter.Eq("item", BsonNull.Value);

        //    //matches only documents that contain the item field whose value is null; i.e. the value of the item field is of BSON Type Null (type number 10) :
        //    //var filter = Builders<BsonDocument>.Filter.Type("item", BsonType.Null);

        //    var filter = Builders<BsonDocument>.Filter.Exists("item", false);
        //    var result = collection.Find(filter).ToList();
        //    foreach (var item in result)
        //    {
        //        //ALWAYS REMEMBER BSONDOCUMENT is nothing but a Dictionary
        //        Console.WriteLine(item["_id"].ToString());

        //    }
        //    Console.ReadKey();
        //}

        private static void SetUpData(IMongoCollection<BsonDocument> collection)
        {
            var documents = new[]
            {
                new BsonDocument { { "_id", 1 }, { "item", BsonNull.Value } },
                new BsonDocument { { "_id", 2 } }
            };
            collection.InsertMany(documents);
        }
    }
}
