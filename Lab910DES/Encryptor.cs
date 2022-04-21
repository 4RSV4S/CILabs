using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace Lab910DES
{
    internal class Encryptor
    {
        private string FirstName = "Arseni";
        private string LastName = "Vasenda";
        private string Message;
        private string Key;

        private void SetMessage()
        {
            int i = 0;
            Message += LastName;
            while(Message.Length < 8)
            {
                Message += FirstName[i];
                i++;
            }
        }

        private void SetKey()
        {
            for(int i = 0; i < 7; i++)
            {
                Key += LastName[i]; 
            }
            Key += ' ';
        }

        public void Encrypt()
        {
            SetMessage();
            SetKey();
            Console.WriteLine($"Шифруемое сообщение в символьном представлении:\n{Message}");
            Console.WriteLine($"Шифруемое сообщение в битовом представлении:\n{ConvertToBinary(Message)}");
            Console.WriteLine($"Ключ в символьном представлении:\n{Key}");
            Console.WriteLine($"Ключ в битовом представлении:\n{ConvertToBinary(Key)}");
            Message = DESEncryption();
        }

        private string DESEncryption()
        {
            Message = ConvertToBinary(Message);
            StringBuilder sb = new StringBuilder();
            string[] keys = KeyGenerator(ConvertToBinary(Key));
            Console.WriteLine("Ключевые элементы ki: ");
            for(int i = 0; i < keys.Length; i++)
            {
                Console.Write($"{keys[i]}\n");
            }
            for (int i = 0; i < Message.Length; i += 64)
            {
                Console.WriteLine($"Pезультат начальной перестановки IP:\n{Transposition(Message.Substring(i, 64), Tables.IP)}");
                string Li, Hi, middle, part = Transposition(Message.Substring(i, 64), Tables.IP);
                Li = part.Substring(32, part.Length / 2);
                Hi = part.Substring(0, part.Length / 2);
                Console.WriteLine($"полублоки Hi и Li:\n{Li}\n{Hi}");
                Console.WriteLine("f(ki, Li):");
                for (int k = 0; k < 16; k++)
                {
                    Console.WriteLine(f(Li, keys[k]));
                    middle = Hi;
                    Hi = Li;
                    Li = XOR(middle, f(Li, keys[k]));
                }
                Console.WriteLine($"Hi + f(ki, Li):\n{Li + Hi}");
                Console.WriteLine($"Результат конечной перестановки IP-1:\n{Transposition(Li + Hi, Tables.IP1)}");
                sb.Append(Transposition(Li + Hi, Tables.IP1));
            }

            return BinaryToString(sb.ToString());
        }

        private string BinaryToString(string data)
        {
            List<Byte> byteList = new List<Byte>();

            for (int i = 0; i < data.Length; i += 8)
            {
                byteList.Add(Convert.ToByte(data.Substring(i, 8), 2));
            }
            return Encoding.Default.GetString(byteList.ToArray());
        }

        private string f(string RightPart, string key) //+
        {
            RightPart = Transposition(RightPart, Tables.table);
            RightPart = XOR(key, RightPart);

            StringBuilder sb = new StringBuilder();
            for (int i = 0, j = 0; i < 8; i++, j += 6)
            {
                int row = Convert.ToInt32(Convert.ToString(RightPart.Substring(j, 6)[0]) + Convert.ToString(RightPart.Substring(j, 6)[5]), 2);
                int coloumn = Convert.ToInt32(RightPart.Substring(j, 6).Substring(1, 4), 2);

                sb.Append(Convert.ToString(Tables.S[i][row, coloumn], 2).PadLeft(4, '0'));
            }

            RightPart = Transposition(sb.ToString(), Tables.P);

            return RightPart;
        }

        private string ConvertToBinary(string message)
        {
            StringBuilder str = new StringBuilder("");
            byte[] test = Encoding.Default.GetBytes(message);
            for (int i = 0; i < message.Length; i++)
            {
                str.Append(Convert.ToString(test[i], 2).PadLeft(8, '0'));
            }
            return str.ToString();
        }

        private string XOR(string str1, string str2)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < str2.Length; i++)
                sb.Append((byte)(str2[i] ^ str1[i]));
            return sb.ToString();
        }

        private string Transposition(string str, int[] array)
        {
            StringBuilder sb = new StringBuilder("");
            for (int i = 0; i < array.Length; i++)
            {
                sb.Append(str[array[i] - 1]);
            }
            return sb.ToString();
        }

        private string[] KeyGenerator(string key)
        {
            string[] result = new string[16];
            int[] move = { 1, 1, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 1 };

            key = Transposition(key, Tables.PC1);
            string G0 = key.Substring(0, key.Length / 2);
            string D0 = key.Substring(key.Length / 2, key.Length / 2);

            for (int i = 0; i < 16; i++)
            {
                G0 = MoveLeft(G0, move[i]);
                D0 = MoveLeft(D0, move[i]);
                result[i] = Transposition(G0 + D0, Tables.PC2);

            }
            return result;
        }

        private string MoveLeft(string str, int bit)
        {
            for (int i = 0; i < bit; i++) str = str.Remove(0, 1) + "0";
            return str;
        }
    }        
}
