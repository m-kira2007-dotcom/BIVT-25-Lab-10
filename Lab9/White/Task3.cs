using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab9.White
{
    public class Task3: White
    {
        private string[,] _codes;// таблица кодов [слово, код]
        private string _output;

        public Task3(string text, string[,] codes) : base(text)//принимает текст и таблтцу с кодами
        {
            _codes = codes;
        }

        public string Output
        {
            get
            {
                if (Input == null || Input == "")
                    return Input;

                return _output;
            }
        }

        public override void Review()
        {
            _output = ReplaceWordsWithCodes(Input);
        }

        private string ReplaceWordsWithCodes(string text)
        {
            var result = new StringBuilder();//сщбираем результат
            var currentWord = new StringBuilder();//собираем слово в дан момент

            for (int i = 0; i < text.Length; i++)
            {
                char sl = text[i];

                if (char.IsLetter(sl) || sl== '\'' || sl == '-')//если это часть слова(буквы,дефис,апост)
                {
                    currentWord.Append(sl);//добавляем в слова
                }
                else
                {
                    if (currentWord.Length > 0)//если слово собралось надо заменить
                    {
                        string word = currentWord.ToString();
                        string code = FindCode(word);//ищем код в табл
                        result.Append(code);//добавляем код
                        currentWord.Clear();//очищаем для след слова
                    }
                    result.Append(sl);//добав знак препинания или пробел 
                }
            }
            
            if (currentWord.Length > 0)//делаем все то же для последнего слова
            {
                string word = currentWord.ToString();
                string code = FindCode(word);
                result.Append(code);
            }

            return result.ToString();
        }

        private string FindCode(string word)
        {
            for (int i = 0; i < _codes.GetLength(0); i++)//проходимся по строкам в табл
            {
                if (string.Equals(_codes[i, 0], word, StringComparison.Ordinal))
                {
                    return _codes[i, 1];
                }
            }
            return word;//если не нашли возращаем слово
        }

        public override string ToString()
        {
            return Output;
        }
    }

}

