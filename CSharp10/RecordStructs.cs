using System;
using System.IO;
using System.Runtime.InteropServices;
using Common;

namespace CSharp10
{
    /*
        record struct
        ---------------------------------------------------------------------------------------------------------------- 
        
        'record struct' is a new feature added to records, a 'record struct' produces a value-type with all the same
        benefits of record's added in C# 9.
        
        public record struct Point(double X, double Y);
        
        Creates a Point struct with synthesized code for:
        
            * An override of object.Equals(object), .Equals(R) where R is the record struct
            * == operator, != operator
            * Implements System.IEquatable<R>
            * Implementation of GetHashCode
            * Implementation of ToString()
                 - Uses a synthesized private method 'PrintMembers' that prints the name & value of each property
                 
        Declaring the following record struct:
                 
        public record struct Point
        {
            public double X { get; set; }
            public double Y { get; set; }
        }
        
        Will result in a struct:
        
        public struct Point : IEquatable<Point>
        {
            [CompilerGenerated]
            private double <X>k__BackingField;

            [CompilerGenerated]
            private double <Y>k__BackingField;

            public double X
            {
                [IsReadOnly]
                [CompilerGenerated]
                get
                {
                    return <X>k__BackingField;
                }
                [CompilerGenerated]
                set
                {
                    <X>k__BackingField = value;
                }
            }

            public double Y
            {
                [IsReadOnly]
                [CompilerGenerated]
                get
                {
                    return <Y>k__BackingField;
                }
                [CompilerGenerated]
                set
                {
                    <Y>k__BackingField = value;
                }
            }

            public override string ToString()
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("Point");
                stringBuilder.Append(" { ");
                if (PrintMembers(stringBuilder))
                {
                    stringBuilder.Append(" ");
                }
                stringBuilder.Append("}");
                return stringBuilder.ToString();
            }

            private bool PrintMembers(StringBuilder builder)
            {
                builder.Append("X");
                builder.Append(" = ");
                builder.Append(X.ToString());
                builder.Append(", ");
                builder.Append("Y");
                builder.Append(" = ");
                builder.Append(Y.ToString());
                return true;
            }

            public static bool operator !=(Point left, Point right)
            {
                return !(left == right);
            }

            public static bool operator ==(Point left, Point right)
            {
                return left.Equals(right);
            }

            public override int GetHashCode()
            {
                return EqualityComparer<double>.Default.GetHashCode(<X>k__BackingField) * -1521134295 + EqualityComparer<double>.Default.GetHashCode(<Y>k__BackingField);
            }

            public override bool Equals(object obj)
            {
                if (obj is Point)
                {
                    return Equals((Point)obj);
                }
                return false;
            }

            public bool Equals(Point other)
            {
                if (EqualityComparer<double>.Default.Equals(<X>k__BackingField, other.<X>k__BackingField))
                {
                    return EqualityComparer<double>.Default.Equals(<Y>k__BackingField, other.<Y>k__BackingField);
                }
                return false;
            }
        }
                         
        readonly record struct
        ---------------------------------------------------------------------------------------------------------------- 
        
        'readonly record struct' creates an immutable record struct, that contains only immutable properties.
        Attempting to add a mutable property to a readonly record struct, results in a compile-time error.
        
        public readonly record struct Point
        {
            public double X { get; set; } // compile-time error
            public double Y { get; set; } // compile-time error
        }
        
        If we write the Point from 'record struct' but specify the readonly keyword:
        
        public readonly record struct Point
        {
            public double X { get; }
            public double Y { get; }
        }
        
        The resulting struct will be almost completely identical, except that the struct and each of the properties 
        will have a [IsReadOnly] attribute.
        
        [IsReadOnly]
        public struct Point : IEquatable<Point>
        {
            public double X
            {
                [IsReadOnly]
                [CompilerGenerated]
                get
                {
                    return <X>k__BackingField;
                }
            }
            
            public double Y
            {
                [IsReadOnly]
                [CompilerGenerated]
                get
                {
                    return <Y>k__BackingField;
                }
            }
        }
    
        Record class
        ----------------------------------------------------------------------------------------------------------------
        
        'record' types default to reference types, so the following produces Person as a reference type:
        public record Person(string FirstName, string LastName);
        
        for symmetry with 'record struct', we can explicitly specify the record as a reference type using:
        public record class Person(string FirstName, string LastName); 
    
    
        Properties must be set before exit
        ----------------------------------------------------------------------------------------------------------------
     
        All properties must be set at exit, this record will cause a compile-time error
        because the Z property has not been set to a value
        
        public readonly record struct ReadOnlyPoint(double X, double Y)
        {
            public double Z { get; }
        }
        
        It is not enough to provide the value in a user-defined constructor
        so the following is also a compile-time error:
        
        public readonly record struct ReadOnlyPoint(double X, double Y)
        {
            public double Z { get; }
            
            public ReadOnlyPoint(double z) : this(0,0)
            {
                Z = z;
            }
        }
        
        This is because we can call the (double,double) constructor, which would cause Z to be unspecified at exit:
        var point = new ReadOnlyPoint(0, 0); point.Z is unspecified
        
        User-defined constructors
        ----------------------------------------------------------------------------------------------------------------
     
        If the record struct has a constructor 'ReadOnlyPoint2(double X, double Y)
        any user-defined constructor must make an explicit call to the this constructor
        
        public record struct ReadOnlyPoint(double X, double Y)
        {
            public double Z { get; } = 12.2
            
            public ReadOnlyPoint(double z) // compile-time error
            { 
                Z = z;
            }
        }
        
        public record struct PointUserDefinedConstructor(double X, double Y)
        {
            public double Z { get; } = 12.2;

            public PointUserDefinedConstructor(double z) : this(Double.MinValue, Double.MaxValue)
            {
                Z = z;
            }
        }
     */

    public class RecordStructs : Feature
    {
        public RecordStructs(Stream outputStream) : base(outputStream, "Record Structs") { }

        record struct Point
        {
            public double X { get; set; }
            public double Y { get; set; }

            public Point(double x, double y)
            {
                X = x;
                Y = y;
            }
        }

        readonly record struct ReadOnlyPoint(double X, double Y);

        public override void Run()
        {
            WriteLine($"typeof(Point).IsValueType: {typeof(Point).IsValueType}");
            WriteLine($"new Point(10,20).Equals(new Point(10,20)): {new Point(10,20).Equals(new Point(10,20))}");
            WriteLine($"new Point(10,20).Equals(new Point(0,0)): {new Point(10,20).Equals(new Point(0,0))}");
            WriteLine($"new Point(10,20).Equals(null): {new Point(10,20).Equals(null)}");
            WriteLine($"new Point(10,20).GetHashCode(): {new Point(10,20).GetHashCode()}");
            WriteLine($"new Point(10,20) == new Point(10,20): {new Point(10,20) == new Point(10,20)}");
            WriteLine($"new Point(10,20) != new Point(10,20): {new Point(10,20) != new Point(10,20)}");
            WriteLine($"new Point(10,20).ToString(): {new Point(10,20)}");
            
            var point = new Point(10.80, 7.21);
            WriteLine($"{point}");

            // valid
            point.X = 200;
            WriteLine($"{point}");

            var readonlyPoint = new ReadOnlyPoint(10.80, 7.21);
            WriteLine($"{readonlyPoint}");

            // invalid
            //readonlyPoint.X = 200;
            
            // valid
            readonlyPoint = new ReadOnlyPoint(200, 7.21);
            WriteLine($"{readonlyPoint}");
            
            // also supports with statement
            var readonlyPoint2 = readonlyPoint with { Y = 200 };
            WriteLine($"{readonlyPoint2}");

            unsafe
            {
                ReadOnlyPoint* readonlyPointPtr = &readonlyPoint;

                // still invalid at compile-time
                //readonlyPointPtr->X = 200;

                // no run-time checks are performed, we can change the value
                // if we REALLY want to
                Point* pointPtr = (Point*)readonlyPointPtr; // safe since Point & ReadOnlyPoint happen to be binary compatible
                pointPtr->X = 900;
                WriteLine($"{readonlyPoint}");
            }

            ExplicitLayoutRecordStructs();
        }

        [StructLayout(LayoutKind.Explicit, Size = 200)]
        public struct Sized200Struct { }

        [StructLayout(LayoutKind.Explicit, Size = 200)]
        public record struct Sized200RecordStruct;

        // Won't compile because the synthesized code will not have [FieldOffset] attributes
        //[StructLayout(LayoutKind.Explicit)]
        //public record struct RecordStruct(byte A, byte B);
        
        // Unsupported
        //[StructLayout(LayoutKind.Explicit)]
        //public record struct RecordStruct([FieldOffset(0)] byte A, [FieldOffset(4)] byte B);
        
        // Will work
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public record struct SequentialLayoutRecordStruct(uint A, byte B);

        public struct StructWithNoPacking
        {
            private uint A;
            private byte B;
        }

        void ExplicitLayoutRecordStructs()
        {
            // will be 200
            WriteLine($"SizeOf Sized200Struct: {Marshal.SizeOf<Sized200Struct>()}");
            
            // will be 200
            WriteLine($"SizeOf Sized200RecordStruct: {Marshal.SizeOf<Sized200RecordStruct>()}");
            
            // will be 5
            WriteLine($"SizeOf RecordStructWithPacking: {Marshal.SizeOf<SequentialLayoutRecordStruct>()}");
            
            // will be 8
            WriteLine($"SizeOf RecordWithNoPacking: {Marshal.SizeOf<StructWithNoPacking>()}");
        }
    }
}