using System;

namespace Lab1Caesar
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
