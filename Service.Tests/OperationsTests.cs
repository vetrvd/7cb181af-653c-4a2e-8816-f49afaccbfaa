using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;
using Service.Model;
using Xunit;

namespace Service.Tests
{
    public class OperationsTests
    {
 
        [Fact]
        public void GetOperation_Empty()
        {
            string val = @"x = 1;";
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(val)))
            using (var stream = new StreamReader(ms))
            {
                var l = new List<AOperator>();
                var provider = new Operations(l);
                l.Add(new AssignOperator());
                l.Add(new IfOperator(provider));

                var tree = provider.GetOperator(stream);
                
                Assert.NotNull(tree);
                Assert.NotEmpty(tree.Values);
                Assert.Empty(tree.Next);
            }
        }
        
        [Fact]
        public void GetOperation_Empty_2()
        {
            string val = @"
x = 1;
x = 1;";
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(val)))
            using (var stream = new StreamReader(ms))
            {
                var l = new List<AOperator>();
                var provider = new Operations(l);
                l.Add(new AssignOperator());
                l.Add(new IfOperator(provider));

                var tree = provider.GetOperator(stream);
                
                Assert.NotNull(tree);
                Assert.NotEmpty(tree.Values);
                Assert.Empty(tree.Next);
                Assert.Equal(tree.Values.Count() , 1);
            }
        }

        [Fact]
        public void GetOperation_Empty_3()
        {
            string val = @"
ifg = 1;
ifg = 1;";
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(val)))
            using (var stream = new StreamReader(ms))
            {
                var l = new List<AOperator>();
                var provider = new Operations(l);
                l.Add(new AssignOperator());
                l.Add(new IfOperator(provider));

                var tree = provider.GetOperator(stream);
                
                Assert.NotNull(tree);
                Assert.NotEmpty(tree.Values);
                Assert.Empty(tree.Next);
                Assert.Equal(tree.Values.Count() , 1);
            }
        }

        [Fact]
        public void GetOperation_If_1()
        {
            string val = @"
x = 1;
if(sfd()) 
x = 2;";
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(val)))
            using (var stream = new StreamReader(ms))
            {
                var l = new List<AOperator>();
                var provider = new Operations(l);
                l.Add(new AssignOperator());
                l.Add(new IfOperator(provider));

                var tree = provider.GetOperator(stream);
                
                Assert.NotNull(tree);
                Assert.NotEmpty(tree.Values);
                Assert.NotEmpty(tree.Next);
                Assert.Equal(tree.Values.Count() , 1);
                Assert.Equal(tree.Next.Count() , 1);
            }
        }
        
        [Fact]
        public void GetOperation_If_2()
        {
            string val = @"
x = 1;
if(sfd()) 
x = 2;
x = 1;";
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(val)))
            using (var stream = new StreamReader(ms))
            {
                var l = new List<AOperator>();
                var provider = new Operations(l);
                l.Add(new AssignOperator());
                l.Add(new IfOperator(provider));

                var tree = provider.GetOperator(stream);
                
                Assert.NotNull(tree);
                Assert.NotEmpty(tree.Values);
                Assert.NotEmpty(tree.Next);
                Assert.Equal(tree.Values.Count() , 1);
                Assert.Empty(tree.Next.First().Values);
            }
        }
        
        [Fact]
        public void GetOperation_If_3()
        {
            string val = @"
x = 1;
if(sfd())
{
x = 2;
}";
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(val)))
            using (var stream = new StreamReader(ms))
            {
                var l = new List<AOperator>();
                var provider = new Operations(l);
                l.Add(new AssignOperator());
                l.Add(new IfOperator(provider));

                var tree = provider.GetOperator(stream);
                
                Assert.NotNull(tree);
                Assert.NotEmpty(tree.Values);
                Assert.NotEmpty(tree.Next);
                Assert.Equal(tree.Values.Count() , 1);
                Assert.NotEmpty(tree.Next.First().Values);
            }
        }
        
        [Fact]
        public void GetOperation_If_4()
        {
            string val = @"
        x = 1;
        if (conditions[0]) {
            x = 2;
            if (conditions[1]) {
                x = 3;
            }
            x = 4;
            if (conditions[2]) {
                x = 5;
            }
        }
        if (conditions[3]) {
            x = 6;
        }";
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(val)))
            using (var stream = new StreamReader(ms))
            {
                var l = new List<AOperator>();
                var provider = new Operations(l);
                l.Add(new AssignOperator());
                l.Add(new IfOperator(provider));

                var tree = provider.GetOperator(stream);
                
                Assert.NotNull(tree);
                Assert.NotEmpty(tree.Values);
                Assert.NotEmpty(tree.Next);
            }
        }

        [Fact]
        public void GetOperation_Integration()
        {
            string val = @"
        int x;
        x = 1;
        if (conditions[0]) {
            x = 2;
            if (conditions[1]) {
                x = 3;
            }
            x = 4;
            if (conditions[2]) {
                x = 5;
            }
        }
        if (conditions[3]) {
            x = 6;
        }";
            Tree tree = null;
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(val)))
            using (var stream = new StreamReader(ms))
            {
                var l = new List<AOperator>();
                var provider = new Operations(l);
                l.Add(new AssignOperator());
                l.Add(new IfOperator(provider));

                tree = provider.GetOperator(stream);
                var result = tree.Print("x");
                
                Assert.Equal(expected: "1,4,5,6",  actual: string.Join(",", result));
            }

        }
    }
}