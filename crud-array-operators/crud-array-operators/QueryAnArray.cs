using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crud_array_operators
{
    class QueryAnArray
    {
        //static void Main(string[] args)
        //{
        //    var client = new MongoClient();
        //    var database = client.GetDatabase("mongodb");
        //    database.DropCollection("arrays");
        //    var collection = database.GetCollection<BsonDocument>("arrays");

        //    SetUpData(collection);
        //    var data = QueryforanArrayElementthatMeetsMultipleCriteria(collection);
        //    foreach (var item in data)
        //    {
        //        //ALWAYS REMEMBER BSONDOCUMENT is nothing but a Dictionary
        //        Console.WriteLine(item["item"].ToString());

        //    }
        //    Console.ReadKey();
        //}


        private static void SetUpData(IMongoCollection<BsonDocument> collection)
        {
            var documents = new[]
             {
                new BsonDocument
                {
                    { "item", "journal" },
                    { "qty", 25 },
                    { "tags", new BsonArray { "blank", "red" } },
                    { "dim_cm", new BsonArray { 14, 21 } }
                },
                new BsonDocument
                {
                    { "item", "notebook" },
                    { "qty", 50 },
                    { "tags", new BsonArray { "red", "blank" } },
                    { "dim_cm", new BsonArray { 14, 21 } }
                },
                new BsonDocument
                {
                    { "item", "paper" },
                    { "qty", 100 },
                    { "tags", new BsonArray { "red", "blank", "plain" } },
                    { "dim_cm", new BsonArray { 14, 21 } }
                },
                new BsonDocument
                {
                    { "item", "planner" },
                    { "qty", 75 },
                    { "tags", new BsonArray { "blank", "red" } },
                    { "dim_cm", new BsonArray { 22.85, 30 } }
                },
                new BsonDocument
                {
                    { "item", "postcard" },
                    { "qty", 45 },
                    { "tags", new BsonArray { "blue" } },
                    { "dim_cm", new BsonArray { 10, 15.25 } }
                }
            };
            collection.InsertMany(documents);
        }

        private static void Match_An_Array(IMongoCollection<BsonDocument> collection)
        {
            //Match an Array
            /*The following example queries for all documents where the field tags value is an array with exactly two elements, "red" and "blank", in the specified order:*/
            var filter = Builders<BsonDocument>.Filter.Eq("tags", new[] { "red", "blank" });
            // var data = collection.Find(filter).ToList();
            var cursor = collection.Find(filter).ToCursor();
            while (cursor.MoveNext())
            {
                foreach (var item in cursor.Current)
                {
                    Console.WriteLine(item);
                }
            }
        }
        private static List<BsonDocument> Query_an_Array_for_an_Element(IMongoCollection<BsonDocument> collection)
        {
            //queries for all documents where tags is an array that contains the string "red" as one of its elements:
            //var filter = Builders<BsonDocument>.Filter.Eq("tags", "red");

            // the following operation queries for all documents where the array dim_cm contains at least one element whose value is greater than 25.
            //db.inventory.find( { dim_cm: { $gt: 25 } } )
            var filter = Builders<BsonDocument>.Filter.Gt("dim_cm", 25);
            var data = collection.Find(filter).ToList();
            return data;
        }
        private static List<BsonDocument> Specify_Multiple_Conditions_for_Array_Elements(IMongoCollection<BsonDocument> collection)
        {
            //queries for documents where the dim_cm array contains elements that in some combination satisfy the query conditions; 
            //e.g., one element can satisfy the greater than 15 condition and another element can satisfy the less than 20 condition, or a single element can satisfy both
            var builder = Builders<BsonDocument>.Filter;
            //db.inventory.find( { dim_cm: { $gt: 15, $lt: 20 } } )
            var filter = builder.And(builder.Gt("dim_cm", 15) & builder.Lt("dim_cm", 20));
            var data = collection.Find(filter).ToList();
            return data;
        }
        private static List<BsonDocument> QueryforanArrayElementthatMeetsMultipleCriteria(IMongoCollection<BsonDocument> collection)
        {
            var builder = Builders<BsonDocument>.Filter;
            //db.inventory.find( { dim_cm: { $elemMatch: { $gt: 22, $lt: 30 } } } )
            var filter = builder.ElemMatch<BsonValue>("dim_cm", new BsonDocument { { "$gt", 15 }, { "$lt", 20 } });
            var data = collection.Find(filter).ToList();
            return data;
        }
        private static List<BsonDocument> QueryforanElementbytheArrayIndexPosition(IMongoCollection<BsonDocument> collection)
        {
            var builder = Builders<BsonDocument>.Filter;
            // where the second element in the array dim_cm is greater than 25:
            //.find({"dim_cm.1", {$gt : 25}})
            //var filter = builder.Gt("dim_cm.1", 25);

            //Query an Array by Array Length
            //selects documents where the array tags has 3 elements.
            //.find("tags", {$size : 3})
            var filter = builder.Size("tags", 3);
            var data = collection.Find(filter).ToList();
            return data;
        }
    }
}
