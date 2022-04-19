using System.Text;

namespace Lab7
{
    internal abstract class Counter
    {
        protected string HadjiMurat = System.IO.File.ReadAllText(@"hadji-murat.txt", Encoding.Default).Replace("\n", " ");
        protected string Alphabet = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";

        public abstract void Count();

        public abstract void CreateExcelHistogram();
    }
}
