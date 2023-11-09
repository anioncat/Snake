using System;

namespace SnakeGame
{
    public static class Program
    {
        [STAThread]
        private static void Main()
        {
            using var game = new SnakeGame();
            game.Run();
        }
    }
}
