using System;
using System.Text;

namespace Lab12Stream
{
    internal class Program
    {
        static void Main(string[] args)
        {
            byte[] key = new byte[100];
            string strKey;
            string testString = "Plaintext";
            byte[] testBytes = new byte[100];
            byte[] result = new byte[100];
            byte[] decryptedBytes = new byte[100];
            Input();
            void Input()
            {
                Console.WriteLine("Выберите действие:\n{1} Зашифровать текст, используя ключ\n{2} Дешифровать текст, используя ключ\n{3} Выход");
                string action = Console.ReadLine();
                while (action != "1" && action != "2" && action != "3")
                {
                    Console.WriteLine("Выберите действие (1-3).");
                    action = Console.ReadLine();
                }
                switch (action)
                {
                    case "1":
                        Encryptor encoder = new Encryptor();
                        Console.WriteLine("Введите текст:");
                        testString = Console.ReadLine();
                        while (testString.Length > 100)
                        {
                            Console.WriteLine("Текст не может быть длиннее 100 символов. Повторите ввод.");
                            testString = Console.ReadLine();
                        }
                        Console.WriteLine("Введите ключ:");
                        strKey= Console.ReadLine();
                        while (strKey.Length > 20 || strKey.Length < 4)
                        {
                            Console.WriteLine("Ключ не может быть короче 4 символов либо длиннее 20. Повторите ввод.");
                            strKey = Console.ReadLine();
                        }
                        key = ASCIIEncoding.ASCII.GetBytes(strKey);
                        encoder.init(key);
                        testBytes = Encoding.ASCII.GetBytes(testString);
                        result = encoder.Encrypt(testBytes, testBytes.Length);
                        Console.WriteLine($"Зашифрованный текст:\n{ASCIIEncoding.ASCII.GetString(result)}");
                        Input();
                        break;
                    case "2":
                        Encryptor decoder = new Encryptor();
                        Console.WriteLine("Введите ключ:");
                        strKey = Console.ReadLine();
                        while (strKey.Length > 20 || strKey.Length < 4)
                        {
                            Console.WriteLine("Ключ не может быть короче 4 символов либо длиннее 20. Повторите ввод.");
                            strKey = Console.ReadLine();
                        }
                        key = ASCIIEncoding.ASCII.GetBytes(strKey);
                        decoder.init(key);
                        byte[] decryptedBytes = decoder.Decrypt(result, result.Length);
                        string decryptedString = ASCIIEncoding.ASCII.GetString(decryptedBytes);
                        Console.WriteLine($"{decryptedString}");
                        Input();
                        break;
                    case "3":
                        break;
                }
            }
        }
    }
}
