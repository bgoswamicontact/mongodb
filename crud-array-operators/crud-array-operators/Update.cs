using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crud_array_operators
{
    class Update
    {
        static void Main(string[] args)
        {
            var client = new MongoClient();
            var database = client.GetDatabase("mongodb");
            database.DropCollection("inventory");
            var collection = database.GetCollection<BsonDocument>("inventory");
            var documents = new[]
            {
                new BsonDocument
                {
                    { "item", "canvas" },
                    { "qty", 100 },
                    { "size", new BsonDocument { { "h", 28 }, { "w", 35.5 }, { "uom", "cm" } } },
                    { "status", "A" }
                },
                new BsonDocument
                {
                    { "item", "journal" },
                    { "qty", 25 },
                    { "size", new BsonDocument { { "h", 14 }, { "w", 21 }, { "uom", "cm" } } },
                    { "status", "A" }
                },
                new BsonDocument
                {
                    { "item", "mat" },
                    { "qty", 85 },
                    { "size", new BsonDocument { { "h", 27.9 }, { "w", 35.5 }, { "uom", "cm" } } },
                    { "status", "A" }
                },
                new BsonDocument
                {
                    { "item", "mousepad" },
                    { "qty", 25 },
                    { "size", new BsonDocument { { "h", 19 }, { "w", 22.85 }, { "uom", "cm" } } },
                    { "status", "P" }
                },
                new BsonDocument
                {
                    { "item", "notebook" },
                    { "qty", 50 },
                    { "size", new BsonDocument { { "h", 8.5 }, { "w", 11 }, { "uom", "in" } } },
                    { "status", "P" } },
                new BsonDocument
                {
                    { "item", "paper" },
                    { "qty", 100 },
                    { "size", new BsonDocument { { "h", 8.5 }, { "w", 11 }, { "uom", "in" } } },
                    { "status", "D" }
                },
                new BsonDocument
                {
                    { "item", "planner" },
                    { "qty", 75 },
                    { "size", new BsonDocument { { "h", 22.85 }, { "w", 30 }, { "uom", "cm" } } },
                    { "status", "D" }
                },
                new BsonDocument
                {
                    { "item", "postcard" },
                    { "qty", 45 },
                    { "size", new BsonDocument { { "h", 10 }, { "w", 15.25 }, { "uom", "cm" } } },
                    { "status", "A" }
                },
                new BsonDocument
                {
                    { "item", "sketchbook" },
                    { "qty", 80 },
                    { "size", new BsonDocument { { "h", 14 }, { "w", 21 }, { "uom", "cm" } } },
                    { "status", "A" }
                },
                new BsonDocument
                {
                    { "item", "sketch pad" },
                    { "qty", 95 },
                    { "size", new BsonDocument { { "h", 22.85 }, { "w", 30.5 }, { "uom", "cm" } } }, { "status", "A" } },
            };
            collection.InsertMany(documents);

            //The update operation:

            //uses the $set operator to update the value of the size.uom field to "cm" and the value of the status field to "P",
            //uses the $currentDate operator to update the value of the lastModified field to the current date. If lastModified field does not exist, $currentDate will create the field

            var filter = Builders<BsonDocument>.Filter.Eq("item", "paper");
            //var update = Builders<BsonDocument>.Update.Set("size.uom", "cm").Set("status", "P").CurrentDate("lastModified");
            //var updateResult = collection.UpdateOne(filter, update);
            var replacement = new BsonDocument
            {
                { "item", "paper" },
                { "instock", new BsonArray
                    {
                        new BsonDocument { { "warehouse", "A" }, { "qty", 60 } },
                        new BsonDocument { { "warehouse", "B" }, { "qty", 40 } } }
                    }
            };

            var updateResult = collection.ReplaceOne(filter, replacement);

            var result = collection.Find(filter).ToList();
            foreach (var item in result)
            {
                Console.WriteLine(item.ToJson()); 
            }
            Console.ReadKey();

            /* IMPORTANT INFO
             1. All write operations in MongoDB are atomic on the level of a single document
             2. Once set, you cannot update the value of the _id field nor can you replace an existing document with a replacement document that has a different _id field value.
             3.When performing update operations that increase the document size beyond the allocated space for that document, the update operation relocates the document on disk.
             4. MongoDB preserves the order of the document fields following write operations except for the following cases:

                The _id field is always the first field in the document.
                Updates that include renaming of field names may result in the reordering of fields in the document.
             5. Upsert Option
             If UpdateOne(), UpdateMany(), or ReplaceOne() includes an UpdateOptions argument instance with the IsUpsert option set to true and no documents match the specified filter,
             then the operation creates a new document and inserts it. If there are matching documents, then the operation modifies or replaces the matching document or documents.
             
             */
        }
    }
}
