using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crud_array_operators
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new MongoClient();
            var database = client.GetDatabase("mongodb");
            database.DropCollection("inventory");
            var collection = database.GetCollection<BsonDocument>("inventory");

            SetUpData(collection);
            var data = SpecifyQueryConditionOnFieldInArrayOfDocuments(collection);
            foreach (var item in data)
            {
                //ALWAYS REMEMBER BSONDOCUMENT is nothing but a Dictionary
                Console.WriteLine(item["item"].ToString());

            }
            Console.ReadKey();
        }

        private static List<BsonDocument> SpecifyQueryConditionOnFieldInArrayOfDocuments(IMongoCollection<BsonDocument> collection)
        {
            //instock array has at least one embedded document that contains the field qty whose value is less than or equal to 20:
            //var filter = Builders<BsonDocument>.Filter.Lte("instock.qty", 20);

            //instock array has as its first element a document that contains the field qty whose value is less than or equal to 20
            //var filter = Builders<BsonDocument>.Filter.Lte("instock.0.qty", 20);

            //A Single Nested Document Meets Multiple Query Conditions on Nested Fields¶
            //instock array has at least one embedded document that contains both the field qty equal to 5 and the field warehouse equal to A:
            //var filter = Builders<BsonDocument>.Filter.ElemMatch<BsonValue>("instock", new BsonDocument { { "qty", 5 }, { "warehouse", "C" } });

            //instock array has at least one embedded document that contains the field qty that is greater than 10 and less than or equal to 20:
            var filter = Builders<BsonDocument>.Filter.ElemMatch<BsonValue>("instock", new BsonDocument { { "qty", new BsonDocument { { "$gt", 10 }, { "$lte", 20 } } } });
            var result = collection.Find(filter).ToList();
            return result;
        }

        private static List<BsonDocument> QueryforaDocumentNestedinanArray(IMongoCollection<BsonDocument> collection)
        {
            // an element in the instock array matches the specified document:
            var filter = Builders<BsonDocument>.Filter.AnyEq("instock", new BsonDocument { { "warehouse", "A" }, { "qty", 5 } });//This will match item: journal
//Equality matches on the whole embedded/nested document require an exact match of the specified document, including the field order. For example, the following query does not match any documents in the inventory collection:
// var filter = Builders<BsonDocument>.Filter.AnyEq("instock", new BsonDocument { { "qty", 5 }, { "warehouse", "A" } });
            var result = collection.Find(filter).ToList();
            return result;
        }

        private static void SetUpData(IMongoCollection<BsonDocument> collection)
        {
            //{ item: "journal", instock: [ { warehouse: "A", qty: 5 }, { warehouse: "C", qty: 15 } ] }
            var documents = new[]
             {
                new BsonDocument
                {
                    { "item", "journal" },
                    { "instock", new BsonArray
                        {
                            new BsonDocument { { "warehouse", "A" }, { "qty", 5 } },
                            new BsonDocument { { "warehouse", "C" }, { "qty", 15 } } }
                        }
                },
                new BsonDocument
                {
                    { "item", "notebook" },
                    { "instock", new BsonArray
                        {
                            new BsonDocument { { "warehouse", "C" }, { "qty", 5 } } }
                        }
                },
                new BsonDocument
                {
                    { "item", "paper" },
                    { "instock", new BsonArray
                        {
                            new BsonDocument { { "warehouse", "A" }, { "qty", 60 } },
                            new BsonDocument { { "warehouse", "B" }, { "qty", 15 } } }
                        }
                },
                new BsonDocument
                {
                    { "item", "planner" },
                    { "instock", new BsonArray
                        {
                            new BsonDocument { { "warehouse", "A" }, { "qty", 40 } },
                            new BsonDocument { { "warehouse", "B" }, { "qty", 5 } } }
                        }
                },
                new BsonDocument
                {
                    { "item", "postcard" },
                    { "instock", new BsonArray
                        {
                            new BsonDocument { { "warehouse", "B" }, { "qty", 15 } },
                            new BsonDocument { { "warehouse", "C" }, { "qty", 35 } } }
                        }
                }
            };
            collection.InsertMany(documents);
        }

    }
}
