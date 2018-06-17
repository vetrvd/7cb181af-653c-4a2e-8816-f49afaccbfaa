using System.IO;

namespace Service.Model
{
    public abstract class AOperator
    {
        public abstract string Tag { get; }

        public abstract Tree Process(string paramets, StreamReader streamReader);
    }
}