using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Service.Model;

namespace Service
{
    public class Operations
    {
        private readonly IEnumerable<AOperator> _operators;

        public Operations(IEnumerable<AOperator> aOperator)
        {
            _operators = aOperator;
        }

        public Tree GetOperator(StreamReader stream, int? limitOperation = null)
        {
            var result = new Tree();
            var symbol = stream.Read();
            var buffer = new StringBuilder(20);
            var limit = limitOperation;
            var skipSymbol = new int[]{'\r','\n','\t', ' '};
            var stopSymbol = new int[]{'\r','\n','\t', ' ', '('};

            while (symbol >= 0 && symbol != '}') 
            {
                if (buffer.Length == 0 && skipSymbol.Contains(symbol))
                {
                    symbol = stream.Read();
                    continue;
                }

                if (buffer.Length != 0 && symbol == ';')
                {
                    buffer.Clear();
                    symbol = stream.Read();
                    continue;
                }

                var curOperation = _operators.FirstOrDefault(it =>
                    it.Tag.Equals(((char)symbol).ToString(), StringComparison.CurrentCultureIgnoreCase));

                if (curOperation != null)
                {
                    var tree = curOperation.Process(buffer.ToString(), stream);
                    result.Add(tree);
                    buffer.Clear();
                    if (limit.HasValue)
                    {
                        if (limit == 1)
                        {
                            return result;
                        }

                        limit = limit - 1;
                    }
                }
                else
                {
                    buffer.Append((char)symbol);
                    var next = stream.Peek();
                    if (!stopSymbol.Contains(next))
                    {
                        symbol = stream.Read();
                        continue;
                    }
                    
                    var bufferTag = buffer.ToString().Trim();
                    
                    curOperation = _operators.FirstOrDefault(it =>
                        it.Tag.Equals(bufferTag, StringComparison.CurrentCultureIgnoreCase));

                    if (curOperation != null)
                    {
                        var tree = curOperation.Process(buffer.ToString(), stream);
                        result.Add(tree);
                        buffer.Clear();
                        if (limit.HasValue)
                        {
                            if (limit == 1)
                            {
                                return result;
                            }

                            limit = limit - 1;
                        }

                    }
                }
                symbol = stream.Read();
            }
            return result;
        }
    }
}