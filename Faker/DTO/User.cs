using System;
using System.Collections.Generic;

namespace Faker.DTO
{
    public class User
    {
        public String name;
        public int age;
        public List<Dog> dogs;
        public long test { get; set; }
        public float money = 10.5f;
        public Profile profile;
    }

    public class Dog
    {
        public string name;
        public User owner;
        

    }

    public class Profile
    {
        public string address;
        public Profile(string address)
        {
            this.address = address;
        }

    }
}