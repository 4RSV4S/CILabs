using System;

namespace Lab7
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var counter = new LetterCounter();
            counter.Count();
            //counter.CreateExcelHistogram();

            var pairCounter = new PairCounter();
            pairCounter.Count();
            //pairCounter.CreateExcelHistogram();
        }
    }
}
