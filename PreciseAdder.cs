using System;
using System.Collections.Generic;
using System.Text;

namespace EconomicSimulation
{
    //public class PreciseAdder
    //{
    //    private double r, t, s;
    //    public PreciseAdder()
    //    {
    //        r = t = s = 0d;
    //    }
    //    public void Add(double x)
    //    {
    //        r = r + x;
    //        t = s;
    //        s = s + r;
    //        t = s - t;
    //        r = r - t;
    //    }
    //    public double Sum
    //    {
    //        get { return s; }
    //    }
    //}
    public class AddedDouble
    {
        private double r, t, s;
        public AddedDouble()
        {
            r = t = s = 0d;
        }
        public AddedDouble(double initialValue) :this()
        {
            s = initialValue;
        }
        public virtual void Add(double value)
        {
            r = r + value;
            t = s;
            s = s + r;
            t = s - t;
            r = r - t;
        }
        public virtual double Value
        {
            get { return s; }
        }

    }
    public class AddedDoubleBinary : AddedDouble
    {
        List<double> list;
        public AddedDoubleBinary()
        {
            list = new List<double>();
        }
        public AddedDoubleBinary(double initialValue)
            : this()
        {
            list.Add(initialValue);
        }
        public override void Add(double value)
        {
            list.Add(value);
        }
        public override double Value
        {
            get {
                if (list.Count == 0) return 0d;
                while (list.Count > 1)
                {
                    int end = list.Count;
                    int half = list.Count / 2;
                    for (int i = 0; i < half; i++)
                    {
                        int j = end - 1 - i;
                        list[i] += list[j];
                        list.RemoveAt(j);
                    }
                }
                return list[0]; 
            }
        }

    }

    public class AddedDoubleBinaryPrecise : AddedDouble
    {
        List<AddedDouble> list;
        public AddedDoubleBinaryPrecise()
        {
            list = new List<AddedDouble>();
        }
        public AddedDoubleBinaryPrecise(double initialValue)
            : this()
        {
            list.Add(new AddedDouble(initialValue));
        }
        public override void Add(double value)
        {
            list.Add(new AddedDouble(value));
        }
        public override double Value
        {
            get
            {
                if (list.Count == 0) return 0d;
                while (list.Count > 1)
                {
                    int end = list.Count;
                    int half = list.Count / 2;
                    for (int i = 0; i < half; i++)
                    {
                        int j = end - 1 - i;
                        list[i].Add(list[j].Value);
                        list.RemoveAt(j);
                    }
                }
                return list[0].Value;
            }
        }

    }

}
