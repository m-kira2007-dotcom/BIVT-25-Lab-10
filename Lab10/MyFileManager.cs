using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab10
{
    public abstract class MyFileManager: IFileManager, IFileLifeController
    {
        private string _fileName;
        private string _fileExtension;
        private string _folderPath;

        public string Name { get; protected set; }  
        public string FolderPath => _folderPath;//папка
        public string FileName => _fileName;//имя файлф
        public string FileExtension => _fileExtension;//расширение для файла 

        // FullPath  возвращает полный путь к файлу
        public string FullPath=>Path.Combine(FolderPath, FileName) + "." + FileExtension;
        //путь к папке, имя файла и расширение файла



        // первый конструктор 
        public MyFileManager(string name)
        {
            Name = name;
            _fileName = "";
            _fileExtension = "";
            _folderPath = "";
        }

        //  второй конструктор(имя, название папки, название файла и необязательный параметр для расширения файла со значением “txt”)
        public MyFileManager(string name,string folderpath,string filename,string fileExtension="txt")
        {
            Name = name;
            _folderPath= folderpath;
            _fileExtension= fileExtension;
            _fileName = name;
        }

        //методы

        public void CreateFile()
        {
            //если ПАПКА не создана -создаем 
            if (!Directory.Exists(FolderPath))
            {
                Directory.CreateDirectory(FolderPath);
            }
            //в папке создаем ФАЙЛ
            if (!File.Exists(FullPath))
            {
                File.Create(FullPath).Close();//Closeчтобы освободить файл для дальнейших операций(без файл остаётся открытым,аналог using)

            }
        }
        public void DeleteFile()
        {
            if (File.Exists(FullPath))//удаляем файл по пути FullPath, если он имеется
            {
                File.Delete(FullPath);//удаление файла
            }
        }

        public virtual void EditFile(string file)//олжен заменять содержимое файла на переданное
        {
            if (File.Exists(FullPath))
            {
                File.WriteAllText(FullPath, file);//замена старого текста на новый 
            }

        }

        public virtual void ChangeFileExtension(string file)//меняем расширение файла с сохранением содержимого
        {
            if (File.Exists(FullPath))
            {
                string fileNew=Path.Combine(FolderPath, FileName)+ "."+ file;//склеивает папку и имя файла+ добавляет точку и новое расширение
                File.Move(FullPath, fileNew);//Перемещает/переименовывает его в новый путь
            }
            _fileExtension = file;//заменили расширение у метода
        }


        //для изменения рабочей папки 
        public void SelectFolder(string folder)
        {
            _folderPath = folder;
        }


        //для изменения имени файла (без расширения)
        public void ChangeFileName(string name)
        {
            _fileName = name;
        }

        public void ChangeFileFormat(string format)//меняет расширение файла
        {
            _fileExtension = format;//Обновление расширения в поле
            ChangeFileExtension(format);// Переименовывает файл(сохр содерж)
            if (!File.Exists(FullPath))
            {
                CreateFile();//Создаём пустой файл если его не сущ
            }
        }



    }
}
