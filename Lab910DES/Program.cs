using System;

namespace Lab910DES
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var enc = new Encryptor();
            enc.Encrypt();
        }
    }
}
