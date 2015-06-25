using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreezyCode.Classes
{
    public class Base
    {
        public virtual void VirtualMethod()
        {
            Console.WriteLine("Base::VirtualMethod");
        }
    }

    public class Derived : Base
    {
        public override void VirtualMethod()
        {
            Console.WriteLine("Derived::VirtualMethod");
        }

    }

    public class MethodOverloading
    {
        public static void OverloadedMethod(Derived d)
        {
            d.VirtualMethod();
        }

        public static void OverloadedMethod(Base b)
        {
            b.VirtualMethod();
        }
    }

}
