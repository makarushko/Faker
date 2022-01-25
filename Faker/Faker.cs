using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Faker.Generator;
using Faker.Generator.Collections;
using Faker.Generator.ScalarValues;
using Faker.MainApp;

namespace Faker
{
    public class Faker
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
        
        private void AutoLoadGenerators()
        {
            var pluginsLoader = new PluginLoader("C:\\Users\\Anna\\RiderProjects\\Faker\\Faker\\Plugins");
            _generators = pluginsLoader.LoadPlugins(this);
        }
        
        private void LoadGenerators()
        {
            _generators.Add(typeof(string).ToString(), new StringGenerator());
            _generators.Add(typeof(List<int>).ToString(), new ListGenerator());
            _generators.Add(Regex.Replace(typeof(List<string>).Name, "`.+$", ""), new ListGenerator());
            _generators.Add(typeof(bool).ToString(), new BoolGenerator());
        }
        public T Create<T>()
        {
            return (T) Create(typeof(T));
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
        
         private void InitFields(object obj, IEnumerable<FieldInfo> fields)
        {
            var defaultConstr = obj.GetType().GetConstructors()[0];
            var defaultObj = defaultConstr.Invoke(new object[defaultConstr.GetParameters().Length]);

            foreach (var field in fields)
            {
                var defaultValue = field.GetValue(defaultObj);
                var value = field.GetValue(obj); 
                
                if ( (defaultValue == null && value != null) ) continue;
                if (value != null && !defaultValue.Equals(value)) continue;

                try
                {
                    if (field.FieldType.IsPrimitive ^ (field.FieldType == typeof(string) & field.GetValue(obj) != null))
                    {
                        if (field.FieldType == typeof(string) && !field.GetValue(obj).Equals("")) continue;

                        var defValue = Activator.CreateInstance(field.FieldType);
                        var qe = field.GetValue(obj);

                        if (defValue.Equals(qe))
                        {
                            field.SetValue(obj, _generators[field.FieldType.ToString()]?.
                                Generate(new GeneratorContext(this, field.FieldType)));
                        }
                    }
                    else
                        field.SetValue(obj, Create(field.FieldType));
                }
                catch (Exception)
                {
                    
                }
            }
        }
        
        private void InitProperties(object obj, IEnumerable<PropertyInfo> properties) 
        {
            var defaultConstr = obj.GetType().GetConstructors()[0];
            var defaultObj = defaultConstr.Invoke(new object[defaultConstr.GetParameters().Length]);

            foreach (var property in properties)
            {
                var defaultValue = property.GetValue(defaultObj);
                var value = property.GetValue(obj); 
                
                if ( defaultValue == null && value != null ) continue;
                if (value != null && !defaultValue.Equals(value)) continue; 


                if (property.PropertyType.IsPrimitive ^ property.PropertyType == typeof(string))
                { 

                    if (property.SetMethod != null && (property.SetMethod.IsPrivate | property.SetMethod.IsFamily ) )
                        continue;
                    
                    
                    try
                    {
                        property.SetValue(obj, _generators[property.PropertyType.ToString()]?.
                            Generate(new GeneratorContext(this, property.PropertyType)));
                    }
                    catch (Exception)
                    {
                        
                    }

                } else Create(property.PropertyType);
            }  
        }
        
        private IEnumerable<ConstructorInfo> GetSortedConstructorInfos(Type type, IComparer<ConstructorInfo> constrCompare)
        {
            var constInfoList = type.GetConstructors().ToList();
            constInfoList.Sort(constrCompare);
            
            return constInfoList;
        } 
    }
}