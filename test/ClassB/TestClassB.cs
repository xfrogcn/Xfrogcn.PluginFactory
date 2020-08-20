using ClassA;
using System;

namespace ClassB
{
    public class TestClassB : TestClassA
    {
        public string B { get; set; }

        public override string ToString()
        {
            return this.A + this.B;
        }
    }
}
