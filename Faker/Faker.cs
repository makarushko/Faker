using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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
        
        public object Create(Type type)
        {
            try
            {
                if (type.IsGenericType)
                {
                    try
                    {
                        return _generators.ContainsKey(Regex.Replace(type.Name.ToString(), "`.+$", ""))
                            ? _generators[Regex.Replace(type.Name.ToString(), "`.+$", "")]
                                .Generate(new GeneratorContext(this, type))
                            : CreateObject(type);
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                }
                else 
                    return _generators.ContainsKey(type.ToString()) ?
                        _generators[type.ToString()].Generate(new GeneratorContext(this, type)) : CreateObject(type);
            }
            catch (Exception)
            {
                if (_cycleDependClassHolder.Count == 0) return null;
                
                throw;
            }
            
        }
        private object CreateObject(Type type)
        {
            object obj = null;
            
            if (_cycleDependClassHolder.Contains(type)) throw new Exception("Cycle Dependency exception was found");
            _cycleDependClassHolder.Add(type);

            try
            {
                obj = InitConstructor(GetSortedConstructorInfos(type, new ConstrCompareAsc()));
                InitFields(obj, type.GetFields());
                InitProperties(obj, type.GetProperties());
            }
            catch (Exception)
            {
                
            }

            _cycleDependClassHolder.Remove(type);
            return obj; 
        }
        
        private object InitConstructor (IEnumerable<ConstructorInfo> constructors)  
        {
            object obj = null;
            
            foreach (var constructor in constructors)
            {
                if (constructor.IsPrivate)
                    continue;

                try
                {
                    var list = new List<object>();

                    for (var i = 0; i < constructor.GetParameters().Length; i++)
                    {
                        var paramType = constructor.GetParameters()[i].ParameterType;
                        list.Add(Create(paramType));
                    }
                    obj = constructor.Invoke(list.ToArray());
                    break;
                }
                catch (Exception)
                {
                    
                }
            }
            
            if (obj == null) throw  new Exception("Faker can't create object of class " + obj.GetType().Name);
            return obj;
        }
    }
}