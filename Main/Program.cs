using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Common;
using CSharp10;

namespace Main
{
    public class Program
    {
        public static void Main()
            => new Program().Run();
        
        private readonly Dictionary<CSharpVersion, List<Feature>> _features;

        public Program()
        {
            var outputStream = Console.OpenStandardOutput();
            
            _features = new Dictionary<CSharpVersion, List<Feature>>
            {
                { CSharpVersion.CSharp10, new List<Feature>
                    {
                        MakeFeature<RecordStructs>(outputStream)
                    }
                }
            };
        }

        private void Run()
        {
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

                if (!_features.ContainsKey(version) || !_features[version].Any())
                {
                    Console.WriteLine($"No features for {ToString(version)}");
                    continue;
                }

                while (true)
                {
                    Console.Clear();
                    var features = _features[version];
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

        private Feature MakeFeature<T>(Stream stream) where T : Feature
            => (T)Activator.CreateInstance(typeof(T), stream)!;

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
    }
}