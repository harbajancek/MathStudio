using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;

namespace MathStudioWpf
{
    public readonly struct Fraction : IComparable<double>, IComparable<Fraction>, IEquatable<double>, IEquatable<Fraction>, IFormattable
    {
        public readonly long Numerator { get; }
        public readonly long Denominator { get; }
        public readonly double RealValue
        {
            get
            {
                if (IsPi)
                {
                    return Math.PI * (((double)Numerator) / ((double)Denominator));
                }
                else
                {
                    return (((double)Numerator) / ((double)Denominator));
                }
            }
        }
        public readonly bool IsPi { get; }

        public Fraction(long numerator, long denominator, bool isPi = false)
        {
            IsPi = isPi;
            Numerator = numerator;
            Denominator = denominator;
            if (numerator == double.NaN || denominator == double.NaN)
            {
                throw new ArgumentException("Arguments can't be NaN");
            }

            if ((Numerator < 0 && Denominator < 0) || (Numerator > 0 && Denominator < 0))
            {
                Numerator = -Numerator;
                Denominator = -Denominator;
            }

            if ((Numerator % 1) == 0 && (Denominator % 1) == 0)
            {
                var divisor = GCD(Math.Abs(Numerator), Denominator);
                Numerator /= divisor;
                Denominator /= divisor;
            }
        }

        public Fraction(double number, bool isPi = false)
        {
            IsPi = isPi;

            if (number == 0)
            {
                Numerator = 0;
                Denominator = 1;
            }
            else
            {
                Denominator = 1;
                while (number % 1 != 0)
                {
                    number *= 10;
                    Denominator *= 10;
                }
                Numerator = (long)number;
            }

            if ((Numerator < 0 && Denominator < 0) || (Numerator > 0 && Denominator < 0))
            {
                Numerator = -Numerator;
                Denominator = -Denominator;
            }
        }

        public Fraction ConvertToPi()
        {
            return new Fraction((Numerator / Math.PI) / Denominator, true);
        }

        // Euclidean algorithm, https://stackoverflow.com/questions/18541832/c-sharp-find-the-greatest-common-divisor
        private static long GCD(long a, long b)
        {
            while (a != 0 && b != 0)
            {
                if (a > b)
                    a %= b;
                else
                    b %= a;
            }

            return a == 0 ? b : a;
        }
        public int CompareTo([AllowNull] double other)
        {
            return RealValue.CompareTo(other);
        }
        public int CompareTo([AllowNull] Fraction other)
        {
            return RealValue.CompareTo(other.RealValue);
        }
        public bool Equals([AllowNull] double other)
        {
            return RealValue.Equals(other);
        }
        public bool Equals([AllowNull] Fraction other)
        {
            return RealValue.Equals(other.RealValue);
        }
        public override bool Equals([AllowNull] object obj)
        {
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            string numerator;
            if (IsPi)
            {
                if (Numerator == 1)
                {
                    numerator = "𝜋";
                }
                else if (Numerator == -1)
                {
                    numerator = "-𝜋";
                }
                else if (Numerator == 0)
                {
                    numerator = "0";
                }
                else
                {
                    numerator = $"{(Numerator).ToString(format, formatProvider)}𝜋";
                }
            }
            else
            {
                numerator = Numerator.ToString(format, formatProvider);
            }

            if (Denominator == 1 || Numerator == 0)
            {
                return numerator;
            }
            else
            {
                return $"{numerator}/{Denominator.ToString(format, formatProvider)}";
            }

        }
        public string ToString(string format)
        {
            string numerator;
            if (IsPi)
            {
                if (Numerator == 1)
                {
                    numerator = "𝜋";
                }
                else if (Numerator == -1)
                {
                    numerator = "-𝜋";
                }
                else if (Numerator == 0)
                {
                    numerator = "0";
                }
                else
                {
                    numerator = $"{(Numerator).ToString(format)}𝜋";
                }
            }
            else
            {
                numerator = Numerator.ToString(format);
            }

            if (Denominator == 1 || Numerator == 0)
            {
                return numerator;
            }
            else
            {
                return $"{numerator}/{Denominator.ToString(format)}";
            }
        }
        public string ToString(IFormatProvider formatProvider)
        {
            string numerator;
            if (IsPi)
            {
                if (Numerator == 1)
                {
                    numerator = "𝜋";
                }
                else if (Numerator == -1)
                {
                    numerator = "-𝜋";
                }
                else if (Numerator == 0)
                {
                    numerator = "0";
                }
                else
                {
                    numerator = $"{(Numerator).ToString(formatProvider)}𝜋";
                }
            }
            else
            {
                numerator = Numerator.ToString(formatProvider);
            }

            if (Denominator == 1 || Numerator == 0)
            {
                return numerator;
            }
            else
            {
                return $"{numerator}/{Denominator.ToString(formatProvider)}";
            }

        }

        public static Fraction operator +(Fraction a, Fraction b)
        {
            long divisor = GCD(a.Denominator, b.Denominator);

            long numerator = a.Numerator * (b.Denominator / divisor) + b.Numerator * (a.Denominator / divisor);
            long denominator = a.Denominator * (b.Denominator / divisor);

            bool isPi = false;
            if (a.IsPi && b.IsPi)
            {
                isPi = true;
            }
            return new Fraction(numerator, denominator, isPi);
        }
        public static Fraction operator *(Fraction a, Fraction b)
        {
            return new Fraction(a.Numerator * b.Numerator, a.Denominator * b.Denominator, (a.IsPi || b.IsPi) ? true : false);
        }
        public static Fraction operator -(Fraction a)
        {
            return new Fraction(-a.Numerator, a.Denominator, a.IsPi);
        }
        public static Fraction operator /(Fraction a, Fraction b)
        {
            return a * new Fraction(b.Denominator, b.Numerator, (a.IsPi || b.IsPi) ? true : false);
        }
        public static Fraction operator -(Fraction a, Fraction b)
        {
            return a + -b;
        }
        public static Fraction operator +(Fraction a, double b)
        {
            return a + new Fraction(b);
        }
        public static Fraction operator *(Fraction a, double b)
        {
            return a * new Fraction(b);
        }
        public static Fraction operator /(Fraction a, double b)
        {
            return a / new Fraction(b);
        }
        public static Fraction operator -(Fraction a, double b)
        {
            return a - new Fraction(b);
        }
        public static bool operator ==(Fraction a, Fraction b)
        {
            return a.Equals(b);
        }
        public static bool operator !=(Fraction a, Fraction b)
        {
            return !a.Equals(b);
        }
        public static bool operator >(Fraction a, Fraction b)
        {
            return a.CompareTo(b) == 1;
        }
        public static bool operator <(Fraction a, Fraction b)
        {
            return a.CompareTo(b) == -1;
        }
        public static bool operator ==(Fraction a, double b)
        {
            return a.Equals(b);
        }
        public static bool operator !=(Fraction a, double b)
        {
            return !a.Equals(b);
        }
        public static bool operator >(Fraction a, double b)
        {
            return a.CompareTo(b) == 1;
        }
        public static bool operator <(Fraction a, double b)
        {
            return a.CompareTo(b) == -1;
        }

    }
}
