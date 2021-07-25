using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.PropertyLookup.Models
{
    /// <summary>
    /// Concepts
    /// </summary>
    public class Concepts: Subtract, Addition
    {
        public void Display()
        {
            Console.WriteLine("Executing Implicit Display Method");
        }

        void Addition.Display()
        {
            Console.WriteLine("Executing Explicit display Method");
        }

        /// <summary>
        /// CheckingExceptionFilter
        /// </summary>
        public void CheckingExceptionFilter()
        {
            string data = null;
            data.Split(';');
        }
        public void ExplicitCasting()
        {
            Child child = new Child();
            Child c2 = (Child)new Parent();
            Parent parent = new Parent();
            Parent parentnew = new Child();
        }

        public void MethodHiding()
        {
            Child child = new Child();
            child.Display();
        }

        public void VirtualWithAbstract()
        {
            GrandChild grandChild = new GrandChild();
            grandChild.VirtualMethod();


            Parent parent = new GrandChild();
            parent.VirtualMethod();


            Parent parent1 = new Parent();
            parent1.VirtualMethod();

            //AbstractChild abstractChild = new GrandChild();
            //abstractChild.VirtualMethod();

            try
            {
                //AbstractChild abstractChild1 = new Parent(); // Requires explicit conversion
                //AbstractChild abstractChild2 = (AbstractChild)new Parent();
                //AbstractChild abstractChild3 = (GrandChild)new Parent();
            }
            catch(Exception ex)
            {

            }
        }

        public void Variance()
        {
            // Invariance
            Concepts concepts = new Concepts();
            List<GrandChild> list = new List<GrandChild>();
           // concepts.InVariance(list); FOR INVARIANCE EXPLICIT CONVERSION IS REQUIRED

            // Co Variance
            concepts.CoVariance(list); // FOR CO VARIANCE IT AUTOMATICALLY HAPPENS but if covariance method
            // accepts IEnumerable<GreaterGrandChild> then it wont accept

            //concepts.ContraVariance(new Action<GreaterGrandChild>(item => Console.WriteLine("Greater Grand Child"))); for Action<parent> as input
            
        }

        public void InVariance(IList<Parent> parents)
        {
            Console.WriteLine("Example of variance");
        }

        public void CoVariance(IEnumerable<Parent> parents)
        {
            Console.WriteLine("Example of Co variance");
        }

        public void ContraVariance(Action<Parent> action)
        {
            Console.WriteLine("Inside Contra Variance Method");
        }
    }

    public abstract class AbstractChild : Parent
    {
        public abstract override void VirtualMethod();

        public void AbstractChildMethod()
        {
            Console.WriteLine("Inside Abstract Child Method");
        }
    }

    public class GrandChild : Parent //: AbstractChild
    {
        public new void VirtualMethod()
        {
            Console.WriteLine("Inside GrnadChild Virtual Method");
        }

        public void GrandChildMethod()
        {
            Console.WriteLine("Inside GrandChild Method");
        }
    }

    public class GreaterGrandChild: GrandChild
    {
        public void GreaterGrandChildMethod()
        {
            Console.WriteLine("Inside Greater Grand Child");
        }
    }

    public class Parent
    {
        public void Dislay()
        {
            Console.WriteLine("Inside Parent");
        }

        public virtual void VirtualMethod()
        {
            Console.WriteLine("Inside Virtual Method");
        }

        public void ParentMethod()
        {
            Console.WriteLine("Inside parentmethod");
        }
    }

    public class Child : Parent
    {
        public void Display()
        {
            Console.WriteLine("Inside Child");
        }
    }

    interface Subtract
    {
        void Display();
    }

    interface Addition
    {
        void Display();
    }
}
