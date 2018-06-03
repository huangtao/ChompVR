public struct Point
{
    public int X { get; set; }


    public int Z { get; set; }


    public Point(int x, int z)
    {
        X = x;
        Z = z;
    }

    #region Overloaded Operators
    public static Point operator +(Point p1, Point p2)
    {
        return new Point(p1.X + p2.X, p1.Z + p2.Z);
    }

    public static Point operator ++(Point p)
    {
        return new Point(p.X + 1, p.Z + 1);
    }

    public static Point operator -(Point p1, Point p2)
    {
        return new Point(p1.X - p2.X, p1.Z - p2.Z);
    }

    public static Point operator --(Point p)
    {
        return new Point(p.X - 1, p.Z - 1);
    }

    public static Point operator *(Point p1, Point p2)
    {
        return new Point(p1.X * p2.X, p1.Z * p2.Z);
    }

    public static Point operator *(Point p, int d)
    {
        return new Point(p.X * d, p.Z * d);
    }

    public static Point operator /(Point p1, Point p2)
    {
        return new Point(p1.X / p2.X, p1.Z / p2.Z);
    }

    public static Point operator /(Point p, int d)
    {
        return new Point(p.X / d, p.Z / d);
    }

    public static Point operator ^(Point p1, Point p2)
    {
        return new Point(p1.X ^ p2.X, p1.Z ^ p2.Z);
    }

    public static Point operator ^(Point p, int d)
    {
        return new Point(p.X ^ d, p.Z ^ d);
    }

    public static bool operator <(Point p1, Point p2)
    {
        return (p1.X < p2.X) && (p1.Z < p2.Z);
    }

    public static bool operator >(Point p1, Point p2)
    {
        return (p1.X > p2.X) && (p1.Z > p2.Z);
    }

    public static bool operator <=(Point p1, Point p2)
    {
        return (p1.X <= p2.X) && (p1.Z <= p2.Z);
    }

    public static bool operator >=(Point p1, Point p2)
    {
        return (p1.X >= p2.X) && (p1.Z >= p2.Z);
    }

    public static bool operator ==(Point p1, Point p2)
    {
        return (p1.X == p2.X) && (p1.Z == p2.Z);
    }

    public static bool operator !=(Point p1, Point p2)
    {
        return (p1.X != p2.X) && (p1.Z != p2.Z);
    }

    public bool Equals(Point p)
    {
        if ((object)p == null)
        {
            return false;
        }

        return (X == p.X) && (Z == p.Z);
    }

    public override bool Equals(object obj)
    {
        if (obj == null)
        {
            return false;
        }

        Point p = (Point)obj;

        if ((object)p == null)
        {
            return false;
        }

        return (X == p.X) && (Z == p.Z);
    }

    public override int GetHashCode()
    {
        return X ^ Z;
    }

    public override string ToString()
    {
        return string.Format("({0}, {1})", X, Z);
    }
    #endregion
}
