using System;
using System.Collections.Generic;
using Faker.Generator;

namespace Faker
{
    class Faker
    {
        private Dictionary<string,IGenerator> _generators;
        private List<Type> _cycleDependClassHolder ;
        
        public Faker()
        {
            _generators = new Dictionary<string, IGenerator>();
            _cycleDependClassHolder = new List<Type>();
            
            AutoLoadGenerators();
            LoadGenerators();
        }
    }
}