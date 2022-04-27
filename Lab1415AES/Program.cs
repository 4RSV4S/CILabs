using System;
using System.IO;

namespace Lab1415AES
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string FilePath = @"someFile.txt";
            string S;
   
            using (var Reader = new StreamReader(FilePath))
            {
                string Source = Reader.ReadToEnd();
                S = Source;
            }

            string source = S;
            byte[,] key = { { 0x2b, 0x28, 0xab, 0x09 }, { 0x7e, 0xae, 0xf7, 0xcf }, { 0x15, 0xd2, 0x15, 0x4f }, { 0x16, 0xa6, 0x88, 0x3c } };
            AES aes = new AES(key);

            byte[] encryptedArray = aes.Encrypt(source);

            Console.WriteLine("Encrypted array of bytes:");
            for (int i = 0; i < encryptedArray.Length; i++)
                Console.Write(Convert.ToString(encryptedArray[i], 16) + " ");

            string decryptedString = aes.Decrypt(encryptedArray);
            Console.WriteLine("\nDecrypted file:");
            Console.WriteLine(decryptedString);

            Console.ReadKey();
        }
    }
}
