using System;

public struct GridPosition : IEquatable<GridPosition>
{
    public int x;
    public int y;

    public GridPosition(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
    public override string ToString()
    {
        return x + ", " + y;
    }

    public bool Equals(GridPosition other)
    {
        return x == other.x && y == other.y;
    }
    public override bool Equals(object obj)
    {
        if (obj is GridPosition)
        {
            return Equals((GridPosition)obj);
        }
        return false;
    }
    public override int GetHashCode()
    {
        return x ^ y;
    }
    public static bool operator ==(GridPosition a, GridPosition b)
    {
        return a.Equals(b);
    }
    public static bool operator !=(GridPosition a, GridPosition b)
    {
        return !a.Equals(b);
    }
    public static GridPosition operator +(GridPosition a, GridPosition b)
    {
        return new GridPosition(a.x + b.x, a.y + b.y);
    }
}
