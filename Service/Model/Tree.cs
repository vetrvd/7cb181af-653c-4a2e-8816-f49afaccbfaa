using System;
using System.Collections.Generic;
using System.Linq;

namespace Service.Model
{
    public class Tree
    {
        private List<Tree> _next = new List<Tree>();
        public IEnumerable<Tree> Next => _next;
        
        private List<Variable<int>> _values  = new List<Variable<int>>();
        public IEnumerable<Variable<int>> Values => _values;
       
        public Tree()
        {
        }
        
        public Tree(Variable<int> value)
        {
            _values.Add(value);
        }
        
        public Tree(
            IEnumerable<Tree> next,
            IEnumerable<Variable<int>> value)
        {
            _next.AddRange(next);
            _values.AddRange(value);
        }

        public void Add(Tree tree)
        {
            foreach (var treeValue in tree._values.ToArray())
            {
                DeleteValue(treeValue.Name);
            }
            _next.AddRange(tree.Next);
            _values.AddRange(tree._values);
            
        }

        private void DeleteValue(string name)
        {
            foreach (var item in _values.Where(it => it.Name.Equals(name)).ToArray())
            {
                _values.Remove(item);
            }

            foreach (var tree in _next)
            {
                tree.DeleteValue(name);
            }
        }

        public IEnumerable<string> GetVariable()
        {
            var result = new List<string>();
            
            //change stack -> queue to BFS
            var stack = new Stack<Tree>();
            stack.Push(this);

            while (stack.TryPop(out var item))
            {
                result.AddRange(item.Values.Select(it => it.Name));
                foreach (var next in item.Next.Reverse())
                {
                    stack.Push(next);
                }
            }
            
            return result.Distinct();
        }
        
        public IEnumerable<int> Print(string variableName)
        {
            if (string.IsNullOrEmpty(variableName))
            {
                throw new ArgumentNullException(nameof(variableName));
            }

            var result = new List<int>();
            
            //change stack -> queue to BFS
            var stack = new Stack<Tree>();
            stack.Push(this);

            while (stack.TryPop(out var item))
            {
                result.AddRange(item.Values.Where(it => it.Name.Equals(variableName)).Select(it => it.Value).ToList());
                foreach (var next in item.Next.Reverse())
                {
                    stack.Push(next);
                }
            }
            
            return result.Distinct();
        }

    }
}