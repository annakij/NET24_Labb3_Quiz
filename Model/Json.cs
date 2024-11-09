using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Labb3.Model
{
    class Json
    {
        private readonly string dataPath;
        private readonly string dataDirectory;
        private readonly JsonSerializerOptions options;

        public Json()
        {

            dataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            dataDirectory = Path.Combine(dataPath, "Labb3QuizConfigurator");
            

            options = new JsonSerializerOptions
            {
                IncludeFields = true,
                PropertyNameCaseInsensitive = true
            };

            if (!Directory.Exists(dataDirectory))
            {
                Directory.CreateDirectory(dataDirectory);
            }
        }

        public async Task SaveQuestionPacks(List<QuestionPack> packs)
        {
            string filePath = Path.Combine(dataDirectory, "QuestionPacks.json");
            string json = JsonSerializer.Serialize(packs, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(filePath, json);
        }

        public async Task<List<QuestionPack>> LoadQuestionPacks()
        {
            string filePath = Path.Combine(dataDirectory, "QuestionPacks.json");

            if (!File.Exists(filePath))
            {
                var startPack = new QuestionPack("Default QuestionPack", Difficulty.Medium, 15);
                await SaveQuestionPacks(new List<QuestionPack> { startPack });
            }

            try
            {
                string json = await File.ReadAllTextAsync(filePath);

                List<QuestionPack> packs = JsonSerializer.Deserialize<List<QuestionPack>>(json, options) ?? new List<QuestionPack>();

                return packs;
            }
            catch (JsonException)
            {
                return new List<QuestionPack>();
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Error reading the question packs: {ex.Message}");
                return new List<QuestionPack>();
            }
        }

    }
}
