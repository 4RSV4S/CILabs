using System;

namespace Lab2Vizhiner
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var enc = new Encryptor();
            enc.Encrypt();
            enc.Decrypt();
        }
    }
}
