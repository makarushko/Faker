using System.Collections.Generic;
using Faker.Generator;

namespace Faker.MainApp
{
    public class PluginLoader
    {
        private readonly string _pluginsPath;

        private Dictionary<string, IGenerator> Generators { get; set; }

        public PluginLoader(string pluginsPath)
        {
            _pluginsPath = pluginsPath;
        }

        public Dictionary<string, IGenerator> LoadPlugins(Faker faker)
        {
            
            return Generators;
        }
    }
}