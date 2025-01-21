using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb3.Model;

internal class MongoDb
{
    private readonly IMongoDatabase _database;

    public MongoDb(string connectionString, string databaseString)
    {


        var client = new MongoClient(connectionString);
        _database = client.GetDatabase(databaseString);

        Categories = _database.GetCollection<Category>("Categories");
        QuestionPacks = _database.GetCollection<QuestionPack>("QuestionPacks");

        InizializeDatabase();

    }

    public IMongoCollection<QuestionPack> QuestionPacks { get; set; }
    public IMongoCollection<Category> Categories { get; set; }

    public void InizializeDatabase()
    {
        if (!Categories.AsQueryable().Any())
        {
            Categories.InsertMany(
            [
                new Category("History"),
                new Category("Programming"),
                new Category("Other"),
                new Category("Animals")
            ]);
        }
        
        if (!QuestionPacks.AsQueryable().Any())
        {
            QuestionPacks.InsertOne(new QuestionPack
            {
                Name = "Default Pack",
                Difficulty = Difficulty.Medium,
                TimeLimitInSeconds = 20,
                Category = Categories.Find(c => c.Name == "Animals").First(),
                Questions = new List<Question> {
                    new Question
                    {
                        Query = "What type of breed is Kerstin?",
                        CorrectAnswer = "Griffon",
                        IncorrectAnswers = [ "Gremlin", "Rat", "Wolf" ]
                    },
                    new Question
                    {
                        Query = "What type of animal is connected to IH-Högskolan?",
                        CorrectAnswer = "Ducks",
                        IncorrectAnswers= [ "Elephants", "Whales", "Aligators" ]

                    }}
            });
        }
    }
}
