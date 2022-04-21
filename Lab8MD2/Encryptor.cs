using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab8MD2
{
    internal class Encryptor
    {
        private string InputString { get; set; }
        private string Hash { get; set; }

        private void Input()
        {
            Console.WriteLine("Введите исходную строку: ");
            InputString = Console.ReadLine();
        }

        public void Encrypt()
        {
            Input();

            int num = 0;
            int c = 0;
            int l = 0;
            int t = 0;
            int[] S = new int[256]; //рандомный массив на 256 символов
            int[] C = new int[16]; //контрольная сумма
            int[] X = new int[48]; //48-битовый буфер 
            int blockAmount = 0;

            num = 16 - (InputString.Length % 16); //Step 1 Добавление недостающих бит
            for (int i = 0; i < num; i++)
            {
                InputString = InputString + num;
            }
            blockAmount = InputString.Length / 16;


            Random rand = new Random(); //Step 2 Добавление контрольной суммы
            for (int i = 0; i < S.Length; i++)
            {
                S[i] = rand.Next(255);
            }
               for (int i = 0; i < C.Length; i++)
            {
                C[i] = 0;
            }
            for (int i = 0; i < blockAmount; i++) 
            {
                for (int j = 0; j < C.Length; j++)
                {
                    c = Convert.ToInt32(InputString[i * 16 + j]);
                    int b;
                    b = c ^ l;
                    C[j] = C[j] ^ S[b];
                    l = C[j];
                }
            }

            for (int i = 0; i < C.Length; i++)
                InputString += C[i];


            for (int i = 0; i < X.Length; i++) //Step 3 Инициализация MD-буфера
                X[i] = 0;

            blockAmount = InputString.Length / 16; //Step 4 Обработка сообщения блоками по 16 байт
            for (int i = 0; i < blockAmount; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    X[16 + j] = InputString[i * 16 + j];
                    X[32 + j] = Convert.ToInt32(X[16 + j]) ^ Convert.ToInt32(X[j]);
                }
                for (int j = 0; j < 18; j++)
                {
                    for (int a = 0; a < X.Length; a++)
                    {
                        X[a] = X[a] ^ S[t];
                        t = X[a];
                    }
                    t = (t + j) % 256;
                }
            }

            for (int i = 0; i < 16; i++) //Step 5 Формирование хеша
                Hash += X[i].ToString();

            Console.WriteLine($"Хеш: {Hash}");
        }
    }
}
