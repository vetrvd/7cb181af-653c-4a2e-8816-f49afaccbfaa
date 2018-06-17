using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text;

namespace Service.Model
{
    public class IfOperator : AOperator
    {
        private readonly Operations _repository;

        public IfOperator(
            Operations repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }
        
        public override string Tag { get; } = "if" ;
        
        public override Tree Process(string paramets, StreamReader streamReader)
        {
            var symbol = streamReader.Read();
            var stopChar = new int[] {'=', ' ', '\t', '(', ')', ';', '\r', '\n' };
            var buffer = new StringBuilder(20);
            int bracket = 0;

            while (symbol >= 0) 
            {
                if (buffer.Length == 0 && symbol != '(')
                {
                    symbol = streamReader.Read();
                    continue;
                }

                if (symbol == '(')
                {
                    bracket++;
                    buffer.Append(symbol);
                    symbol = streamReader.Read();
                    continue;
                }
                
                if (symbol == ')')
                {
                    bracket--;
                }

                if (bracket == 0)
                {
                    break;
                }

                buffer.Append((char)symbol);
                symbol = streamReader.Read();
            }
            
            buffer.Clear();
            while (symbol >= 0)
            {
                symbol = streamReader.Peek();
                if ((char.IsLetter((char)symbol)) && symbol != '{' )
                {
                    var tree = _repository.GetOperator(streamReader, 1);
                    return new Tree(new []{ tree }, new List<Variable<int>>());
                }

                if (symbol == '{')
                {
                    symbol = streamReader.Read();
                    var tree = _repository.GetOperator(streamReader);
                    return new Tree(new []{ tree }, new List<Variable<int>>());
                }

                symbol = streamReader.Read();
            }

            return new Tree();
        }
    }
}