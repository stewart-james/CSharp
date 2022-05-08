using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Common;
using CSharp10;

namespace Main
{
    public class Program
    {
        public static void Main()
            => new Program().Run();

        private readonly Dictionary<CSharpVersion, Assembly> _assemblies;

        public Program()
        {
            _assemblies = new Dictionary<CSharpVersion, Assembly>
            {
                { CSharpVersion.CSharp10, Assembly.GetAssembly(typeof(RecordStructs))! }
            };
        }

        private void Run()
        {
            var outputStream = Console.OpenStandardOutput();
            
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Select a C# version:");

                foreach (var vers in new[] { CSharpVersion.CSharp10 })
                    Console.WriteLine($"{(int)vers} - {ToString(vers)}");

                string? input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                    continue;

                if (string.Equals("Q", input, StringComparison.InvariantCultureIgnoreCase))
                    return;

                if (!Enum.TryParse(input, out CSharpVersion version))
                    continue;

                if (!_assemblies.ContainsKey(version))
                    continue;

                var features = GetFeaturesInAssembly(_assemblies[version], outputStream);

                if (!features.Any())
                {
                    Console.WriteLine($"No features for {ToString(version)}");
                    continue;
                }

                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("Select a feature:");
                    for (int i = 0; i < features.Count; ++i)
                        Console.WriteLine($"{i + 1} - {features[i].Name}");

                    input = Console.ReadLine();

                    if (string.Equals("Q", input, StringComparison.InvariantCultureIgnoreCase))
                        break;

                    if (!int.TryParse(input, out var featureNumber) || 
                        featureNumber == 0 || 
                        featureNumber > features.Count)
                        continue;
                    
                    Console.Clear();
                    var feature = features[featureNumber - 1];
                    feature.Run();
                    
                    Console.WriteLine("");
                    Console.WriteLine("");
                    Console.WriteLine("");
                    Console.WriteLine("Press any key to continue.");
                    Console.ReadKey();
                }
            }
        }

        private string ToString(CSharpVersion vers)
        {
            switch (vers)
            {
                case CSharpVersion.CSharp10:
                    return "C# 10";
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(vers), vers, "Unexpected version");
            }
        }

        private static List<Feature> GetFeaturesInAssembly(Assembly assembly, Stream stream)
            => assembly
                .GetTypes()
                .Where(t => t.IsAssignableTo(typeof(Feature)))
                .Select(t => (Feature?)Activator.CreateInstance(t, stream))
                .Where(i => i != null)
                .ToList()!;
    }
}