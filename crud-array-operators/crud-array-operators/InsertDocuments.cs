using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crud_array_operators
{
    class InsertDocuments
    {
        static void Main1(string[] args)
        {
            var client = new MongoClient();
            var database = client.GetDatabase("mongodb");
            database.DropCollection("inventory");
            var collection = database.GetCollection<BsonDocument>("inventory");
            // { item: "canvas", qty: 100, tags: ["cotton"], size: { h: 28, w: 35.5, uom: "cm" } }
            var document = new BsonDocument()
            {
                { "item", "canvas"},
                { "qty" , 100},
                { "tags" , new BsonArray{ "cotton"} },
                { "size" , new BsonDocument{
                    { "h", 28},
                    { "w", 35.5},
                    { "uom", "cm"}

                } }
            };
            collection.InsertOne(document);

            var documents = new BsonDocument[]
            {
                new BsonDocument
                {
                    { "item", "journal" },
                    { "qty", 25 },
                    { "tags", new BsonArray { "blank", "red" } },
                    { "size", new BsonDocument { { "h", 14 }, { "w", 21 }, {  "uom", "cm"} } }
                },
                new BsonDocument
                {
                    { "item", "mat" },
                    { "qty", 85 },
                    { "tags", new BsonArray { "gray" } },
                    { "size", new BsonDocument { { "h", 27.9 }, { "w", 35.5 }, {  "uom", "cm"} } }
                },
                new BsonDocument
                {
                    { "item", "mousepad" },
                    { "qty", 25 },
                    { "tags", new BsonArray { "gel", "blue" } },
                    { "size", new BsonDocument { { "h", 19 }, { "w", 22.85 }, {  "uom", "cm"} } }
                },
            };
            collection.InsertMany(documents, new InsertManyOptions { IsOrdered = false}); // With unordered inserts, if an error occurs during an insert of one of the documents, MongoDB continues to insert the remaining documents in the array.

        }
    }
}
