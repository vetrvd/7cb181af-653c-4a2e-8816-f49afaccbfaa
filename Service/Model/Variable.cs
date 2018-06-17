namespace Service.Model
{
    public struct Variable<T>
    {
        public string Name { get; }
        public T Value { get; }

        public Variable(string name, T value)
        {
            Name = name;
            Value = value;
        }
    }
}