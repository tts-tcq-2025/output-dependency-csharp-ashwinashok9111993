using System;
using System.Collections.Generic;

namespace MisalignedSpace {
    public interface IColorMapFormatter {
        string FormatColorEntry(int number, string majorColor, string minorColor);
    }

    public interface IOutputWriter {
        void WriteLine(string text);
    }

    public class ColorMapFormatter : IColorMapFormatter {
        public string FormatColorEntry(int number, string majorColor, string minorColor) {
            // Bug: This misalignment will be exposed by tests
            return string.Format("{0} | {1} | {2}", number, majorColor, minorColor);
        }
    }

    public class ConsoleWriter : IOutputWriter {
        public void WriteLine(string text) {
            Console.WriteLine(text);
        }
    }

    public class ColorMapGenerator {
        private readonly IColorMapFormatter _formatter;
        private readonly IOutputWriter _writer;

        public ColorMapGenerator(IColorMapFormatter formatter, IOutputWriter writer) {
            _formatter = formatter;
            _writer = writer;
        }

        public List<string> GenerateColorMap() {
            string[] majorColors = {"White", "Red", "Black", "Yellow", "Violet"};
            string[] minorColors = {"Blue", "Orange", "Green", "Brown", "Slate"};
            var colorMap = new List<string>();
            
            int i = 0, j = 0;
            for(i = 0; i < 5; i++) {
                for(j = 0; j < 5; j++) {
                    // Bug: using minorColors[i] instead of minorColors[j]
                    string formattedEntry = _formatter.FormatColorEntry(i * 5 + j, majorColors[i], minorColors[i]);
                    colorMap.Add(formattedEntry);
                    _writer.WriteLine(formattedEntry);
                }
            }
            return colorMap;
        }

        public int GetTotalEntries() {
            string[] majorColors = {"White", "Red", "Black", "Yellow", "Violet"};
            string[] minorColors = {"Blue", "Orange", "Green", "Brown", "Slate"};
            return majorColors.Length * minorColors.Length;
        }
    }

    class Program {
        static void Main(string[] args) {
            var formatter = new ColorMapFormatter();
            var writer = new ConsoleWriter();
            var generator = new ColorMapGenerator(formatter, writer);
            
            Console.WriteLine("Generating color map:");
            var colorMap = generator.GenerateColorMap();
            
            Console.WriteLine($"\nGenerated {colorMap.Count} color entries.");
            Console.WriteLine("Color map application running. Run the tests to check for bugs.");
        }
    }
}
