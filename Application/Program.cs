using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Service;
using Service.Model;

namespace Application
{
    class Program
    {
        public static void Main(string[] args)
        {
            var list = new List<AOperator>();
            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .AddSingleton<Operations>(it => 
                    new Operations(list))
                .AddSingleton<FileDataProvider>(it => 
                    new FileDataProvider(
                        it.GetService<ILogger<FileDataProvider>>(), 
                        it.GetService<Operations>(),
                        args[0]))
                .AddSingleton<IfOperator>()
                .AddSingleton<AssignOperator>()
                .BuildServiceProvider();
            
            list.Add(serviceProvider.GetService<AssignOperator>());
            list.Add(new IfOperator(serviceProvider.GetService<Operations>()));

            serviceProvider
                .GetService<ILoggerFactory>()
                .AddConsole(LogLevel.Debug);

            var logger = serviceProvider.GetService<ILoggerFactory>()
                .CreateLogger<Program>();

            using (var provider = serviceProvider.GetService<FileDataProvider>())
            {                
                var tree = provider.GetTree();
                Console.WriteLine($"[{string.Join(",", tree.Print(tree.GetVariable().FirstOrDefault()?? "y"))}]");
            }
        }
    }
}