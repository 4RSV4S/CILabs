using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab36
{
    internal class Decryptor
    {
        private string EncryptedString { get; set; }
        private string DecryptedString { get; set; }
        private string Key { get; set; }
        private List<int> KeyNumbers { get; set; }
        private string DistinctKey { get; set; }
        private List<int> DistinctKeyNumbers { get; set; }
        private string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public void Decrypt()
        {
            Input();
            DistinctKeyNumbers = KeyToNumbers(DistinctKey);
            List<char> encryptedList = DeleteDusruptSymbols();

            KeyNumbers = KeyToNumbers(Key);
            DecryptedString = Decipher(encryptedList);

            Console.WriteLine($"Дешифрованная строка:\n{DecryptedString}");
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

        private List<char> DeleteDusruptSymbols()
        {
            char[] encryptedArray;
            encryptedArray = EncryptedString.ToCharArray();

            for (int i = 0, j = 0, counter = 1; i < EncryptedString.Length; i++, counter++)
            {
                if (counter > DistinctKeyNumbers[j])
                {
                    encryptedArray[i] = '*';
                    counter = 0;
                    if (j >= DistinctKeyNumbers.Count - 1)
                    {
                        j = 0;
                    }
                    else
                    {
                        j++;
                    }
                }
            }
            List<char> encryptedList = encryptedArray.ToList<char>();
            encryptedList.RemoveAll(element => element == '*');

            return encryptedList;
        }

        private void Input()
        {
            Console.WriteLine("Введите зашифрованную строку:");
            EncryptedString = Console.ReadLine();
            Console.WriteLine("Введите ключ:");
            Key = Console.ReadLine().ToUpper();
            Console.WriteLine("Введите прерывающий ключ:");
            DistinctKey = Console.ReadLine().ToUpper();
        }

        private string Decipher(List<char> encryptedList)
        {
            StringBuilder output = new StringBuilder();
            int totalChars = encryptedList.Count;
            int totalColumns = (int)Math.Ceiling((double)totalChars / Key.Length);
            int totalRows = Key.Length;
            char[,] rowChars = new char[totalRows, totalColumns];
            char[,] colChars = new char[totalColumns, totalRows];
            char[,] unsortedColChars = new char[totalColumns, totalRows];
            int currentRow, currentColumn, i, j;

            for (i = 0; i < totalChars; ++i)
            {
                currentRow = i / totalColumns;
                currentColumn = i % totalColumns;
                rowChars[currentRow, currentColumn] = encryptedList[i];
            }

            for (i = 0; i < totalRows; ++i)
            {
                for (j = 0; j < totalColumns; ++j)
                {
                    colChars[j, i] = rowChars[i, j];
                }
            }

            for (i = 0; i < totalColumns; ++i)
            {
                for (j = 0; j < totalRows; ++j)
                {
                    unsortedColChars[i, j] = colChars[i, KeyNumbers[j]-1];
                }
            }

            for (i = 0; i < totalChars; ++i)
            {
                currentRow = i / totalRows;
                currentColumn = i % totalRows;
                output.Append(unsortedColChars[currentRow, currentColumn]);
            }

            return output.ToString();
        }
    }
}
