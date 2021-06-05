using System;
using System.Collections.Generic;
using System.Linq;
using fileorganizer_dotnet.Processors;


namespace fileorganizer_dotnet
{
    class Program
    {
        static void Main(string[] args)
        {
            var fileProcessors =  GetAll<FileProcessor>();
            
            Console.WriteLine(fileProcessors.Count());
            foreach (var processor in fileProcessors)
            {
                Console.WriteLine(processor.ProcessFiles());
            }
        }

        static IEnumerable<T> GetAll<T>()
        {
            return System.Reflection.Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.IsSubclassOf(typeof(T)) && !t.IsAbstract)
                .Select(t => (T) Activator.CreateInstance((t)));
        }
    }
}
