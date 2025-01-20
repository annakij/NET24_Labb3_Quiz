﻿using System;
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

    public string Name { get; set; }
    public Difficulty Difficulty { get; set; }
    public int TimeLimitInSeconds { get; set; }
    public List<Question> Questions { get; set; }
    public Category Category { get; set; }

}
