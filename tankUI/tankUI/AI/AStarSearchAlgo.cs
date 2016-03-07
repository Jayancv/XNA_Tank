using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;

namespace tankUI.AI
{
    class AStarSearchAlgo
   {
      // contains cells that have already been examined. 
        public Dictionary<Cell, Cell> cameFrom = new Dictionary<Cell, Cell>();

        // contains cost for each cell that have already been examined. 
        public Dictionary<Cell, int> costSoFar = new Dictionary<Cell, int>();

        // Note: a generic version of A* would abstract over Cell and
        // also Heuristic
        static public int Heuristic(Cell a, Cell b)
        {
            return Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y);
        }

        public AStarSearchAlgo(Grid graph, Cell start, Cell goal)
        {
            //  Priority Queue which contains cells that are candidates for examining, lowest priority to the node with the lowest f value
            var frontier = new PQueue<Cell>();
            frontier.Enqueue(start, 0);

            cameFrom[start] = start;
            costSoFar[start] = 0;

            while (frontier.Count > 0)
            {
                var current = frontier.Dequeue();

                // Exit the search if goal have discovered
                if (current.Equals(goal)) { break; }

                // discovers the neighbours
                foreach (var next in graph.Neighbors(current))
                {
                    int newCost = costSoFar[current] + graph.Cost(current, next);

                    if (!costSoFar.ContainsKey(next) || newCost < costSoFar[next])
                    {
                        costSoFar[next] = newCost;

                        // f = g + h
                        int priority = newCost + Heuristic(next, goal);

                        frontier.Enqueue(next, priority);
                        cameFrom[next] = current;
                    }
                }
            }
        }


    }
}
