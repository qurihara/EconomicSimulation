using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

using CenterSpace.NMath.Core;
using CenterSpace.NMath.Analysis;


namespace EconomicSimulation
{
    public partial class Form1 : Form
    {

        //debug
        //public static int nAllPlayer = 200;
        //public static int nRaces = 1;

        //debug2
        //public static int nAllPlayer = 20000;
        //public static int nRaces = 10;

        //debug3
        //public static int nAllPlayer = 20;
        //public static int nRaces = 10000;

        //mini
        //public static int nAllPlayer = 2000;
        //public static int nRaces = 100;

        //full
        //public static int nAllPlayer = 20000;
        //public static int nRaces = 1000;

        //publish
        public static int nAllPlayer = 20000;
        public static int nRaces = 10000;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SimulationManager sm = new SimulationManager();

            string resultFile = "result.csv";
            if (File.Exists(resultFile))
            {
                File.Delete(resultFile);
            }
            Stream stream = File.OpenWrite(resultFile);
            //sm.Simulate(20000,0, 10000, stream,false);
            //sm.Simulate(18000,0,2000,0, 500, stream);
            //sm.Simulate(18000, 2000, 0, 0, 500, stream);

            double p1 = 0.640864210621135;
            //double p2 = 1d - p1;
            int a1 = (int)(nAllPlayer * p1);
            int a2 = nAllPlayer - a1;
            sm.Simulate(a1, a2, nRaces, stream,false);

            //double p1 = 0.627536835514774;
            //double p2 = 1d - p1;
            //int a1 = (int)(nAllPlayer * p1);
            //int a2 = (int)(nAllPlayer * p2);
            //sm.Simulate(a1, a2, 500, stream);

            //double p1 = 0.4267;
            //double p2 = 0.2578;
            //double p3 = 0.2584;
            //double p4 = 1d - p1 - p2 - p3;
            //int a1 = (int)(nAllPlayer * p1);
            //int a2 = (int)(nAllPlayer * p2);
            //int a3 = (int)(nAllPlayer * p3);
            //int a4 = (int)(nAllPlayer * p4);
            //double ret = sm.Simulate(a1, a2, a3, a4, nRaces, stream);

            //double p1 = 0.460551686;
            //double p2 = 0.370453559;
            //double p3 = 1d - p1 - p2;//0.168994755

            //int a1 = (int)(nAllPlayer * p1);
            //int a2 = (int)(nAllPlayer * p2);
            //int a3 = nAllPlayer - a1 - a2;

            //double ret = sm.Simulate(a1, 0, a2, 0, a3, 0, nRaces, stream, false);

            //double p1 = 0.360461632517634;
            //double p2 = 0.381818793492776;
            //double p3 = 0.0772860805049818;
            //double p4 = 1d - p1 - p2 - p3;//,0.180433493484609

            //int a1 = (int)(nAllPlayer * p1);
            //int a2 = (int)(nAllPlayer * p2);
            //int a3 = (int)(nAllPlayer * p3);
            //int a4 = nAllPlayer - a1 - a2 -a3;

            //double ret = sm.Simulate(a1, 0, a2, 0, a3,a4, nRaces, stream,false);


            MessageBox.Show("finished");
        }

        int maxIter = 20;
        double tol = 1e-3;//1e-5;

        private void button2_Click(object sender, EventArgs e)
        {

            DoubleVector start = new DoubleVector(3);
            start[0] = -Math.Log(3d);
            start[1] = -Math.Log(3d);
            start[2] = -Math.Log(3d);

            DoubleVector dv = OptimizePowell(start, maxIter, tol);
            double p1 = f_func_minus(dv[0]);
            double p2 = f_func_minus(dv[1]);
            double p3 = f_func_minus(dv[2]);
            double p4 = 1d - p1 - p2 - p3;

            MessageBox.Show("finished:" + p1.ToString() + "," + p2.ToString() + "," + p3.ToString() + "," + p4.ToString());

            string resultFile = "resultpara.csv";
            if (File.Exists(resultFile))
            {
                File.Delete(resultFile);
            }
            Stream stream = File.OpenWrite(resultFile);
            StreamWriter sw = new StreamWriter(stream);
            sw.WriteLine(p1.ToString() + "," + p2.ToString() + "," + p3.ToString() + "," + p4.ToString());
            sw.Close();

        }

        public virtual double L(DoubleVector para)
        {
            SimulationManager sm = new SimulationManager();

            //string resultFile = "result2.csv";
            //if (File.Exists(resultFile))
            //{
            //    File.Delete(resultFile);
            //}
            //Stream stream = File.OpenWrite(resultFile);
            Stream stream = Stream.Null;

            double p1 = f_func_minus(para[0]);
            double p2 = f_func_minus(para[1]);
            double p3 = f_func_minus(para[2]);
            double p4 = 1d - p1 - p2 - p3;
            int a1 = (int)(nAllPlayer * p1);
            int a2 = (int)(nAllPlayer * p2);
            int a3 = (int)(nAllPlayer * p3);
            int a4 = (int)(nAllPlayer * p4);
            if (a4 < 0) return double.PositiveInfinity;

            double ret = sm.Simulate(a1, a2, a3, a4, nRaces, stream,false);

            return ret;
        }

        public virtual double L2(DoubleVector para)
        {
            SimulationManager sm = new SimulationManager();

            Stream stream = Stream.Null;

            double p1 = f_func_minus(para[0]);
            double p2 = 1d - p1;
            int a1 = (int)(nAllPlayer * p1);
            int a2 = (int)(nAllPlayer * p2);
            if (a2 < 0) return double.PositiveInfinity;

            double ret = sm.Simulate(a1, a2, nRaces, stream,false);

            return ret;
        }

        public virtual double L3(DoubleVector para)
        {
            SimulationManager sm = new SimulationManager();

            Stream stream = Stream.Null;

            double p1 = f_func_minus(para[0]);
            double p2 = f_func_minus(para[1]);
            double p3 = 1d - p1 - p2;
            int a1 = (int)(nAllPlayer * p1);
            int a2 = (int)(nAllPlayer * p2);
            int a3 = (int)(nAllPlayer * p3);
            if (a3 < 0) return double.PositiveInfinity;

            double ret = sm.Simulate(a1, 0,a2,0,a3,0,0, nRaces, stream,false);

            return ret;
        }

        public virtual double L3c(DoubleVector para)
        {
            SimulationManager sm = new SimulationManager();

            Stream stream = Stream.Null;

            double p1 = Math.Sin(para[0]) * Math.Sin(para[0]) * Math.Cos(para[1]) * Math.Cos(para[1]);
            double p2 = Math.Sin(para[0]) * Math.Sin(para[0]) * Math.Sin(para[1]) * Math.Sin(para[1]);
            //double p3 = 1d - p1 - p2;// Math.Cos(para[0]) * Math.Cos(para[0]);

            int a1 = (int)(nAllPlayer * p1);
            int a2 = (int)(nAllPlayer * p2);
            int a3 = nAllPlayer - a1 - a2;// (int)(nAllPlayer * p3);
            if (a3 < 0) return double.PositiveInfinity;

            double ret = sm.Simulate(a1, 0, a2, 0, a3,0,0, nRaces, stream,false);

            return ret;
        }
        public virtual double L4(DoubleVector dv)
        {
            SimulationManager sm = new SimulationManager();

            Stream stream = Stream.Null;

            double p1 = Math.Sin(dv[2]) * Math.Sin(dv[2]) * Math.Sin(dv[0]) * Math.Sin(dv[0]) * Math.Cos(dv[1]) * Math.Cos(dv[1]);
            double p2 = Math.Sin(dv[2]) * Math.Sin(dv[2]) * Math.Sin(dv[0]) * Math.Sin(dv[0]) * Math.Sin(dv[1]) * Math.Sin(dv[1]);
            double p3 = Math.Sin(dv[2]) * Math.Sin(dv[2]) * Math.Cos(dv[0]) * Math.Cos(dv[0]);
            double p4 = 1d - p1 - p2 - p3;

            int a1 = (int)(nAllPlayer * p1);
            int a2 = (int)(nAllPlayer * p2);
            int a3 = (int)(nAllPlayer * p3);
            int a4 = nAllPlayer - a1 - a2 - a3;
            if (a4 < 0) return double.PositiveInfinity;

            double ret = sm.Simulate(a1, 0, a2, 0, a3,a4,0, nRaces, stream, false);

            return ret;
        }

        public virtual double L2_2(DoubleVector para)
        {
            SimulationManager sm = new SimulationManager();

            Stream stream = Stream.Null;

            double p1 = f_func_minus(para[0]);
            double p2 = 1d - p1;
            int a1 = (int)(nAllPlayer * p1);
            int a2 = (int)(nAllPlayer * p2);
            if (a2 < 0) return double.PositiveInfinity;

            double ret = sm.Simulate(a1, a2,0,0,0,0,0, nRaces, stream, false);

            return ret;
        }

        public virtual double L2_3(DoubleVector para)
        {
            SimulationManager sm = new SimulationManager();

            Stream stream = Stream.Null;

            double p1 = f_func_minus(para[0]);
            double p2 = 1d - p1;
            int a1 = (int)(nAllPlayer * p1);
            int a2 = nAllPlayer -a1;
            if (a2 < 0) return double.PositiveInfinity;

            double ret = sm.Simulate(a1, 0, 0, 0, 0, 0,a2, nRaces, stream, false);

            return ret;
        }

        public virtual double L2_4(DoubleVector para)
        {
            SimulationManager sm = new SimulationManager();

            Stream stream = Stream.Null;

            double p1 = f_func_minus(para[0]);
            double p2 = 1d - p1;
            int a1 = (int)(nAllPlayer * p1);
            int a2 = nAllPlayer - a1;
            if (a2 < 0) return double.PositiveInfinity;

            double ret = sm.Simulate(0, 0,a1, 0, 0, 0, a2, nRaces, stream, false);

            return ret;
        }
        public virtual double L2_5(DoubleVector para)
        {
            SimulationManager sm = new SimulationManager();

            Stream stream = Stream.Null;

            double p1 = f_func_minus(para[0]);
            double p2 = 1d - p1;
            int a1 = (int)(nAllPlayer * p1);
            int a2 = nAllPlayer - a1;
            if (a2 < 0) return double.PositiveInfinity;

            double ret = sm.Simulate(0, 0, a1, 0, 0, 0,0, a2, nRaces, stream, false);

            return ret;
        }


        //protected static double f_func(double x, double y, double a, double b, double v)
        //{
        //    double f = 1d / (1d + Math.Exp((-y + b * x + a) * v));
        //    return f;
        //}
        //protected static double f_func_minus(double x, double y, double a, double b, double v)
        //{
        //    double f = 1d / (1d + Math.Exp(-(-y + b * x + a) * v));
        //    return f;
        //}
        protected static double f_func_minus(double x)
        {
            return _f_func_minus(x, 0d, 1d, 1d);
        }
        protected static double _f_func_minus(double x, double y, double b, double v)
        {
            double f = 1d / (1d + Math.Exp(-(-y + b * x) * v));
            return f;
        }


        public DoubleVector OptimizePowell(DoubleVector start, int maxIter, double tol)
        {
            //尤度関数
            NMathFunctions.DoubleVectorDoubleFunction d = new NMathFunctions.DoubleVectorDoubleFunction(L);
            MultiVariableFunction f = new MultiVariableFunction(d);

            PowellMinimizer minimizer = new PowellMinimizer(tol, maxIter);

            DoubleVector min = minimizer.Minimize(f, start);
            return min;
        }

        public DoubleVector OptimizePowell2Para(DoubleVector start, int maxIter, double tol)
        {
            //尤度関数
            NMathFunctions.DoubleVectorDoubleFunction d = new NMathFunctions.DoubleVectorDoubleFunction(L2);
            MultiVariableFunction f = new MultiVariableFunction(d);

            PowellMinimizer minimizer = new PowellMinimizer(tol, maxIter);

            DoubleVector min = minimizer.Minimize(f, start);
            return min;
        }
        public DoubleVector OptimizePowell3Para(DoubleVector start, int maxIter, double tol)
        {
            //尤度関数
            //NMathFunctions.DoubleVectorDoubleFunction d = new NMathFunctions.DoubleVectorDoubleFunction(L3);
            NMathFunctions.DoubleVectorDoubleFunction d = new NMathFunctions.DoubleVectorDoubleFunction(L3c);
            MultiVariableFunction f = new MultiVariableFunction(d);

            PowellMinimizer minimizer = new PowellMinimizer(tol, maxIter);

            DoubleVector min = minimizer.Minimize(f, start);
            return min;
        }
        public DoubleVector OptimizePowell4Para(DoubleVector start, int maxIter, double tol)
        {
            //尤度関数
            //NMathFunctions.DoubleVectorDoubleFunction d = new NMathFunctions.DoubleVectorDoubleFunction(L3);
            NMathFunctions.DoubleVectorDoubleFunction d = new NMathFunctions.DoubleVectorDoubleFunction(L4);
            MultiVariableFunction f = new MultiVariableFunction(d);

            PowellMinimizer minimizer = new PowellMinimizer(tol, maxIter);

            DoubleVector min = minimizer.Minimize(f, start);
            return min;
        }
        public DoubleVector OptimizePowell2Para2(DoubleVector start, int maxIter, double tol)
        {
            //尤度関数
            NMathFunctions.DoubleVectorDoubleFunction d = new NMathFunctions.DoubleVectorDoubleFunction(L2_2);
            MultiVariableFunction f = new MultiVariableFunction(d);

            PowellMinimizer minimizer = new PowellMinimizer(tol, maxIter);

            DoubleVector min = minimizer.Minimize(f, start);
            return min;
        }

        public DoubleVector OptimizePowell2Para3(DoubleVector start, int maxIter, double tol)
        {
            //尤度関数
            NMathFunctions.DoubleVectorDoubleFunction d = new NMathFunctions.DoubleVectorDoubleFunction(L2_3);
            MultiVariableFunction f = new MultiVariableFunction(d);

            PowellMinimizer minimizer = new PowellMinimizer(tol, maxIter);

            DoubleVector min = minimizer.Minimize(f, start);
            return min;
        }
        public DoubleVector OptimizePowell2Para4(DoubleVector start, int maxIter, double tol)
        {
            //尤度関数
            NMathFunctions.DoubleVectorDoubleFunction d = new NMathFunctions.DoubleVectorDoubleFunction(L2_4);
            MultiVariableFunction f = new MultiVariableFunction(d);

            PowellMinimizer minimizer = new PowellMinimizer(tol, maxIter);

            DoubleVector min = minimizer.Minimize(f, start);
            return min;
        }
        public DoubleVector OptimizePowell2Para5(DoubleVector start, int maxIter, double tol)
        {
            //尤度関数
            NMathFunctions.DoubleVectorDoubleFunction d = new NMathFunctions.DoubleVectorDoubleFunction(L2_5);
            MultiVariableFunction f = new MultiVariableFunction(d);

            PowellMinimizer minimizer = new PowellMinimizer(tol, maxIter);

            DoubleVector min = minimizer.Minimize(f, start);
            return min;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DoubleVector start = new DoubleVector(1);
            start[0] = 0d;

            DoubleVector dv = OptimizePowell2Para(start, maxIter, tol);
            double p1 = f_func_minus(dv[0]);
            double p2 = 1d - p1;


            MessageBox.Show("finished:" + p1.ToString() + "," + p2.ToString());

            string resultFile = "resultpara.csv";
            if (File.Exists(resultFile))
            {
                File.Delete(resultFile);
            }
            Stream stream = File.OpenWrite(resultFile);
            StreamWriter sw = new StreamWriter(stream);
            sw.WriteLine(p1.ToString() + "," + p2.ToString());
            sw.Close();

            int a1 = (int)(nAllPlayer * p1);
            int a2 = nAllPlayer - a1;
            SimulationManager sm = new SimulationManager();

            string resultFile2 = "result.csv";
            if (File.Exists(resultFile2))
            {
                File.Delete(resultFile2);
            }
            Stream stream2 = File.OpenWrite(resultFile2);
            double ret = sm.Simulate(a1, a2, nRaces, stream2, false);

            MessageBox.Show("finished");

        }

        private void button4_Click(object sender, EventArgs e)
        {
            DoubleVector start = new DoubleVector(2);
            start[0] = Math.Acos(1 / Math.Sqrt(3));
            start[1] = Math.PI / 4;

            DoubleVector dv = OptimizePowell3Para(start, maxIter, tol);
            double p1 = Math.Sin(dv[0]) * Math.Sin(dv[0]) * Math.Cos(dv[1])*Math.Cos(dv[1]);
            double p2 = Math.Sin(dv[0]) * Math.Sin(dv[0]) * Math.Sin(dv[1]) * Math.Sin(dv[1]);
            double p3 = 1d - p1 - p2;// Math.Cos(dv[0]) * Math.Cos(dv[0]);
           

            string resultFile = "resultpara.csv";
            if (File.Exists(resultFile))
            {
                File.Delete(resultFile);
            }
            Stream stream = File.OpenWrite(resultFile);
            StreamWriter sw = new StreamWriter(stream);
            sw.WriteLine(p1.ToString() + "," + p2.ToString() + "," + p3.ToString());
            sw.Close();

            int a1 = (int)(nAllPlayer * p1);
            int a2 = (int)(nAllPlayer * p2);
            int a3 = nAllPlayer - a1 - a2;
            SimulationManager sm = new SimulationManager();

            string resultFile2 = "result.csv";
            if (File.Exists(resultFile2))
            {
                File.Delete(resultFile2);
            }
            Stream stream2 = File.OpenWrite(resultFile2);
            double ret = sm.Simulate(a1,0,a2,0,a3,0,0, nRaces, stream2, false);

            MessageBox.Show("finished");

        }

        private void button5_Click(object sender, EventArgs e)
        {
            DoubleVector start = new DoubleVector(3);
            start[0] = Math.Acos(1 / Math.Sqrt(3));
            start[1] = Math.PI / 4;
            start[2] = Math.PI / 3;

            DoubleVector dv = OptimizePowell4Para(start, maxIter, tol);
            double p1 = Math.Sin(dv[2]) * Math.Sin(dv[2]) * Math.Sin(dv[0]) * Math.Sin(dv[0]) * Math.Cos(dv[1]) * Math.Cos(dv[1]);
            double p2 = Math.Sin(dv[2]) * Math.Sin(dv[2]) * Math.Sin(dv[0]) * Math.Sin(dv[0]) * Math.Sin(dv[1]) * Math.Sin(dv[1]);
            double p3 = Math.Sin(dv[2]) * Math.Sin(dv[2]) * Math.Cos(dv[0]) * Math.Cos(dv[0]);
            double p4 = 1d - p1 - p2 - p3;


            string resultFile = "resultpara.csv";
            if (File.Exists(resultFile))
            {
                File.Delete(resultFile);
            }
            Stream stream = File.OpenWrite(resultFile);
            StreamWriter sw = new StreamWriter(stream);
            sw.WriteLine(p1.ToString() + "," + p2.ToString() + "," + p3.ToString()+ "," + p4.ToString());
            sw.Close();


            int a1 = (int)(nAllPlayer * p1);
            int a2 = (int)(nAllPlayer * p2);
            int a3 = (int)(nAllPlayer * p3);
            int a4 = nAllPlayer - a1 - a2 - a3;
            SimulationManager sm = new SimulationManager();

            string resultFile2 = "result.csv";
            if (File.Exists(resultFile2))
            {
                File.Delete(resultFile2);
            }
            Stream stream2 = File.OpenWrite(resultFile2);
            double ret = sm.Simulate(a1, 0, a2, 0, a3, a4, 0, nRaces, stream2, false);
            MessageBox.Show("finished");

        }

        private void button6_Click(object sender, EventArgs e)
        {
            DoubleVector start = new DoubleVector(1);
            start[0] = 0d;

            DoubleVector dv = OptimizePowell2Para2(start, maxIter, tol);
            double p1 = f_func_minus(dv[0]);
            double p2 = 1d - p1;


            MessageBox.Show("finished:" + p1.ToString() + "," + p2.ToString());

            string resultFile = "resultpara.csv";
            if (File.Exists(resultFile))
            {
                File.Delete(resultFile);
            }
            Stream stream = File.OpenWrite(resultFile);
            StreamWriter sw = new StreamWriter(stream);
            sw.WriteLine(p1.ToString() + "," + p2.ToString());
            sw.Close();

        }

        private void button7_Click(object sender, EventArgs e)
        {
            SimulationManager sm = new SimulationManager();

            string resultFile = "result.csv";
            if (File.Exists(resultFile))
            {
                File.Delete(resultFile);
            }
            Stream stream = File.OpenWrite(resultFile);
            sm.Simulate(0,20000,0,0,0,0,0,nRaces, stream,false);

            MessageBox.Show("finished");

        }

        private void button8_Click(object sender, EventArgs e)
        {
            DoubleVector start = new DoubleVector(1);
            start[0] = 0d;

            DoubleVector dv = OptimizePowell2Para3(start, maxIter, tol);
            double p1 = f_func_minus(dv[0]);
            double p2 = 1d - p1;


            MessageBox.Show("finished:" + p1.ToString() + "," + p2.ToString());

            string resultFile = "resultpara.csv";
            if (File.Exists(resultFile))
            {
                File.Delete(resultFile);
            }
            Stream stream = File.OpenWrite(resultFile);
            StreamWriter sw = new StreamWriter(stream);
            sw.WriteLine(p1.ToString() + "," + p2.ToString());
            sw.Close();

            int a1 = (int)(nAllPlayer * p1);
            int a2 = nAllPlayer - a1;
            SimulationManager sm = new SimulationManager();

            string resultFile2 = "result.csv";
            if (File.Exists(resultFile2))
            {
                File.Delete(resultFile2);
            }
            Stream stream2 = File.OpenWrite(resultFile2);
            //sm.Simulate(0, 0, 0, 0, 0, 0, 20000, nRaces, stream, false);
            double ret = sm.Simulate(a1, 0, 0, 0, 0, 0, a2, nRaces, stream2, false);

            MessageBox.Show("finished");


        }

        private void button9_Click(object sender, EventArgs e)
        {
            SimulationManager sm = new SimulationManager();

            string resultFile = "result.csv";
            if (File.Exists(resultFile))
            {
                File.Delete(resultFile);
            }
            Stream stream = File.OpenWrite(resultFile);
            sm.Simulate(0, 0, 0, 0, 0, 0, 20000, nRaces, stream, false);

            MessageBox.Show("finished");

        }

        private void button10_Click(object sender, EventArgs e)
        {
            DoubleVector start = new DoubleVector(1);
            start[0] = 0d;

            DoubleVector dv = OptimizePowell2Para4(start, maxIter, tol);
            double p1 = f_func_minus(dv[0]);
            double p2 = 1d - p1;


            MessageBox.Show("finished:" + p1.ToString() + "," + p2.ToString());

            string resultFile = "resultpara.csv";
            if (File.Exists(resultFile))
            {
                File.Delete(resultFile);
            }
            Stream stream = File.OpenWrite(resultFile);
            StreamWriter sw = new StreamWriter(stream);
            sw.WriteLine(p1.ToString() + "," + p2.ToString());
            sw.Close();

            int a1 = (int)(nAllPlayer * p1);
            int a2 = nAllPlayer - a1;
            SimulationManager sm = new SimulationManager();

            string resultFile2 = "result.csv";
            if (File.Exists(resultFile2))
            {
                File.Delete(resultFile2);
            }
            Stream stream2 = File.OpenWrite(resultFile2);
            double ret = sm.Simulate(0, 0,a1, 0, 0, 0, a2, nRaces, stream2, false);

            MessageBox.Show("finished");

        }

        private void button11_Click(object sender, EventArgs e)
        {
            SimulationManager sm = new SimulationManager();

            Race.Dump1Nin = true;
            string ninFile = "1nin.csv";
            if (File.Exists(ninFile))
            {
                File.Delete(ninFile);
            }
            //Stream stream = File.OpenWrite(ninFile);

            
            string resultFile = "result.csv";
            if (File.Exists(resultFile))
            {
                File.Delete(resultFile);
            }
            Stream stream = File.OpenWrite(resultFile);
            sm.Simulate(0, 0, 0, 0, 0, 0, 20000, nRaces, stream, true);

            MessageBox.Show("finished");

        }

        private void button12_Click(object sender, EventArgs e)
        {
            SimulationManager sm = new SimulationManager();

            string resultFile = "result.csv";
            if (File.Exists(resultFile))
            {
                File.Delete(resultFile);
            }
            Stream stream = File.OpenWrite(resultFile);
            sm.Simulate(20000, 0, 0, 0, 0, 0, 0, nRaces, stream, false);

            MessageBox.Show("finished");

        }

        private void button13_Click(object sender, EventArgs e)
        {
            SimulationManager sm = new SimulationManager();

            for (int i = 0; i <= 10; i++)
            {
                string resultFile = "result_" +i.ToString() + ".csv";
                if (File.Exists(resultFile))
                {
                    File.Delete(resultFile);
                }
                Stream stream = File.OpenWrite(resultFile);

                float ii = (float)i / 10f;
                int a1 = (int)(nAllPlayer * ii);
                int a2 = nAllPlayer - a1;

                double ret = sm.Simulate(0, 0, a1, 0, 0, 0, a2, nRaces, stream, false);
            }
            MessageBox.Show("finished");

        }

        private void button14_Click(object sender, EventArgs e)
        {
            SimulationManager sm = new SimulationManager();

            for (int i = 0; i <= 10; i++)
            {
                string resultFile = "result_" + i.ToString() + ".csv";
                if (File.Exists(resultFile))
                {
                    File.Delete(resultFile);
                }
                Stream stream = File.OpenWrite(resultFile);

                float ii = (float)i / 10f;
                int a1 = (int)(nAllPlayer * ii);
                int a2 = nAllPlayer - a1;

                double ret = sm.Simulate(a2, 0, a1, 0, 0, 0, 0, nRaces, stream, false);
            }
            MessageBox.Show("finished");

        }

        private void button15_Click(object sender, EventArgs e)
        {
            SimulationManager sm = new SimulationManager();

            Race.Dump1Nin = true;
            string ninFile = "1nin.csv";
            if (File.Exists(ninFile))
            {
                File.Delete(ninFile);
            }
            //Stream stream = File.OpenWrite(ninFile);


            string resultFile = "result.csv";
            if (File.Exists(resultFile))
            {
                File.Delete(resultFile);
            }
            Stream stream = File.OpenWrite(resultFile);
            sm.Simulate(20000, 0, 0, 0, 0, 0,0, nRaces, stream, true);

            MessageBox.Show("finished");

        }

        private void button16_Click(object sender, EventArgs e)
        {
            SimulationManager sm = new SimulationManager();

            Race.ChangeStartTime = true;
            Race.Dump1Nin = true;
            string ninFile = "1nin.csv";
            if (File.Exists(ninFile))
            {
                File.Delete(ninFile);
            }


            string resultFile = "result.csv";
            if (File.Exists(resultFile))
            {
                File.Delete(resultFile);
            }
            Stream stream = File.OpenWrite(resultFile);
            sm.Simulate(0, 0, 0, 0, 0, 0,0, 20000, nRaces, stream, false);

            MessageBox.Show("finished");

        }

        private void button17_Click(object sender, EventArgs e)
        {
            SimulationManager sm = new SimulationManager();
            Race.ChangeStartTime = true;

            for (int i = 0; i <= 10; i++)
            {
                string resultFile = "result_" + i.ToString() + ".csv";
                if (File.Exists(resultFile))
                {
                    File.Delete(resultFile);
                }
                Stream stream = File.OpenWrite(resultFile);

                float ii = (float)i / 10f;
                int a1 = (int)(nAllPlayer * ii);
                int a2 = nAllPlayer - a1;

                double ret = sm.Simulate(0, 0, a2, 0, 0, 0, 0,a1, nRaces, stream, false);
            }
            MessageBox.Show("finished");

        }

        private void button18_Click(object sender, EventArgs e)
        {
            DoubleVector start = new DoubleVector(1);
            start[0] = 0d;

            DoubleVector dv = OptimizePowell2Para5(start, maxIter, tol);
            double p1 = f_func_minus(dv[0]);
            double p2 = 1d - p1;


            //MessageBox.Show("finished:" + p1.ToString() + "," + p2.ToString());

            string resultFile = "resultpara.csv";
            if (File.Exists(resultFile))
            {
                File.Delete(resultFile);
            }
            Stream stream = File.OpenWrite(resultFile);
            StreamWriter sw = new StreamWriter(stream);
            sw.WriteLine(p1.ToString() + "," + p2.ToString());
            sw.Close();

            int a1 = (int)(nAllPlayer * p1);
            int a2 = nAllPlayer - a1;
            SimulationManager sm = new SimulationManager();

            string resultFile2 = "result.csv";
            if (File.Exists(resultFile2))
            {
                File.Delete(resultFile2);
            }
            Stream stream2 = File.OpenWrite(resultFile2);
            double ret = sm.Simulate(0, 0, a1, 0, 0, 0,0, a2, nRaces, stream2, false);

            MessageBox.Show("finished");

        }

        private void button19_Click(object sender, EventArgs e)
        {
            SimulationManager sm = new SimulationManager();
            Race.ChangeStartTime = true;

            StreamWriter sw = new StreamWriter("param.csv");
            for (int i = 0; i <= 10; i++)
            {
                for (double alpha = 0.1d; alpha <= 1d; alpha = alpha + 0.1)
                {
                    EconomicBetaHitterPlayer.Alpha = alpha;
                    //EconomicBetaHitterPlayer.Beta = 1d;

                    string resultFile = "result_" + i.ToString() + "_" + alpha.ToString() + ".csv";
                    if (File.Exists(resultFile))
                    {
                        File.Delete(resultFile);
                    }
                    Stream stream = File.OpenWrite(resultFile);

                    float ii = (float)i / 10f;
                    int a1 = (int)(nAllPlayer * ii);
                    int a2 = nAllPlayer - a1;

                    double ret = sm.Simulate(0, 0, a1, 0, 0, 0, 0,0, a2, nRaces, stream, false);
                    sw.Write(ret.ToString() + ",");
                }
                sw.Write(sw.NewLine);
            }
            sw.Close();
            MessageBox.Show("finished");
        }

        private void button20_Click(object sender, EventArgs e)
        {
            SimulationManager sm = new SimulationManager();
            Race.ChangeStartTime = true;
            using (StreamWriter sw = new StreamWriter("param_vary.csv"))
            {
                for (double beta = 0.1d; beta <= 1d; beta = beta + 0.1)
                {
                    for (double alpha = 0.1d; alpha <= 1d; alpha = alpha + 0.1)
                    {
                        EconomicBetaHitterPlayer.Alpha = alpha;
                        BetaBuyProbRace.Beta = beta;

                        string resultFile = "result_vary_" + beta.ToString() + "_" + alpha.ToString() + ".csv";
                        if (File.Exists(resultFile))
                        {
                            File.Delete(resultFile);
                        }
                        Stream stream = File.OpenWrite(resultFile);

                        double ret = sm.Simulate(0, 0, 0, 0, 0, 0, 0, 0, nAllPlayer, nRaces, stream, false);
                        sw.Write(ret.ToString() + ",");
                    }
                    sw.Write(sw.NewLine);
                }
                sw.Close();
            }

            SimulationManager sm2 = new SimulationManager();
            Race.ChangeStartTime = false;
            using (StreamWriter sw = new StreamWriter("param_fix.csv"))
            {
                for (double beta = 0.1d; beta <= 1d; beta = beta + 0.1)
                {
                    for (double alpha = 0.1d; alpha <= 1d; alpha = alpha + 0.1)
                    {
                        EconomicBetaHitterPlayer.Alpha = alpha;
                        BetaBuyProbRace.Beta = beta;

                        string resultFile = "result_fix_" + beta.ToString() + "_" + alpha.ToString() + ".csv";
                        if (File.Exists(resultFile))
                        {
                            File.Delete(resultFile);
                        }
                        Stream stream = File.OpenWrite(resultFile);

                        double ret = sm2.Simulate(0, 0, 0, 0, 0, 0, 0, 0, nAllPlayer, nRaces, stream, false);
                        sw.Write(ret.ToString() + ",");
                    }
                    sw.Write(sw.NewLine);
                }
                sw.Close();
            }

            MessageBox.Show("finished");

        }

        private void button21_Click(object sender, EventArgs e)
        {
            SimulationManager sm = new SimulationManager();
            Race.ChangeStartTime = false;

            string resultFile = "result.csv";
            if (File.Exists(resultFile))
            {
                File.Delete(resultFile);
            }
            Stream stream = File.OpenWrite(resultFile);
            //sm.Simulate(20000,0, 10000, stream,false);
            //sm.Simulate(18000,0,2000,0, 500, stream);
            //sm.Simulate(18000, 2000, 0, 0, 500, stream);
            sm.Simulate(nAllPlayer, 0, nRaces, stream, false);


            MessageBox.Show("finished");

        }

        private void button22_Click(object sender, EventArgs e)
        {
            SimulationManager sm = new SimulationManager();
            Race.ChangeStartTime = false;

            string resultFile = "result.csv";
            if (File.Exists(resultFile))
            {
                File.Delete(resultFile);
            }
            Stream stream = File.OpenWrite(resultFile);
            sm.Simulate(0,0,0,0,0,0,0,0,0,nAllPlayer, nRaces, stream, false);

            MessageBox.Show("finished");

        }

        private void button23_Click(object sender, EventArgs e)
        {
            SimulationManager sm = new SimulationManager();
            Race.ChangeStartTime = false;

            StreamWriter sw = new StreamWriter("param.csv");
            for (int i = 0; i < 1; i++)
            {
                //for (double alpha = 0.1d; alpha <= 1d; alpha = alpha + 0.1)
                ////for (double alpha = 1d; alpha > 0.0001d; alpha = alpha *0.5d)
                for (double alpha = 0d; alpha <= 8d; alpha = alpha + 1d)
                {
                    EconomicOtokuBetaBuyPlayer.Alpha = Math.Pow(2,alpha);
                    //EconomicOtokuBetaBuyPlayer.Beta = alpha;

                    string resultFile = "result_" + i.ToString() + "_" + alpha.ToString() + ".csv";
                    if (File.Exists(resultFile))
                    {
                        File.Delete(resultFile);
                    }
                    Stream stream = File.OpenWrite(resultFile);

                    //float ii = (float)i / 10f;
                    //int a1 = (int)(nAllPlayer * ii);
                    //int a2 = nAllPlayer - a1;

                    double ret = sm.Simulate(0, 0, 0, 0, 0, 0, 0, 0, 0,0,nAllPlayer, nRaces, stream, false);
                    sw.Write(ret.ToString() + ",");
                }
                sw.Write(sw.NewLine);
            }
            sw.Close();
            MessageBox.Show("finished");

        }

        private void button24_Click(object sender, EventArgs e)
        {
            SimulationManager sm = new SimulationManager();
            Race.ChangeStartTime = false;

            StreamWriter sw = new StreamWriter("param.csv");
            for (int i = 0; i < 1; i++)
            {
                //for (double alpha = 0.1d; alpha <= 1d; alpha = alpha + 0.1)
                ////for (double alpha = 1d; alpha > 0.0001d; alpha = alpha *0.5d)
                for (double alpha = 0d; alpha <= 8d; alpha = alpha + 1d)
                {
                    EconomicBetaHitterPlayer.Alpha = Math.Pow(2, alpha);
                    //EconomicOtokuBetaBuyPlayer.Beta = alpha;

                    string resultFile = "result_" + i.ToString() + "_" + alpha.ToString() + ".csv";
                    if (File.Exists(resultFile))
                    {
                        File.Delete(resultFile);
                    }
                    Stream stream = File.OpenWrite(resultFile);

                    //float ii = (float)i / 10f;
                    //int a1 = (int)(nAllPlayer * ii);
                    //int a2 = nAllPlayer - a1;

                    double ret = sm.Simulate(0, 0, 0, 0, 0, 0, 0, 0,nAllPlayer, nRaces, stream, false);
                    sw.Write(ret.ToString() + ",");
                }
                sw.Write(sw.NewLine);
            }
            sw.Close();
            MessageBox.Show("finished");

        }

        private void button25_Click(object sender, EventArgs e)
        {
            SimulationManager sm = new SimulationManager();
            Race.ChangeStartTime = false;

            StreamWriter sw = new StreamWriter("param.csv");
            for (int i = 0; i < 1; i++)
            {
                //for (double alpha = 0.1d; alpha <= 1d; alpha = alpha + 0.1)
                ////for (double alpha = 1d; alpha > 0.0001d; alpha = alpha *0.5d)
                for (double alpha = 0d; alpha <= 8d; alpha = alpha + 1d)
                {
                    EconomicOtokuBetaBuyPlayer.Alpha = Math.Pow(2, alpha);
                    //EconomicOtokuBetaBuyPlayer.Beta = alpha;

                    string resultFile = "result_" + i.ToString() + "_" + alpha.ToString() + ".csv";
                    if (File.Exists(resultFile))
                    {
                        File.Delete(resultFile);
                    }
                    Stream stream = File.OpenWrite(resultFile);

                    //float ii = (float)i / 10f;
                    //int a1 = (int)(nAllPlayer * ii);
                    //int a2 = nAllPlayer - a1;

                    double ret = sm.Simulate(0, 0, 0, 0, 0, 0, 0, 0, 0, 0,0, nAllPlayer, nRaces, stream, false);
                    sw.Write(ret.ToString() + ",");
                }
                sw.Write(sw.NewLine);
            }
            sw.Close();
            MessageBox.Show("finished");

        }

//        double[] expectedVote = // むりやりMK4K１６頭データから１４頭データを正規化して作った
//        {
//0.3716114,
//0.182826999,
//0.129657902,
//0.092704583,
//0.061589855,
//0.046777042,
//0.03300284,
//0.026371183,
//0.016480133,
//0.012274209,
//0.009300953,
//0.008401947,
//0.006128689,
//0.002872266,            
//        };
        double[] expectedVote = // 160222 土谷によりトイモデルで算出した。
        {
0.278369576,
0.201733928,
0.146196212,
0.105948131,
0.076780418,
0.05564263,
0.040324114,
0.029222812,
0.021177719,
0.015347455,
0.011122273,
0.008060291,
0.005841278,
0.004233164,
    };
        private void button26_Click(object sender, EventArgs e)
        {
            double[] eO = new double[14];
            for (int i = 0; i < 14; i++)
            {
                eO[i] = 0.75d / expectedVote[i];
            }

            Race.nHorses = 14;
            EconomicAllOtokuBekiBuyPlayer.SetExpectedVoteOdds(expectedVote, eO);
            DoEconomicAllOtokuBekiBuyPlayer();
        }

        private void DoEconomicAllOtokuBekiBuyPlayer()
        {
            SimulationManager sm = new SimulationManager();
            Race.ChangeStartTime = false;

            StreamWriter sw = new StreamWriter("param.csv");

            double[] alphaList = {
                                    0d,
                                    0.5d,
                                    1d,
                                    5d,
                                    10d,
                                    100d,

                                 };
            for (int i = 0; i < alphaList.Length; i++)
            {
                EconomicAllOtokuBekiBuyPlayer.Alpha = alphaList[i];

                string resultFile = "result_" + alphaList[i].ToString() + ".csv";
                if (File.Exists(resultFile))
                {
                    File.Delete(resultFile);
                }
                Stream stream = File.OpenWrite(resultFile);

                double ret = sm.Simulate(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, nAllPlayer, nRaces, stream, false);
                sw.Write(ret.ToString() + ",");
            }
            sw.Write(sw.NewLine);

            sw.Close();
            MessageBox.Show("finished");
        }

        private void button27_Click(object sender, EventArgs e)
        {
            double[] eV = new double[14];
            double[] eO = new double[14];

            for (int i = 0; i < 14; i++)
            {
                eV[i] = 1d / 14d;
            }
            eV[0] += 0.001d * 7d;
            eV[1] += 0.001d * 6d;
            eV[2] += 0.001d * 5d;
            eV[3] += 0.001d * 4d;
            eV[4] += 0.001d * 3d;
            eV[5] += 0.001d * 2d;
            eV[6] += 0.001d * 1d;

            eV[13] -= 0.001d * 7d;
            eV[12] -= 0.001d * 6d;
            eV[11] -= 0.001d * 5d;
            eV[10] -= 0.001d * 4d;
            eV[9] -= 0.001d * 3d;
            eV[8] -= 0.001d * 2d;
            eV[7] -= 0.001d * 1d;


            for (int i = 0; i < 14; i++)
            {
                eO[i] = 0.75d / eV[i];
            }

            Race.nHorses = 14;
            EconomicAllOtokuBekiBuyPlayer.SetExpectedVoteOdds(eV, eO);
            DoEconomicAllOtokuBekiBuyPlayer();

        }

        private void button28_Click(object sender, EventArgs e)
        {
            double[] eV = new double[14];
            double[] eO = new double[14];

            for (int i = 0; i < 14; i++)
            {
                eV[i] = 1d / 14d;
            }

            for (int i = 0; i < 14; i++)
            {
                eO[i] = 0.75d / eV[i];
            }

            Race.nHorses = 14;
            EconomicAllOtokuBekiBuyPlayer.SetExpectedVoteOdds(eV, eO);
            DoEconomicAllOtokuBekiBuyPlayer();

        }

        private void button29_Click(object sender, EventArgs e)
        {
            double[] eV = new double[14];
            double[] eO = new double[14];

            for (int i = 0; i < 14; i++)
            {
                eV[i] = 1d / 14d;
            }

            for (int i = 0; i < 14; i++)
            {
                eO[i] = 0.75d / eV[i];
            }

            Race.nHorses = 14;
            EconomicAllOtokuBekiBuyPlayer.SetExpectedVoteOdds(eV, eO);


            SimulationManager sm = new SimulationManager();
            Race.ChangeStartTime = false;

            StreamWriter sw = new StreamWriter("param.csv");

            EconomicAllOtokuBekiBuyPlayer.Alpha = 100d;

            string resultFile = "result_100test.csv";
            if (File.Exists(resultFile))
            {
                File.Delete(resultFile);
            }
            Stream stream = File.OpenWrite(resultFile);

            double ret = sm.Simulate(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10, 10, stream, false);
            sw.Write(ret.ToString() + ",");

            sw.Write(sw.NewLine);

            sw.Close();
            MessageBox.Show("finished");

        }

        private void button30_Click(object sender, EventArgs e)
        {
            double[] eO = new double[14];
            for (int i = 0; i < 14; i++)
            {
                eO[i] = 0.75d / expectedVote[i];
            }

            Race.nHorses = 14;
            EconomicAllOtokuBekiBuyPlayer.SetExpectedVoteOdds(expectedVote, eO);


            DoubleVector start = new DoubleVector(1);
            start[0] = 5d;

            DoubleVector dv = OptimizePowell_a(start, maxIter, tol);


            string resultFile = "resultpara.csv";
            if (File.Exists(resultFile))
            {
                File.Delete(resultFile);
            }
            Stream stream = File.OpenWrite(resultFile);
            StreamWriter sw = new StreamWriter(stream);
            sw.WriteLine(dv[0].ToString());
            sw.Close();

            string resultFile2 = "result_" + dv[0].ToString() + ".csv";
            if (File.Exists(resultFile))
            {
                File.Delete(resultFile);
            }
            SimulationManager sm = new SimulationManager();
            Race.ChangeStartTime = false;
            Stream stream2 = File.OpenWrite(resultFile);
            EconomicAllOtokuBekiBuyPlayer.Alpha = dv[0];
            double ret = sm.Simulate(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, nAllPlayer, nRaces, stream2, false);

            MessageBox.Show("finished: a = " + dv[0].ToString());

        }

        public DoubleVector OptimizePowell_a(DoubleVector start, int maxIter, double tol)
        {
            //尤度関数
            NMathFunctions.DoubleVectorDoubleFunction d = new NMathFunctions.DoubleVectorDoubleFunction(L_a);
            MultiVariableFunction f = new MultiVariableFunction(d);

            PowellMinimizer minimizer = new PowellMinimizer(tol, maxIter);

            DoubleVector min = minimizer.Minimize(f, start);
            return min;
        }

        public virtual double L_a(DoubleVector para)
        {
            Stream stream = Stream.Null;

            SimulationManager sm = new SimulationManager();
            Race.ChangeStartTime = false;

            EconomicAllOtokuBekiBuyPlayer.Alpha = para[0];
            double ret = sm.Simulate(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, nAllPlayer, nRaces, stream, false);
            return ret;
        }

        private void button31_Click(object sender, EventArgs e)
        {
            List<string> invLabelList = new List<string>();
            List<double[]> invList = new List<double[]>();

            invLabelList.Add("0.35lh");
            invList.Add(new double[]
            {// 0.35 start last high
0.35,
0.368862616,
0.388741799,
0.409692334,
0.43177196,
0.455041528,
0.479565167,
0.505410463,
0.532648645,
0.561354779,
0.591607978,
0.623491619,
0.65709357,
0.692506438,
0.729827819,
0.769160567,
0.810613083,
0.854299607,
0.900340537,
0.94886276,
1,
            }
            );
            invLabelList.Add("0.35li");
            invList.Add(new double[]
            {// 0.35 start linear
0.35,
0.3825,
0.415,
0.4475,
0.48,
0.5125,
0.545,
0.5775,
0.61,
0.6425,
0.675,
0.7075,
0.74,
0.7725,
0.805,
0.8375,
0.87,
0.9025,
0.935,
0.9675,
1,
            }
            );
            invLabelList.Add("0.35fh");
            invList.Add(new double[]
            {// 0.35 start first high
0.35,
0.40113724,
0.449659463,
0.495700393,
0.539386917,
0.580839433,
0.620172181,
0.657493562,
0.69290643,
0.726508381,
0.758392022,
0.788645221,
0.817351355,
0.844589537,
0.870434833,
0.894958472,
0.91822804,
0.940307666,
0.961258201,
0.981137384,
1,
            }
            );
            invLabelList.Add("0.01lh");
            invList.Add(new double[]
            {// 0.01 start last high
0.01,
0.012589254,
0.015848932,
0.019952623,
0.025118864,
0.031622777,
0.039810717,
0.050118723,
0.063095734,
0.079432823,
0.1,
0.125892541,
0.158489319,
0.199526231,
0.251188643,
0.316227766,
0.398107171,
0.501187234,
0.630957344,
0.794328235,
1,
            }
            );
            invLabelList.Add("0.01li");
            invList.Add(new double[]
            {// 0.01 start linear
0.01,
0.0595,
0.109,
0.1585,
0.208,
0.2575,
0.307,
0.3565,
0.406,
0.4555,
0.505,
0.5545,
0.604,
0.6535,
0.703,
0.7525,
0.802,
0.8515,
0.901,
0.9505,
1,
            }
            );
            invLabelList.Add("0.01fh");
            invList.Add(new double[]
            {// 0.01 start first high
0.01,
0.215671765,
0.379042656,
0.508812766,
0.611892829,
0.693772234,
0.758811357,
0.810473769,
0.851510681,
0.884107459,
0.91,
0.930567177,
0.946904266,
0.959881277,
0.970189283,
0.978377223,
0.984881136,
0.990047377,
0.994151068,
0.997410746,
1,
            }
            );

            ///////////////////////////////

            List<string> exVoteLabelList = new List<string>();
            List<double[]> exVoteList = new List<double[]>();

            exVoteLabelList.Add("-0.05");
            exVoteList.Add(new double[]
            {//-0.05	
0.096879523,
0.092154653,
0.087660217,
0.083384978,
0.079318245,
0.075449848,
0.071770116,
0.068269846,
0.064940286,
0.061773111,
0.058760401,
0.055894622,
0.053168609,
0.050575546,
            }
            );
            exVoteLabelList.Add("-0.1");
            exVoteList.Add(new double[]
            {//-0.1
0.126310325,
0.114290308,
0.103414147,
0.09357299,
0.084668343,
0.076611084,
0.069320576,
0.062723851,
0.056754887,
0.051353946,
0.046466972,
0.042045055,
0.038043939,
0.034423579,
            }
            );
            exVoteLabelList.Add("-0.3");
            exVoteList.Add(new double[]
            {//	-0.3	
0.263127528,
0.194929667,
0.144407449,
0.10697967,
0.079252489,
0.058711688,
0.043494688,
0.032221657,
0.023870391,
0.01768362,
0.013100348,
0.009704977,
0.007189624,
0.005326204,
            }
            );
            exVoteLabelList.Add("-0.5");
            exVoteList.Add(new double[]
            {//-0.5
0.393828465,
0.238869039,
0.144881396,
0.087875009,
0.053298887,
0.032327409,
0.019607565,
0.011892589,
0.00721322,
0.004375039,
0.002653595,
0.001609487,
0.000976203,
0.000592097,
            }
            );
            exVoteLabelList.Add("-1.3");
            exVoteList.Add(new double[]
            {//	-1.3
0.727468216,
0.198258217,
0.054031667,
0.014725347,
0.004013125,
0.001093704,
0.000298069,
8.12333E-05,
2.21387E-05,
6.03349E-06,
1.64432E-06,
4.48129E-07,
1.22129E-07,
3.32841E-08,
            }
            );

            //StreamWriter sw = new StreamWriter("param.csv");

            string[] alphaLabelList = {
                                    "1_400",
                                     "1_54",
                                     "1_20",
                                     "1_7",
                                     "1_4",
                                     "1_2",
                                     "1",
                                     "2",
                                     "4",
                                     "7",
                                     "20",
                                     "54",
                                     "400",
                                      };
            double[] alphaList = {
                                    1d/400d,
                                     1d/54d,
                                     1d/20d,
                                     1d/7d,
                                     1d/4d,
                                     1d/2d,
                                     1d,
                                     2d,
                                     4d,
                                     7d,
                                     20d,
                                     54d,
                                     400d,
                                 };
            Race.nHorses = 14;
            Race.ChangeStartTime = false;
            SimulationManager.OutputExpectedVote = false;


            string resultFile = "result_" + DateTime.Now.Ticks.ToString()+" .csv";
            //if (File.Exists(resultFile))
            //{
            //    File.Delete(resultFile);
            //}
            StreamWriter stw = new StreamWriter(resultFile);
            stw.Write("invest");
            stw.Write(",");
            stw.Write("exVote");
            stw.Write(stw.NewLine);

            for (int m = 0; m < invList.Count; m++)
            {
                AnyInvest_pHitStrictRace.InvestRatio = invList[m];
                for (int k = 0; k < exVoteList.Count; k++)
                {
                    stw.Write(invLabelList[m]);
                    stw.Write(",");
                    stw.Write(exVoteLabelList[k]);
                    //stw.Write(",");
                    //stw.Write(alphaLabelList[i]);
                    stw.Write(stw.NewLine);

                    double[] ex = exVoteList[k];
                    double[] eO = new double[14];
                    for (int j = 0; j < 14; j++)
                    {
                        eO[j] = 0.75d / ex[j];
                    }

                    EconomicAllOtokuBekiBuyPlayer.SetExpectedVoteOdds(ex, eO);
                    stw.WriteLine(",1,2,3,4,5,6,7,8,9,10,11,12,13,14");
                    stw.Write("exVote,");
                    for (int ii = 0; ii < EconomicPlayer.expectedVote.Length; ii++)
                    {
                        stw.Write(EconomicPlayer.expectedVote[ii].ToString() + ",");
                    }
                    stw.Write(stw.NewLine);


                    for (int i = 0; i < alphaList.Length; i++)
                    {
                        SimulationManager sm = new SimulationManager(stw);

                        EconomicAllOtokuBekiBuyPlayer.Alpha = alphaList[i];

                        stw.Write(alphaLabelList[i]);
                        stw.Write(",");

                        Stream stream = null;
                        double ret = sm.Simulate(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, nAllPlayer, nRaces, stream, false);
                        //sw.Write(ret.ToString() + ",");
                    }

                }
            }
            stw.Close();

            //sw.Write(sw.NewLine);
            //sw.Close();
            MessageBox.Show("finished");

        }

    }
}
