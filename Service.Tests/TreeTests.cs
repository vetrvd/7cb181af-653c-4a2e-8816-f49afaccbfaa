using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Moq;
using Service.Model;
using Xunit;

namespace Service.Tests
{
    public class TreeTests
    {
        [Fact]
        public void Print_Empty()
        {
            var result = new Tree().Print("x");
            Assert.NotNull(result);
            Assert.Empty(result);
        }        
        
        [Fact]
        public void Print_ArgumentException()
        {
            Assert.Throws<ArgumentNullException>(() => new Tree().Print(String.Empty));
        }

        [Fact]
        public void Print_1()
        {
            var tree = new Tree(
            
                next : new List<Tree>()
                {
                    new Tree(
                        new List<Tree>(),
                        new List<Variable<int>>()
                        {
                            new Variable<int>("a", 3),
                            new Variable<int>("a", 4)
                        }
                    ),
                    new Tree(
                        new List<Tree>(),
                        new List<Variable<int>>()
                        {
                            new Variable<int>("a", 5),
                            new Variable<int>("a", 6)
                        }
                    )
                },
                value: new List<Variable<int>>()
                {
                    new Variable<int>("a", 1),
                    new Variable<int>("a", 2)
                }
            );
            
            var result = tree.Print("A");
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void Print_2()
        {
            var tree = new Tree(
                next: new List<Tree>()
                {
                    new Tree(
                        new List<Tree>(),
                        new List<Variable<int>>()
                        {
                            new Variable<int>("a", 3),
                            new Variable<int>("a", 4)
                        }
                    ),
                    new Tree(
                        new List<Tree>(),
                        new List<Variable<int>>()
                        {
                            new Variable<int>("a", 5),
                            new Variable<int>("a", 6)
                        }
                    )
                },
                value: new List<Variable<int>>()
                {
                    new Variable<int>("a", 1),
                    new Variable<int>("a", 2)
                }
            );
            
            var result = tree.Print("a").ToArray();
            Assert.NotNull(result);
            Assert.Equal(6, result.Count());

            for (var i = 1; i < 7; i++)
            {
                Assert.Equal(expected: i, actual: result[i-1]);
            }
        }
        
        [Fact]
        public void Print_3()
        {
            var tree = new Tree(
                next: new List<Tree>()
                {
                    new Tree(
                        new List<Tree>(),
                        new List<Variable<int>>()
                        {
                            new Variable<int>("a", 3),
                            new Variable<int>("a", 4)
                        }
                    ),
                    new Tree(
                        new List<Tree>(),
                        new List<Variable<int>>()
                        {
                            new Variable<int>("b", 5),
                            new Variable<int>("b", 6)
                        }
                    )
                },
                value: new List<Variable<int>>()
                {
                    new Variable<int>("a", 1),
                    new Variable<int>("a", 2)
                }
            );
            
            var result = tree.Print("a").ToArray();
            Assert.NotNull(result);
            Assert.Equal(4, result.Count());

            for (var i = 1; i < 5; i++)
            {
                Assert.Equal(expected: i, actual: result[i-1]);
            }
        }
        
    }
}