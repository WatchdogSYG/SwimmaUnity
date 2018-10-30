using System;

namespace Swimma
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Game1 game = new Game1(true))
            {
                game.Run();
            }
        }
    }
#endif
}

