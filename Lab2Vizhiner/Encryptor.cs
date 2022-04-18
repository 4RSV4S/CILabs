using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Lab2Vizhiner
{
    internal class Encryptor
    {
        private string InputString { get; set; }
        private string OutputString { get; set; }
        private string Key { get; set; }
        private string Alphabet = "АБВГДЕЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯАБВГДЕЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";

        private void ToUpperCase()
        {
            InputString = InputString.ToUpper();
            Key = Key.ToUpper();
            InputString = Regex.Replace(InputString, "[^А-Я]", "");
        }

        private void Displace(string operationType)
        {
            int keyIndex = 0;

            foreach (char symbol in InputString)
            {
                switch (operationType)
                {
                    case "encrypt":
                        OutputString += Alphabet[(Alphabet.IndexOf(symbol) + Alphabet.IndexOf(Key[keyIndex])) % 32];
                        break;
                    case "decrypt":
                        OutputString += Alphabet[(Alphabet.IndexOf(symbol) + 32 - Alphabet.IndexOf(Key[keyIndex])) % 32];
                        break;
                }

                keyIndex++;

                if ((keyIndex) == Key.Length)
                {
                    keyIndex = 0;
                }
            }
        }

        public void Encrypt()
        {
            Console.WriteLine("ЗАДАНИЕ 1\nВведите исходную строку:");
            InputString = Console.ReadLine();
            Console.WriteLine("Введите ключ:");
            Key = Console.ReadLine();
            ToUpperCase();
            Displace("encrypt");
            Console.WriteLine($"Зашифрованная строка:\n{OutputString}");
            OutputString = null;
        }

        public void Decrypt()
        {
            Console.WriteLine("\nЗАДАНИЕ 2\nВведите зашифрованную строку:");
            InputString = Console.ReadLine();
            Console.WriteLine("Введите ключ:");
            Key = Console.ReadLine();
            ToUpperCase();
            Displace("decrypt");
            Console.WriteLine($"Дешифрованная строка:\n{OutputString}");
        }
    }
}
