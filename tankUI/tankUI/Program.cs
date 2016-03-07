using System;

namespace tankUI
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Game1 game0 = new Game1())
            {
                game0.Run();
            }
        }
    }
#endif
}

