using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab10.White
{
    public class White
    {
        private Lab9.White.White[] _tasks;
        private WhiteFileManager _manager;

        public WhiteFileManager Manager => _manager;
        public Lab9.White.White[] Tasks => _tasks;
        //первый конструктор(только задачи)
        public White(Lab9.White.White[] tasks = null)
        {
            _tasks = CopyOrEmpty(tasks);
            _manager = null;
        }
        //второйт конструктор(менеджер файлов и необязательный массив задач)
        public White(WhiteFileManager manager, Lab9.White.White[] tasks = null)
        {
            _manager = manager;
            _tasks = CopyOrEmpty(tasks);
        }
        //Третий конструктор(массив задач и менеджер файлов)
        public White(Lab9.White.White[] tasks, WhiteFileManager manager)
        {
            _tasks = CopyOrEmpty(tasks);
            _manager = manager;
        }
        
        public void Add(Lab9.White.White task)
        {
            if (task == null)
            {
                return;
            }

            var ob = new Lab9.White.White[_tasks.Length + 1];
            for (int i = 0; i < _tasks.Length; i++)
            {
                ob[i] = _tasks[i];
            }
            ob[_tasks.Length] = task;
            _tasks = ob;
        }

        //перегруженный метод Add для добавления массива объектов
        //с последовательным добавлением каждого элемента
        public void Add(Lab9.White.White[] tasks)
        {
            if (tasks == null)
            {
                return;
            }

            foreach (var t in tasks)
            {
                Add(t);
            }
        }

        public void Remove(Lab9.White.White task)
        {
            if (task == null || _tasks.Length == 0)
            {
                return;
            }

            int index = -1;
            for (int i = 0; i < _tasks.Length; i++)
            {
                if (ReferenceEquals(_tasks[i], task))
                {
                    index = i;
                    break;
                }
            }
            if (index < 0)
            {
                return;
            }

            var array = new Lab9.White.White[_tasks.Length - 1];
            // Копируем все элементы кроме удаляемого
            for (int i = 0, j = 0; i < _tasks.Length; i++)
            {
                if (i == index)
                {
                    continue;
                }
                array[j++] = _tasks[i];
            }
            _tasks = array;
        }

        public void Clear()
        {
            _tasks = new Lab9.White.White[0];
            // Удаляем папку менеджера если сущ

            if (_manager != null && !string.IsNullOrEmpty(_manager.FolderPath) && Directory.Exists(_manager.FolderPath))
            {
                Directory.Delete(_manager.FolderPath, true);
            }
        }

        public void SaveTasks()//последовательно сохраняет все задачи в файлы
        {
            if (_manager == null)
            {
                return;
            }

            for (int i = 0; i < _tasks.Length; i++)
            {
                _manager.ChangeFileName("task" + i);
                _manager.Serialize(_tasks[i]);
            }
        }
        //последовательно загружает задачи из файлов 

        public void LoadTasks()
        {
            if (_manager == null)
            {
                return;
            }

            for (int i = 0; i < _tasks.Length; i++)
            {
                _manager.ChangeFileName("task" + i);
                var loaded = _manager.Deserialize();
                _tasks[i] = loaded;
            }
        }

        public void ChangeManager(WhiteFileManager manager)
        {
            if (manager == null)
            {
                return;
            }
            //заменяет текущего менеджера на нового
            _manager = manager;

            //создаёт папку на основе имени менеджера и устанавливает её в качестве рабочей папки
            var folder = Path.Combine(Directory.GetCurrentDirectory(), manager.Name);
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            _manager.SelectFolder(folder);
        }


        //коп переданного массива или возвр пустой массив
        private static Lab9.White.White[] CopyOrEmpty(Lab9.White.White[] k )
        {
            if (k == null)
            {
                return new Lab9.White.White[0];
            }

            var copy = new Lab9.White.White[k.Length];
            for (int i = 0; i < k.Length; i++)
            {
                copy[i] = k[i];
            }
            return copy;
        }
    }
}
