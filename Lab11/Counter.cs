using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Lab11
{
    internal class Counter
    {
        private string Operation { get; set; }
        private BigInteger A { get; set; }
        private BigInteger B { get; set; }

        private void Input()
        {
            while (Operation != "1" && Operation != "2")
            {
                Console.WriteLine("Выберите операцию:\n{1} Вычитание / {2} Умножение");
                Operation = Console.ReadLine().ToString();
            }
            Console.WriteLine("Введите число A:");
            A = BigInteger.Parse(Console.ReadLine());
            Console.WriteLine("Введите число B:");
            B = BigInteger.Parse(Console.ReadLine());
        }
        public void Count()
        {
            Input();

            switch (Operation)
            {
                case "1": 
                    Console.WriteLine($"{A} - {B} = {A - B}");
                    break;
                case "2":
                    Console.WriteLine($"{A} * {B} = {A * B}");
                    break;
            }
        }
    }
}
