using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Lab1Caesar
{
    public class Encryptor
    {
        private string InputString { get; set; }
        private string OutputString { get; set; }
        private int Key { get; set; }
        private string Alphabet = "АБВГДЕЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯАБВГДЕЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";

        private void ToUpperCase()
        {
            InputString = InputString.ToUpper();
            InputString = Regex.Replace(InputString, "[^А-Я]", "");
        }

        private void Displace(string operationType)
        {
            foreach(char symbol in InputString)
            {
                switch (operationType)
                {
                    case "encrypt":
                        OutputString += Alphabet[Alphabet.IndexOf(symbol) + Key];
                        break;
                    case "decrypt":
                        OutputString += Alphabet[Alphabet.IndexOf(symbol) + 32 - Key];
                        break;
                }
            }
        }

        public void Encrypt()
        {
            Console.WriteLine("ЗАДАНИЕ 1\nВведите исходную строку:");
            InputString = Console.ReadLine();
            Console.WriteLine("Введите ключ:");
            Key = Convert.ToInt32(Console.ReadLine());
            ToUpperCase();
            Displace("encrypt");
            Console.WriteLine($"Зашифрованная строка:\n{OutputString}");
            OutputString = null;
        }

        public void Decrypt()
        {
            Console.WriteLine("\nЗАДАНИЕ 2\nВведите зашифрованную строку:");
            InputString = Console.ReadLine();
            ToUpperCase();
            Console.WriteLine("Варианты дешифрованных строк:");
            for(int i = 0; i < 32; i++)
            {
                Key = i + 1;
                Displace("decrypt");
                Console.WriteLine(OutputString);
                OutputString = null;
            }
            Console.WriteLine("Назовите ключ смещения:");
            Key = Convert.ToInt32(Console.ReadLine());
            Displace("decrypt");
            Console.WriteLine(OutputString);
        }
    }
}