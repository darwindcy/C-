using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fractals
{
    class Complex
    {
        public double Real { get; set; }
        public double Imag { get; set; }
        public Complex()
        {
            Real = 0;
            Imag = 0; 
        }
        public Complex(double x, double y)
        {
            Real = x;
            Imag = y;
        }
        public static Complex operator +(Complex x, Complex y)
        {
            Complex cTmp = new Complex();
            cTmp.Real = x.Real + y.Real;
            cTmp.Imag = x.Imag + y.Imag;
            return cTmp;
        }
        public static Complex operator *(Complex x, Complex y)
        {
            Complex cTmp = new Complex();
            cTmp.Real = x.Real * y.Real - x.Imag * y.Imag;
            cTmp.Imag = x.Imag * y.Real + y.Imag * x.Real;
            return cTmp;
        }
        public static Complex operator /(Complex x, Complex y)
        {
            Complex cTmp = new Complex();
            cTmp.Real = x.Real * y.Real - x.Imag * y.Imag;
            cTmp.Imag = x.Imag * y.Real + y.Imag * x.Real;
            return cTmp;
        }
        public Complex multWhole(int a)
        {
            Complex cTmp = new Complex();
            cTmp.Real = this.Real * a;
            cTmp.Imag = this.Imag * a;
            return cTmp;
        }
        public Complex addWhole(int a)
        {
            Complex cTmp = new Complex();
            cTmp.Real = this.Real + a;
            cTmp.Imag = this.Imag;
            return cTmp;
        }

        public double Mag()
        {
            return Math.Sqrt(Real * Real + Imag * Imag);
        }
    }
}
