using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Office.Interop.Excel;

namespace Lab7
{
    internal class LetterCounter : Counter
    {
        private Dictionary<char, double> LettersDict = new Dictionary<char, double>();

        public override void Count()
        {
            for (int i = 0; i < Alphabet.Length; i++)
            {
                LettersDict.Add(Alphabet[i], 0);
            }

            foreach (var ch in HadjiMurat)
            {
                if (LettersDict.ContainsKey(char.ToUpper(ch)))
                {
                    LettersDict[char.ToUpper(ch)]++;
                }
            }

            foreach (var letter in LettersDict.Keys.ToList())
            {
                LettersDict[letter] = Math.Round(LettersDict[letter] / HadjiMurat.Length, 4);
            }

            string dashes = new string('-', 20);
            Console.WriteLine($"{dashes}\nTASK 2\n{dashes}");
            foreach (var _pair in LettersDict.OrderByDescending(_pair => _pair.Value))
            {
                Console.WriteLine($"{_pair.Key} - {_pair.Value}");
            }
        }

        public override void CreateExcelHistogram()
        {
            var Excel = new Application();
            Excel.Workbooks.Add();
            var sheet = (Worksheet)Excel.Worksheets.get_Item(1);

            for (int i = 0; i < Alphabet.Length; i++)
            {
                sheet.Range[$"A{i + 1}"].Value = $"{Alphabet[i]}";
                sheet.Range[$"B{i + 1}"].Value = $"{LettersDict[Alphabet[i]]}";
            }

            Excel.Charts.Add();

            Excel.ActiveChart.ChartType = XlChartType.xlColumnClustered;

            Excel.ActiveChart.HasLegend = false;
            Excel.ActiveChart.HasTitle = true;
            Excel.ActiveChart.ChartTitle.Characters.Text = "The frequency of each letter occurs";

            Excel.ActiveChart.Export("letters-histogram.jpg");
            Excel.Visible = true;
        }
    }
}
