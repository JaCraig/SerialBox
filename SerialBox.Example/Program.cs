using SerialBox.Enums;

namespace SerialBox.Example
{
    /// <summary>
    /// Example class
    /// </summary>
    public class Temp
    {
        /// <summary>
        /// Example property
        /// </summary>
        /// <value>
        /// Example value
        /// </value>
        public int A { get; set; }
    }

    /// <summary>
    /// Example program
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        private static void Main(string[] args)
        {
            // Object to serialize
            var TestObj = new Temp() { A = 100 };
            // Serialize to JSON
            var Value = TestObj.Serialize<string, Temp>(SerializationType.JSON);

            // Print the value of Value
            Console.WriteLine(Value);

            // Deserialize from JSON
            var TestObj2 = Value.Deserialize<Temp, string>(SerializationType.JSON);

            // Print the value of TestObj2.A
            Console.WriteLine(TestObj2.A);
        }
    }
}