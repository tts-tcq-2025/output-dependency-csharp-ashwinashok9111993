using System;

namespace TshirtSpace {
    public interface ITshirtSizeClassifier {
        string Size(int cms);
    }

    public class TshirtSizeClassifier : ITshirtSizeClassifier {
        public string Size(int cms) {
            if(cms < 38) {
                return "S";
            } else if(cms > 38 && cms < 42) {
                return "M";
            } else {
                return "L";
            }
        }
    }

    public class Tshirt {
        private readonly ITshirtSizeClassifier _classifier;

        public Tshirt(ITshirtSizeClassifier classifier) {
            _classifier = classifier;
        }

        public string GetSize(int cms) {
            return _classifier.Size(cms);
        }
    }

    class Program {
        static void Main(string[] args) {
            var classifier = new TshirtSizeClassifier();
            var tshirt = new Tshirt(classifier);

            // Production usage example
            Console.WriteLine($"Size for 37cm: {tshirt.GetSize(37)}");
            Console.WriteLine($"Size for 40cm: {tshirt.GetSize(40)}");
            Console.WriteLine($"Size for 43cm: {tshirt.GetSize(43)}");
            
            Console.WriteLine("Tshirt application running. Run the tests to check for bugs.");
        }
    }
}
