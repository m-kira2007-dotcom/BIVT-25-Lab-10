using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab9.White
{
    public class Task2 : White//сколько слов в тексте содержит один слог, два слога, три слога и т.д. Слова, не
                    //содержащие гласных букв считать односложными.
    {
        private int[,] _output;// матрица [слоги, количество слов]

        public Task2(string text) : base(text) 
        {

        }

        public override void Review()
        {
            _output = syllablesMatrix(Input);
        }

        public int[,] Output//если пустой то возвращ пустую матрицу
        {
            get
            {
                if (_output != null)
                    return _output;
                else
                    return new int[0, 2];
            }
        }

        public static int[,] syllablesMatrix(string text)
        {
            var currentWord = new StringBuilder();//подсчитываем кол во слов
            bool wordStartedAfterDigit = false;//слова сразу после циф не учитываем 
            bool lastWasDigit = false;

            int wordCount = 0;
            foreach (char clov in text)// считаем слова и создаем массив нужного размера
            {
                if (char.IsLetter(clov) || clov == '-' || clov == '\'')//слова и все что вних входит
                {
                    if (currentWord.Length == 0)
                        wordStartedAfterDigit = lastWasDigit;
                    currentWord.Append(clov);
                    lastWasDigit = false;
                }
                else
                {
                    if (currentWord.Length > 0)//пробелы знаки 
                    {
                        if (!wordStartedAfterDigit)
                            wordCount++;
                        currentWord.Clear();
                    }
                    lastWasDigit = char.IsDigit(clov);
                }
            }
            if (currentWord.Length > 0 && !wordStartedAfterDigit)
                wordCount++;

            string[] words = new string[wordCount];//собираем в массив 
            currentWord.Clear();
            wordStartedAfterDigit = false;
            lastWasDigit = false;
            int k= 0;

            foreach (char c in text)
            {
                if (char.IsLetter(c) || c == '-' || c == '\'')
                {
                    if (currentWord.Length == 0)
                        wordStartedAfterDigit = lastWasDigit;
                    currentWord.Append(c);
                    lastWasDigit = false;
                }
                else
                {
                    if (currentWord.Length > 0)
                    {
                        if (!wordStartedAfterDigit)
                            words[k++] = currentWord.ToString();
                        currentWord.Clear();
                    }
                    lastWasDigit = char.IsDigit(c);
                }
            }
            if (currentWord.Length > 0 && !wordStartedAfterDigit)
                words[k] = currentWord.ToString();

            char[] vowels = { 'а','е','ё','и','о','у','ы','э','ю','я',
            'А','Е','Ё','И','О','У','Ы','Э','Ю','Я',
            'a','e','i','o','u','y','A','E','I','O','U','Y' };

            int[] syllablesWord = new int[words.Length];
            int maxSyllables = 0;

            for (int i = 0; i < words.Length; i++)
            {
                int syllables = 0;

                foreach (char c in words[i])
                {
                    if (Array.IndexOf(vowels, c) >= 0)
                        syllables++;
                }

                if (syllables == 0)
                    syllables = 1;

                syllablesWord[i] = syllables;

                if (syllables > maxSyllables)
                    maxSyllables = syllables;
            }

            int[,] matrix = new int[maxSyllables, 2];

            for (int i = 0; i < words.Length; i++)
            {
                int slog = syllablesWord[i];
                if (slog > 0)
                {
                    matrix[slog - 1, 0] = slog;//кол во слогов
                    matrix[slog - 1, 1]++;//считаеи слова
                }
            }

            int filledCount = 0;//считаем непустые строки
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                if (matrix[i, 1] > 0)
                    filledCount++;
            }

            int[,] result = new int[filledCount, 2];
            int resultIndex = 0;
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                if (matrix[i, 1] > 0)
                {
                    result[resultIndex, 0] = matrix[i, 0];
                    result[resultIndex, 1] = matrix[i, 1];
                    resultIndex++;
                }
            }

            return result;
        }

        public override string ToString()
        { 
            var matrix = Output;
            if (matrix == null) return string.Empty;

            var result = new System.Text.StringBuilder();

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                result.Append(matrix[i, 0]);//слоги
                result.Append(':');//разделителт
                result.Append(matrix[i, 1]);//колво
                result.AppendLine();//переносы
            }

            return result.ToString().TrimEnd();//избавляемся от последнего переноса
        }
    }
}
