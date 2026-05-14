using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab10
{
    public interface IFileManager
    {
        //строковых свойств FolderPath, FileName, FileExtension,FullPath
        string FolderPath {  get; } 
        string FileName { get; }
        string FileExtension { get; }
        string FullPath { get; }


        //методов SelectFolder, ChangeFileName, ChangeFileFormat строk поле
         
        void SelectFolder(string folder);

        void ChangeFileName(string name);
        void ChangeFileFormat(string format);   






    }
}
