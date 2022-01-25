using System.Collections.Generic;
using System.Security.Cryptography;
using Faker.DTO;

namespace FakerTest.DTO
{
    public class Foo
    {
        public string sdf;
        public int AAA = 123;
        public object ewr = new AesManaged();
        
        public int a, b;
        
        public string TestStr = "It's const str";
        public string Str {  set; get; }
        public A A;
        public Boo Boo;
            
        public Foo(int a)
        {
            this.a = a;
        }
            
        public Foo(int a, int b)
        {
            this.a = a;
            this.b = b;
        }
            
            
        public Foo()
        {

        }

    }
}