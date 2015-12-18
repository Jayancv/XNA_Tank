using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace tankUI.AI
{
    public interface WeightedGraph<L>
    {
        int Cost(Point a, Point b);
        IEnumerable<Point> Neighbors(Point id);
    }
    class GraphGrid : WeightedGraph<Point>
    {
        public static readonly Point[] Direction = new []
        {
            new Point(1, 0),
            new Point(0, -1),
            new Point(-1, 0),
            new Point(0, 1)
        };

        public static int width = 20, height=20;
        public HashSet<Point> stones = new HashSet<Point>();
        public HashSet<Point> water = new HashSet<Point>();
        public HashSet<Point> bricks = new HashSet<Point>();

        public bool InBounds(Point id)      //within the grid
        {
            return 0 <= id.X && id.X < width
                && 0 <= id.Y && id.Y < height;
        }

        public bool Passable(Point id)              //can go through the block
        {
            return !(stones.Contains(id) || water.Contains(id) || bricks.Contains(id));
        }

        public int Cost(Point a, Point b)
        {
            return (stones.Contains(b) || water.Contains(b) || bricks.Contains(b)) ? 5 : 1;
        }
    
        public IEnumerable<Point> Neighbors(Point id)
        {
            foreach (var dir in Direction) 
            {
                Point next = new Point(id.X + dir.X, id.Y + dir.Y);
                if (InBounds(next) && Passable(next))
                {
                    yield return next;
                }
            }
        }

    }
}
