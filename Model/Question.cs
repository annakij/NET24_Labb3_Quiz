using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb3.Model
{
    internal class Question
    {

        public Question()
        {
            IncorrectAnswers = new string[3];
        }

        public Question(string query, string correctAnswer, string incorrectAnswer1, string incorrectAnswer2, string incorrectAnswer3)
        {
            Query = query;
            CorrectAnswer = correctAnswer;
            IncorrectAnswers = new string[3] { incorrectAnswer1, incorrectAnswer2, incorrectAnswer3 };
        }

        [BsonElement("query")]
        public string Query { get; set; }

        [BsonElement("correctanswer")]
        public string CorrectAnswer { get; set; }

        [BsonElement("incorrectanswers")]
        public string[] IncorrectAnswers { get; set; }


    }
}
