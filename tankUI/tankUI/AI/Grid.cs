using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tankUI.AI
{
    class Grid : GraphGrid<Cell>
    {
        public int width, height;
        public HashSet<Cell> brickWalls = new HashSet<Cell>();
        public HashSet<Cell> stone = new HashSet<Cell>();
        public HashSet<Cell> water = new HashSet<Cell>();

        public HashSet<Cell> coins = new HashSet<Cell>();
        public HashSet<Cell> lifePacks = new HashSet<Cell>();


        //Square grid only allows 4 directions of movement
        public static readonly Cell[] MOVABLE_DIRECTIONS = new[]
        {
            new Cell(1, 0), // right
            new Cell(0, -1), // up
            new Cell(-1, 0), // left
            new Cell(0, 1) // down
        };

        public Grid(int width, int height)
        {
            this.width = width;
            this.height = height;
        }


        /// <summary>
        /// check the target cell (id) is within the grid
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool InBounds(Cell id)
        {
            return 0 <= id.x && id.x < width && 0 <= id.y && id.y < height;
        }


        /// <summary>
        /// TO check whether the cell (id) can be traversed
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Passable(Cell id)
        {
            if (stone.Contains(id)) { return false; }
            if (water.Contains(id)) { return false; }
            if (brickWalls.Contains(id)) { return false; }

            return true;
        }

        /// <summary>
        /// Returns the cost to travel on cell b
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public int Cost(Cell a, Cell b)
        {
            // implement the cost function..... here...
            // int cost = 5;

            //  if (lifePacks.Contains(b) /* && player.health <75%*/){cost = 1; }
            //  if (coins.Contains(b) /* && player.health >75%*/) { cost = 1; }


            return 1;
        }

        /// <summary>
        /// Returns an enumerator containing all the neighbours of the Cell-'id', which supports iteration over Cells
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<Cell> Neighbors(Cell id)
        {
            foreach (var dir in MOVABLE_DIRECTIONS)
            {
                Cell next = new Cell(id.x + dir.x, id.y + dir.y);
                if (InBounds(next) && Passable(next))
                {
                    //have used yield-return to return the cells incrementally.
                    yield return next;
                }
            }
        }

    }
}
