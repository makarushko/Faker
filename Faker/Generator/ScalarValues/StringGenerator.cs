using System;
using System.Text;

namespace Faker.Generator.ScalarValues
{
    public class StringGenerator : Generator
    {
        public StringGenerator()
        {
            
        }
        private Random random = new Random();
        public override object Generate(GeneratorContext context)
        {
            int length = random.Next(1, 20);
            string symbols = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            StringBuilder result = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                int pos = random.Next(0, symbols.Length);
                result.Append(symbols[pos]);
            }
            return (String)result.ToString();
        }

        public override string GetType()
        {
            return typeof(string).ToString();
        }
    }
}