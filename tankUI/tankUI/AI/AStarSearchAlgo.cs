using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace tankUI.AI
{
    class AStarSearchAlgo
    {
        public Dictionary<Point, Point> cameFrom
        = new Dictionary<Point, Point>();
    public Dictionary<Point, int> costSoFar
        = new Dictionary<Point, int>();

    // Note: a generic version of A* would abstract over Point and
    // also Heuristic
    static public int Heuristic(Point a, Point b)
    {
        return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
    }

    public AStarSearchAlgo(WeightedGraph<Point> graph, Point start, Point goal)
    {
        var frontier = new PQueue<Point>();
        frontier.Enqueue(start, 0);

        cameFrom[start] = start;
        costSoFar[start] = 0;

        while (frontier.Count > 0)
        {
            var current = frontier.Dequeue();

            if (current.Equals(goal))
            {
                break;
            }

            foreach (var next in graph.Neighbors(current))
            {
                int newCost = costSoFar[current]
                    + graph.Cost(current, next);
                if (!costSoFar.ContainsKey(next)
                    || newCost < costSoFar[next])
                {
                    costSoFar[next] = newCost;
                    int priority = newCost + Heuristic(next, goal);
                    frontier.Enqueue(next, priority);
                    cameFrom[next] = current;
                }
            }
        }
    }
    }
}
