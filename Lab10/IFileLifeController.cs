using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab10
{
    public interface IFileLifeController
    {
        void CreateFile();
        void DeleteFile();

        // EditFile/ChangeFileExtension принимают строковое поле

        void EditFile(string file);

        void ChangeFileExtension(string file);

    }
}
