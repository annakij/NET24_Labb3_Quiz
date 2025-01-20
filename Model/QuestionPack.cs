using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb3.Model;

enum Difficulty { Easy, Medium, Hard }

internal class QuestionPack
{
    public QuestionPack(string name = "Questionpack", Difficulty difficulty = Difficulty.Medium, int timeLimitInSeconds = 30, Category category = null)
    {
        Name = name;
        Difficulty = difficulty;
        TimeLimitInSeconds = timeLimitInSeconds;
        Questions = new List<Question>();
        Category = category;
    }

    [BsonElement("_id")] [BsonRepresentation(BsonType.ObjectId)]
    public string _id {  get; set; }

    [BsonElement("name")]
    public string Name { get; set; }

    [BsonElement("difficulty")]
    public Difficulty Difficulty { get; set; }

    [BsonElement("timelimit")]
    public int TimeLimitInSeconds { get; set; }

    [BsonElement("questions")]
    public List<Question> Questions { get; set; }

    [BsonElement("category")]
    public Category Category { get; set; }

}
