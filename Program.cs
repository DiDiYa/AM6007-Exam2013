using System;
using System.IO;

namespace Exam2013
{


    class Program
    {
        static void Main(string[] args)
        {
            FunctionVector fv = new FunctionVector(3);
            fv[0] = x => x[0] + x[1] + x[2];
            fv[1] = x => x[0] + x[1] * x[2];
            fv[2] = x => x[0] * x[1] + x[2];
            Vector tmp = fv.Evaluate(new Vector(new double[] { 1, -1, 3 }));
            Console.WriteLine("tmp = {0}", tmp);

            double d = fv[0].Invoke(tmp);
            Console.WriteLine(d);

            Console.WriteLine("Start Step 1");
            //predator	prey	simulation
            //(1)	single	value
            PredPrey p = new PredPrey();
            p.Nsettle = 1000;
            Vector v0 = new Vector(new double[] { 0.83, 0.55 });
            p.Delta = 1.38;
            p.run1sim(v0, "C:\\Users\\114113597\\Downloads\\outfile.csv");
            Console.ReadKey();

            Console.WriteLine("Start Step 2");
            //(2)	produce	bifurcation	plot	data	use	default	values
            p.runsimDrange(v0, 1.26, 1.4, 1000, "C:\\Users\\114113597\\Downloads\\outfile1.csv");
            Console.ReadKey();

            Console.WriteLine("Start Step 3");
            //(3)	produce	second	bifurcation	plot
            p.R = 3;
            p.B = 3.5;
            p.D = 2;
            v0 = new Vector(new double[] { 0.57, 0.37 });
            p.runsimDrange(v0, 0.5, 0.95, 1000, "C:\\Users\\114113597\\Downloads\\outfile2.csv");
            Console.ReadKey();
            Console.WriteLine("Finished!!");
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
            if (size < 1)
                return;
            double[] tmp = new double[size];
            foreach (int i in values)
                tmp[i] = values[i];
            values = tmp;
        }

        public static Vector operator +(Vector lhs, Vector rhs)
        {
            if (lhs.values.Length != rhs.values.Length)
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
            get { return values[index]; }
            set { values[index] = value; }
        }
        public override string ToString()
        {
            string[] tmp = new string[values.Length];
            for (int i = 0; i < values.Length; i++)
                tmp[i] = string.Format("{0}", values[i]);
            return string.Join(" ,", tmp);
        }
    }

    public delegate double Function(Vector vector);

    public class FunctionVector
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
            if (size > 0 || size != 2)
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
            double[] array = new double[functionVector.Length];
            for (int i = 0; i < functionVector.Length; i++)
                array[i] = functionVector[i](values);
            Vector tmp = new Vector(array);
            return tmp;
        }

        public Function this[int index]
        {
            get { return functionVector[index]; }
            set { functionVector[index] = value; }
        }

    }

    class PredPrey
    {
        private FunctionVector fv;
        private double delta = 0.5, r = 2, b = 0.6, d = 0.5;
        private int nsettle = 200;
        private int nreps = 200;
        public double Delta { get; set; }
        public double R { get; set; }
        public double B { get; set; }
        public double D { get; set; }
        public double Nsettle { get; set; }

        public PredPrey()
        {
            Function x = (v) => { return r * v[0] * (1 - v[0] - b * v[0] * v[1]); };
            Function y = (v) => { return -d + b * v[0] * v[1]; };
            fv = new FunctionVector(new Function[] { x, y });
        }

        public void runsimDrange(Vector v0, double deltafrom, double deltato, int numsteps, string filename)
        {
            double[] deltalist = new double[numsteps];
            double h = (deltafrom - deltato) / numsteps;
            PredPrey p = new PredPrey();
            //FileStream f = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            //StreamWriter streamWriter = new StreamWriter(filename);
            Vector v = new Vector();
            Vector vloop = new Vector();
            v = v0;
            for (int i = 0; i < numsteps; i++)
            {
                deltalist[i] = deltafrom + (i + 1) * h;
                Vector deltav = new Vector(deltalist);
                for (int s = 0; s < nreps; s++)
                {
                    for (int t = 0; t < nsettle; t++)
                    {
                        StreamWriter streamWriter = new StreamWriter(filename);
                        streamWriter.Write(deltalist[i]);
                        streamWriter.Write(" , ");
                        vloop = fv.Evaluate(v);
                        v = v + vloop;
                        streamWriter.Close();
                        p.run1sim(v, filename);
                        
                    }
                }
            }

            
        }

        public void run1sim(Vector v0, string filename)
        {
           // FileStream f = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
            StreamWriter streamWriter = new StreamWriter(filename);
            streamWriter.Write(v0);
            streamWriter.WriteLine();
            streamWriter.Close();
        }


    }

}
