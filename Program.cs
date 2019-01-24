using System;

namespace Exam2013
{


    class Program
    {
        static void Main(string[] args)
        {
            FunctionVector fv = new FunctionVector(3);
            fv[0] = x => x[0] + x[1] + x[2];
            fv[1] = x => x[0] + x[1] + x[2];
            fv[2] = x => x[0] + x[1] + x[2];
            Vector tmp = fv.Evaluate(new Vector(new double[] { 1, -1, 3 }));
            Console.WriteLine("tmp = {0}", tmp);
            Console.ReadKey();
        }
    }

    public class Vector
    {
        private double[] values = null;

        public Vector()
        {
            values = new double[2] { 0, 0 };
        }

        public Vector(int size)
        {
            if (size > 0 || size != 2)
                values = new double[size];
            foreach (int i in values)
                values[i] = 0;
        }

        public Vector(double[] values)
        {
            if (values.Length > 0)
                this.values = new double[values.Length];
            for (int i = 0; i < values.Length; i++)
                this.values[i] = values[i];
        }

        public void setSize(int size)
        {
            if ( size < 1)
                return;
            double[] tmp = new double[size];
            foreach (int i in values)
                tmp[i] = values[i];
            values = tmp;
        }

        public static Vector operator +(Vector lhs,Vector rhs)
        {
            if(lhs.values.Length != rhs.values.Length)
            {
                Console.WriteLine("Invalid Vector Addition Attemted");
                return null;
            }
            int i = 0;
            Vector tmp = new Vector(lhs.values.Length);
            for (i = 0; i < lhs.values.Length; i++)
                tmp.values[i] = lhs.values[i] + rhs.values[i];
            return tmp;
        }

        public static Vector operator -(Vector lhs, Vector rhs)
        {
            if (lhs.values.Length != rhs.values.Length)
            {
                Console.WriteLine("Invalid Vector Subtraction Attemted");
                return null;
            }
            int i = 0;
            Vector tmp = new Vector(lhs.values.Length);
            for (i = 0; i < lhs.values.Length; i++)
                tmp.values[i] = lhs.values[i] - rhs.values[i];
            return tmp;
        }

        public static Vector operator *(double lhs, Vector rhs)
        {
            int i = 0;
            Vector tmp = new Vector(rhs.values.Length);
            for (i = 0; i < rhs.values.Length; i++)
                tmp.values[i] = lhs * rhs.values[i];
            return tmp;
        }
        public static Vector operator *(Vector lhs, double rhs)
        {
            int i = 0;
            Vector tmp = new Vector(lhs.values.Length);
            for (i = 0; i < lhs.values.Length; i++)
                tmp.values[i] = lhs.values[i] - rhs;
            return tmp;
        }

        public double this[int index]
        {
            get
            {
               return values[index];
            }
            set
            {
                values[index] = value;
            }
        }
        public override string ToString()
        {
            return string.Format("{0}",values);
        }
    }

    public delegate double Function(Vector vector);

    class FunctionVector
    {


        private Function[] functionVector;

        public FunctionVector()
        {
            functionVector = new Function[2];
            for (int i = 0; i < functionVector.Length; i++)
                functionVector[i] = Vector => 0;
        }

        public FunctionVector(int size)
        {
            if( size>0 || size != 2 )
            {
                functionVector = new Function[size];
                for (int i = 0; i < functionVector.Length; i++)
                    functionVector[i] = Vector => 0;
            }
        }

        public FunctionVector(Function[] functions)
        {
            functionVector = new Function[functions.Length];
            Array.Copy(functions, functionVector, functions.Length);
        }

        public Vector Evaluate(Vector values)
        {
            Vector tmp = new Vector();
            tmp = values;
            return tmp;
        }

        public Function this[int index]
        {
            get
            {
                return functionVector[index];
            }
            set
            {
                functionVector[index] = value;
            }
        }

    }

    class PredPrey
    {
        private FunctionVector fv;
        private double delta = 0.5, r = 2, b = 0.6, d = 0.5;
        private int nsettle = 200;
        private int nreps = 200;

        public PredPrey()
        {

        }



    }









}
