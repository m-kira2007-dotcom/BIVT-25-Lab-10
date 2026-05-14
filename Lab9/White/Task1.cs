using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab9.White
{
    public class Task1:White
    {
        private char[] _punctuationMarks = { '.', '!', '?', ',', ':', '"', ';', '–', '\'', '(', ')', '[', ']', '{', '}', '/' };
       

        private char[] _sentenceEnders = { '.', '!', '?' };

        public double Output
        {
            get
            {
                if (Input == null || Input == "")
                    return 0;

                return AverageComplexity();
            }
        }

        public Task1(string text) : base(text)
        {

        }

        private double AverageComplexity()//символы / колво предложений
        {
            string[] sentences = SplitBySentence(Input);// Разбиваем текст на предложения

            if (sentences.Length == 0)
                return 0;

            double totalComplexity = 0;// сумм сложность всех предложений
            int realSentenceCount = 0;//кол во непустых предложений 

            for (int i = 0; i < sentences.Length; i++)
            {
                string trimmed = sentences[i].Trim();// удаление всех пробельных символов в начале и в конце строки

                // проверка не пустая ли строка и не состоит ли только из пробелов
                if (trimmed == null || trimmed == "")
                    continue;

                int wordCount = CountWords(trimmed);
                int punctuationCount = CountPunctuation(trimmed);

                totalComplexity += wordCount + punctuationCount;
                realSentenceCount++;
            }

            if (realSentenceCount == 0)
                return 0;
            else
                return totalComplexity / realSentenceCount;
        }

        private string[] SplitBySentence(string text)
        {
            int count = 0; //считаем количество предложений
            var current = new StringBuilder();

            for (int i = 0; i < text.Length; i++)
            {
                current.Append(text[i]);
                if (Array.IndexOf(_sentenceEnders, text[i]) >= 0)// если знак конца предложения
                {
                    bool isLast = i == text.Length - 1;
                    bool nextIsSpace = !isLast && char.IsWhiteSpace(text[i + 1]);
                    if (isLast || nextIsSpace)
                    {
                        count++;
                        current.Clear();
                    }
                }
            }
            if (current.Length > 0)
                count++;

            string[] sentences = new string[count]; //заполняем массив предложений
            current.Clear();
            int idx = 0;

            for (int i = 0; i < text.Length; i++)
            {
                current.Append(text[i]);
                if (Array.IndexOf(_sentenceEnders, text[i]) >= 0)
                {
                    bool isLast = i == text.Length - 1;
                    bool nextIsSpace = !isLast && char.IsWhiteSpace(text[i + 1]);
                    if (isLast || nextIsSpace)
                    {
                        sentences[idx++] = current.ToString();//превращаем в строку 
                        current.Clear();
                    }
                }
            }
            if (current.Length > 0)
                sentences[idx] = current.ToString();

            return sentences;
        }

        private int CountWords(string text)
        {
            var cleaned = new StringBuilder();

            foreach (var k in text)// Убираем все знаки препинания
            {
                if (Array.IndexOf(_punctuationMarks, k) < 0)//если символ не знак препнания 
                    cleaned.Append(k);//добавляем 
            }
            // Разбиваем по пробелам и считаем
            var words = cleaned.ToString().Replace("-", " ").Split(
                new[] { ' ', '\t', '\n', '\r' },
                StringSplitOptions.RemoveEmptyEntries
            );

            return words.Length;
        }

        private int CountPunctuation(string text)//подсчет знаков препинания
        {
            int count = 0;

            foreach (var c in text)
            {
                if (Array.IndexOf(_punctuationMarks, c) >= 0)
                    count++;
            }

            return count;
        }

        public override void Review()
        {
        }

        public override string ToString()
        {
            return Output.ToString();
        }
    }
}

