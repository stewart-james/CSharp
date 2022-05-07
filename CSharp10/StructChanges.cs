using System.IO;
using System.IO.Compression;
using Common;

namespace CSharp10
{
    /*
        struct parameter-less constructors
        ---------------------------------------------------------------------------------------------------------------- 
        
        structs are now allowed to have parameterless constructors, in C# 9.0 they were not allowed.
        
        struct Point
        {
            // now allowed
            public Point()
            {
            }
        }
        
        The parameterless constructor must initialise all fields (including backing fields), 
        otherwise it is a compile-time error.
        
        e.g
        struct Point
        {
            public double X { get; }
            public double Y;
            
            public Point()
            {
            } 
            // compile-time error as the backing field for X has not been initialised
            // compile-time error as the field Y has not been initialised
        }
    */
    public class StructChanges : Feature
    {
        public StructChanges(Stream outputStream) : base(outputStream, "Struct Changes")
        {}

        public override void Run()
        {
            ParameterlessConstructor();
            StructWith();
        }

        public struct MyStruct
        {
            public int A { get; }
            public int B { get; }

            // structs are now allowed to have parameterless constructors
            public MyStruct()
            {
                A = 200;
                B = 200;
            }
        }

        private void ParameterlessConstructor()
        {
            WriteLine("Parameterless constructors:");
            WriteLine("");
            // the parameterless constructor is called
            var struc = new MyStruct();
            WriteLine($"new MyStruct(): A = {struc.A}, B = {struc.B}");

            // the parameterless constructor is NOT called
            var arr = new MyStruct[5];
            WriteLine("arr = new MyStruct[5]:");
            for(int i = 0; i < arr.Length; ++i)
                WriteLine($"arr[{i}]: A = {arr[i].A}, B = {arr[i].B}");

            // the parameterless constructor is NOT called
            var s = default(MyStruct);
            WriteLine($"var s = default(MyStruct): A = {s.A}, B = {s.B}");
            WriteLine("");
            WriteLine("");
            WriteLine("");
        }

        public struct Point
        {
            public double X { get; set; }
            public double Y { get; set; }
        }

        public struct PointFields
        {
            public double X;
            public double Y;
        }

        public class Person
        {
            public string FirstName;

            public Person(string name)
            {
                FirstName = name;
            }

            public void ChangeName(string newName)
            {
                FirstName = newName;
            }
        }

        public struct Relationship
        {
            public Person Husband;
            public Person Wife;
        }

        // 'with' expression can now be used on structure types and anonymous types
        private void StructWith()
        {
            WriteLine("Struct 'with' expressions:");
            WriteLine("");
            
            var p = new Point
            {
                X = 10,
                Y = 10
            };
            
            WriteLine($"p: X {p.X}, Y {p.Y}");

            // 'with' statement can now be used with structs in C# 10
            // this line would not compile in C# 9
            var p2 = p with { X = 20 };
            WriteLine("p2 = p with { X = 20}");
            WriteLine($"p2: X {p2.X}, Y {p2.Y}");
            WriteLine($"p: X {p.X}, Y {p.Y}");

            var pFields = new PointFields()
            {
                X = 10,
                Y = 10
            };

            // 'with' works with fields
            var pFields2 = pFields with { Y = 20 };
            WriteLine($"pFields2: X {pFields2.X}, Y {pFields2.Y}");

            // works with anonymous types and tuples
            var tuple = (x: 1, y: 2, z: 3);
            WriteLine($"tuple: {tuple}");
            var tuple2 = tuple with { y = 20 };
            WriteLine($"tuple2 = tuple with {{ y = 20}}: {tuple2}");

            var relationship = new Relationship
            {
                Husband = new Person("Kevin"),
                Wife = new Person("Sarah")
            };

            // unsupported in C# 9.0, 'with' could only be used with record types
            // affair.Wife will be the same reference as relationship.Wife
            var affair = relationship with { Husband = new Person("Martin") };
            WriteLine($"object.ReferenceEquals(relationship.Wife, affair.Wife): {ReferenceEquals(relationship.Wife, affair.Wife)}");
            
            // <non-record reference type> with { .. } is still invalid in c# 10
            // class Person
            //{
            //    public string FirstName { get; set; }
            //    public string LastName { get; set; }
            //}
            // var person1 = new Person { FirstName = "Lewis", LastName = "Hamilton"};
            // compile-time error:
            // var person2 = person1 with { LastName = "LeClerc"};
        }
    }
}