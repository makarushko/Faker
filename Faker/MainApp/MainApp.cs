using System;
using System.Xml;
using Faker.DTO;
using Newtonsoft.Json;

namespace Faker.MainApp
{
    internal static class MainApp
    {
        private static void Main(string[] args)
        {

            Faker faker = new Faker();

            var user = faker.Create<User>();
            Console.WriteLine(JsonConvert.SerializeObject(user, Formatting.Indented));

        }
    }
}