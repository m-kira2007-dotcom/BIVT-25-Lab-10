using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab10.White
{
    public abstract class WhiteFileManager : MyFileManager, IWhiteSerializer
    {
        public WhiteFileManager(string name) : base(name)
        {
        }

        public WhiteFileManager(string name, string folderPath, string fileName, string fileExtension = "txt") : base(name, folderPath, fileName, fileExtension)
        {
        }

        public abstract void Serialize(Lab9.White.White obj);
        public abstract Lab9.White.White Deserialize();

        public override void EditFile(string content)
        {
            if (content == null)
            {
                return;
            }

            var path = FullPath;//получаем полный путь
            if (string.IsNullOrEmpty(path))//проверяем его
            {
                return;
            }

            if (!string.IsNullOrEmpty(FolderPath) && !Directory.Exists(FolderPath))
            {
                Directory.CreateDirectory(FolderPath);
            }

            File.WriteAllText(path, content);  // замен содержимое файла
        }
        //переопределяем 
        public override void ChangeFileExtension(string newExtension)
        {
            if (string.IsNullOrEmpty(newExtension))
            {
                return;
            }

            base.ChangeFileExtension(newExtension);//через родительский метод
        }
    }
}
