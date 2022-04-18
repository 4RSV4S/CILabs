using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab36_2
{
    internal class Encryptor
    {
        private string InputString { get; set; }
        private string OutputString { get; set; }
        private string IKey { get; set; }
        private string JKey { get; set; }
        private string Alphabet = "АБВГДЕЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";
        private void Input()
        {
            Console.WriteLine("Введите исходную строку:");
            InputString = Console.ReadLine();
            Console.WriteLine("Введите ключ I:");
            IKey = Console.ReadLine().ToUpper();
            Console.WriteLine("Введите ключ J:");
            JKey = Console.ReadLine().ToUpper();
            OutputString = "";
        }
        public void Encrypt()
        {
            Input();
            char[,] inputArray = new char[2,2];
            int index = 0;

            for(int i = 0, k = 0; i < 2; i++)
            {
                for(int j = 0; j < 2; j++)
                {
                    inputArray[i, j] = InputString[k];
                    k++;
                }
            }

            List<int> iKeyNumbers = KeyToNumbers(IKey);
            List<int> jKeyNumbers = KeyToNumbers(JKey);
            List<char> tempList = new();

            if (iKeyNumbers[0] > iKeyNumbers[1])
            {
                tempList.Add(inputArray[0,0]);
                tempList.Add(inputArray[0,1]);

                inputArray[0, 0] = inputArray[1, 0];
                inputArray[0, 1] = inputArray[1, 1];

                inputArray[1, 0] = tempList[0];
                inputArray[1, 1] = tempList[1];
            }
            if (jKeyNumbers[0] > jKeyNumbers[1])
            {
                tempList.Add(inputArray[0, 0]);
                tempList.Add(inputArray[0, 1]);

                inputArray[0, 0] = inputArray[0, 1];
                inputArray[1, 0] = inputArray[1, 1];

                inputArray[0, 1] = tempList[0];
                inputArray[1, 1] = tempList[1];
            }

            for (int i = 0, k = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    OutputString = OutputString.Insert(k, inputArray[i, j].ToString());
                    k++;
                }
            }
            Console.WriteLine($"Зашифрованная строка:\n{OutputString}");
        }
        private List<int> KeyToNumbers(string key)
        {
            List<int> keyNumbers = new();
            List<int> sortedKeyNumbers = new();
            List<int> markedIndexes = new();

            for (int i = 0; i < key.Count(); i++)
            {
                keyNumbers.Add(Alphabet.IndexOf(key[i]));
            }

            for (int i = 0; i < keyNumbers.Count(); i++)
            {
                sortedKeyNumbers.Add(keyNumbers[i]);
            }
            sortedKeyNumbers.Sort();
            sortedKeyNumbers = sortedKeyNumbers.Distinct().ToList();

            for (int i = 0; i < sortedKeyNumbers.Count; i++)
            {
                for (int j = 0; j < keyNumbers.Count; j++)
                {
                    if (sortedKeyNumbers[i] == keyNumbers[j] && !markedIndexes.Contains(j))
                    {
                        keyNumbers[j] = i + 1;
                        markedIndexes.Add(j);
                    }
                }
            }
            return keyNumbers;
        }
    }
}
