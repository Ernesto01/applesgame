using System;

namespace Apple01
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (ApplesGame game = new ApplesGame())
            {
                game.Run();
            }
        }
    }
#endif
}

