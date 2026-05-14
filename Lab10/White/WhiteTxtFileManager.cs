using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Lab10.White
{
    public class WhiteTxtFileManager : WhiteFileManager
    {
        public WhiteTxtFileManager(string name) : base(name)
        {
        }

        public WhiteTxtFileManager(string name, string folderPath, string fileName, string fileExtension = "txt"): base(name, folderPath, fileName, fileExtension)
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

            var sb = new StringBuilder();
            sb.AppendLine("Type=" + obj.GetType().Name);
            sb.AppendLine("Input=" + EscapeValue(obj.Input ?? string.Empty));

            if (obj is Lab9.White.Task3)
            {
                var codes = ExtractCodes(obj);
                if (codes != null)
                {
                    int rows = codes.GetLength(0);
                    sb.AppendLine("CodesCount=" + rows);
                    for (int i = 0; i < rows; i++)
                    {
                        sb.AppendLine("Code" + i + "Key=" + EscapeValue(codes[i, 0] ?? string.Empty));
                        sb.AppendLine("Code" + i + "Value=" + EscapeValue(codes[i, 1] ?? string.Empty));
                    }
                }
            }

            File.WriteAllText(path, sb.ToString());
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

            var dict = new Dictionary<string, string>();
            using (var reader = new StringReader(text))
            {
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    int eq = line.IndexOf('=');
                    if (eq < 0)
                    {
                        return null!;
                    }
                    var key = line.Substring(0, eq);
                    var val = UnescapeValue(line.Substring(eq + 1));
                    dict[key] = val;
                }
            }

            if (!dict.TryGetValue("Type", out var type))
            {
                return null!;
            }
            string input = dict.TryGetValue("Input", out var inp) ? inp : string.Empty;

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
                    int count = 0;
                    if (dict.TryGetValue("CodesCount", out var cc))
                    {
                        int.TryParse(cc, out count);
                    }

                    var codes = new string[count, 2];
                    for (int i = 0; i < count; i++)
                    {
                        codes[i, 0] = dict.TryGetValue("Code" + i + "Key", out var k) ? k : string.Empty;
                        codes[i, 1] = dict.TryGetValue("Code" + i + "Value", out var v) ? v : string.Empty;
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
            if (newExtension != "txt")
            {
                return;
            }
            ChangeFileFormat("txt");
        }

        private static string EscapeValue(string s)
        {
            return s.Replace("\\", "\\\\").Replace("\r", "\\r").Replace("\n", "\\n");
        }

        private static string UnescapeValue(string s)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == '\\' && i + 1 < s.Length)
                {
                    char n = s[i + 1];
                    if (n == '\\')
                    {
                        sb.Append('\\');
                    }
                    else if (n == 'r')
                    {
                        sb.Append('\r');
                    }
                    else if (n == 'n')
                    {
                        sb.Append('\n');
                    }
                    else
                    {
                        sb.Append(s[i]);
                        sb.Append(n);
                    }
                    i++;
                }
                else
                {
                    sb.Append(s[i]);
                }
            }
            return sb.ToString();
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
