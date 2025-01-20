using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb3.Model;

internal class Category
{
    public Category(string name) 
    {
        Name = name;
    }

    [BsonElement("_id")] [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonElement("categoryname")] [BsonRepresentation(BsonType.String)]
    public string Name { get; set; }
}
