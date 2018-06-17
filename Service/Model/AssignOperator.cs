using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Service.Model 
{
    public class AssignOperator : AOperator
    {
        public override string Tag { get; } = "=";
        public override Tree Process(string paramets, StreamReader streamReader)
        {
            var symbol = streamReader.Read();
            var buffer = new StringBuilder(20);
            var skipSymbol = new int[] {'\n', '\t', ' '};
            while (symbol >= 0 && symbol != ';')
            {
                if (buffer.Length == 0 && skipSymbol.Contains(symbol))
                {
                    symbol = streamReader.Read();
                    continue;
                }

                buffer.Append((char) symbol);
                symbol = streamReader.Read();
            }

            return new Tree(new Variable<int>(paramets.Trim(), Int32.Parse(buffer.ToString().Trim())));
        }
    }
}