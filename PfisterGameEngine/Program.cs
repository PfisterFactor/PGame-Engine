using System;

namespace PfisterGameEngine
{
    static class Program
    {
        // /<summary>
        // /The main entry point for the application.
        // /</summary>
        static void Main()
        {
            using (MainGame game = new MainGame())
            {
                game.Run();
            }
        }
    }
}

