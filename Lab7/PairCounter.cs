using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Office.Interop.Excel;

namespace Lab7
{
    internal class PairCounter : Counter
    {
        private Dictionary<string, double> PairsDict = new Dictionary<string, double>();

        public override void Count()
        {
            string pair;
            for (int i = 0; i < Alphabet.Length; i++)
            {
                for (int j = 0; j < Alphabet.Length; j++)
                {
                    pair = Alphabet[i].ToString() + Alphabet[j].ToString();
                    PairsDict.Add(pair, 0); 
                }
            }
            
            foreach(string _pair in PairsDict.Keys.ToList())
            {
                PairsDict[_pair] = Regex.Matches(HadjiMurat, _pair, RegexOptions.IgnoreCase).Count;
            }
            
            foreach(string _pair in PairsDict.Keys.ToList())
            {
                PairsDict[_pair] = Math.Round(PairsDict[_pair] / HadjiMurat.Length, 4);
            }

            string dashes = new string('-', 20);
            Console.WriteLine($"{dashes}\nTASK 3\n{dashes}");
            int count = 0;
            foreach (var _pair in PairsDict.OrderByDescending(_pair => _pair.Value))
            {
                Console.WriteLine($"{count+1} {_pair.Key} - {_pair.Value}");
                count++;
                if(count == 100)
                {
                    break;
                }
            }

            Console.WriteLine("\nНи разу не встретились в тексте:");
            foreach (string _pair in PairsDict.Keys.ToList())
            {
                if(PairsDict[_pair] == 0)
                {
                    Console.Write(_pair + ", ");
                }
            }
            
            Console.WriteLine("\n\nВстретились в тексте менее 3 раз:");
            foreach (string _pair in PairsDict.Keys.ToList())
            {
                if(PairsDict[_pair] != 0 && PairsDict[_pair] < 3)
                {
                    Console.Write(_pair + ", ");
                }
            }

        }

        public override void CreateExcelHistogram()
        {
            var Excel = new Application();
            Excel.Workbooks.Add();
            var sheet = (Worksheet)Excel.Worksheets.get_Item(1);

            int i = 0;
            foreach (var _pair in PairsDict.OrderByDescending(_pair => _pair.Value))
            {
                sheet.Range[$"A{i+1}"].Value = _pair.Key;
                sheet.Range[$"B{i+1}"].Value = _pair.Value;
                i++;
                if(i == 100)
                {
                    break;
                }
            }

            Excel.Charts.Add();

            Excel.ActiveChart.ChartType = XlChartType.xlColumnClustered;

            Excel.ActiveChart.HasLegend = false;
            Excel.ActiveChart.HasTitle = true;
            Excel.ActiveChart.ChartTitle.Characters.Text = "The top 100 pairs";

            Excel.ActiveChart.Export("pairs-histogram.jpg");
            Excel.Visible = true;
        }
    }
}
