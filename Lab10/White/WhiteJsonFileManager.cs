using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Lab10.White
{
    public class WhiteJsonFileManager : WhiteFileManager
    {
        public WhiteJsonFileManager(string name) : base(name)
        {
        }

        public WhiteJsonFileManager(string name, string folderPath, string fileName, string fileExtension = "txt")
 : base(name, folderPath, fileName, fileExtension)
        {
        }

        public override void Serialize(Lab9.White.White obj)
        {
            if (obj == null)
            {
                return;
            }

            var path = FullPath;
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            if (!string.IsNullOrEmpty(FolderPath) && !Directory.Exists(FolderPath))
            {
                Directory.CreateDirectory(FolderPath);
            }

            var dict = new Dictionary<string, object?>
            {
                ["Type"] = obj.GetType().Name,
                ["Input"] = obj.Input
            };

            if (obj is Lab9.White.Task3)
            {
                var codes = ExtractCodes(obj);
                if (codes != null)
                {
                    int rows = codes.GetLength(0);
                    var arr = new string[rows][];
                    for (int i = 0; i < rows; i++)
                    {
                        arr[i] = new[] { codes[i, 0], codes[i, 1] };
                    }
                    dict["Codes"] = arr;
                }
            }

            var json = JsonSerializer.Serialize(dict, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(path, json);
        }

        public override Lab9.White.White Deserialize()
        {
            var path = FullPath;
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
            {
                return null!;
            }

            var text = File.ReadAllText(path);
            if (string.IsNullOrEmpty(text))
            {
                return null!;
            }

            JsonElement root;
            try
            {
                root = JsonSerializer.Deserialize<JsonElement>(text);
            }
            catch
            {
                return null!;
            }

            if (root.ValueKind != JsonValueKind.Object)
            {
                return null!;
            }

            string type = root.TryGetProperty("Type", out var t) ? (t.GetString() ?? "") : "";
            string input = root.TryGetProperty("Input", out var inp) ? (inp.GetString() ?? "") : "";

            Lab9.White.White result;

            switch (type)
            {
                case "Task1":
                    result = new Lab9.White.Task1(input);
                    break;
                case "Task2":
                    result = new Lab9.White.Task2(input);
                    break;
                case "Task3":
                    string[,] codes = new string[0, 2];
                    if (root.TryGetProperty("Codes", out var c) && c.ValueKind == JsonValueKind.Array)
                    {
                        int len = c.GetArrayLength();
                        codes = new string[len, 2];
                        int i = 0;
                        foreach (var item in c.EnumerateArray())
                        {
                            if (item.ValueKind == JsonValueKind.Array && item.GetArrayLength() >= 2)
                            {
                                codes[i, 0] = item[0].GetString() ?? string.Empty;
                                codes[i, 1] = item[1].GetString() ?? string.Empty;
                            }
                            i++;
                        }
                    }
                    result = new Lab9.White.Task3(input, codes);
                    break;
                case "Task4":
                    result = new Lab9.White.Task4(input);
                    break;
                default:
                    return null!;
            }

            result.Review();
            return result;
        }

        public override void EditFile(string content)
        {
            if (content == null)
            {
                return;
            }

            var obj = Deserialize();
            if (obj == null)
            {
                return;
            }

            obj.ChangeText(content);
            Serialize(obj);
        }

        public override void ChangeFileExtension(string newExtension)
        {
            if (newExtension != "json")
            {
                return;
            }
            ChangeFileFormat("json");
        }

        private static string[,]? ExtractCodes(Lab9.White.White obj)
        {
            var field = typeof(Lab9.White.Task3).GetField("_codes", BindingFlags.Instance | BindingFlags.NonPublic);
            if (field == null)
            {
                return null;
            }
            return field.GetValue(obj) as string[,];
        }
    }
}

