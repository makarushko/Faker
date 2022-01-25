using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Faker;
using Faker.DTO;
using FakerTest.DTO;
using NUnit.Framework;

namespace Test
{
    public class Tests
    {
        [Test]
        public void PluginLoadTest()
        {
            String pluginName1 = "C:\\Users\\Anna\\RiderProjects\\Faker\\Faker\\Plugins\\CharGenerator.dll";
            Assert.AreEqual("C:\\Users\\Anna\\RiderProjects\\Faker\\Faker\\Plugins\\CharGenerator.dll", pluginName1); 
            String pluginName2 = "C:\\Users\\Anna\\RiderProjects\\Faker\\Faker\\Plugins\\IntGenerator.dll";
            Assert.AreEqual("C:\\Users\\Anna\\RiderProjects\\Faker\\Faker\\Plugins\\IntGenerator.dll", pluginName2);
            Assembly assembly1 = Assembly.LoadFile(pluginName1);
            Assert.AreEqual("CharGenerator.dll", assembly1.ManifestModule.Name);
            Assembly assembly2 = Assembly.LoadFile(pluginName2);
            Assert.AreEqual("IntGenerator.dll", assembly2.ManifestModule.Name);
        }
        
        private Boo bar;
        private Faker.Faker faker;
        private User user;
        private Foo foo;
        [SetUp]
        public void Initialization()
        {
            faker = new Faker.Faker();
            bar = faker.Create<Boo>();
            user = faker.Create<User>();
            foo = faker.Create<Foo>();
        }
        [Test] 
        public void TestGenerateClass()
        {
            Assert.True(bar!=null);
        }
        [Test]
        public void TestGenerateFloat()
        {
            Assert.True(user.money != 0f);
            Assert.True(user.money is float);
        }
        [Test]
        public void TestGenerateString()
        {
            Assert.True(user.name != null);
            Assert.True(user.name is string);
        }
        
        [Test]
        public void TestGenerateList()
        {
            Assert.True(user.dogs.Count() > 0);
            foreach (Dog dog in user.dogs)
            {
                Assert.True(dog!=null);
            }
        }
       
        
    }
}