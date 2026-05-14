using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab9.White
{
    public class Task4 : White//Найти сумму всех цифр (не чисел!)
    {
        private int _output;

        public Task4(string text) : base(text)
        {
        }

        public int Output
        {
            get
            {
                return _output;
            }
        }

        public override void Review()
        {
            _output = CalculateSumOfDigits(Input);//методом считаем сумму всех цивр которые есть в тексте(для подсчета и сохранения результ )
        }

        private int CalculateSumOfDigits(string text)
        {
            int sum = 0;

            foreach (char k in text)//(k >= '0' && k <= '9')
            {
                if (char.IsDigit(k))//проверяем является ли метод цифрой
                {
                    sum += k- '0';//превращаем символ циф в число(чтобы посчитать сумму)
                }
            }

            return sum;
        }

        public override string ToString()
        {
            return Output.ToString();
        }
    }

}
   