using Microsoft.Xna.Framework;

namespace SnakeTest
{
    internal class GameGrid
    {
        // The number of spaces in the grid
        private static readonly int GRID_SIZE = 15;

        // Discretise the window size to get the grid numbers Spacing is updated with the window,
        // else is read only to other objects
        private readonly double[] discretisedSpace = new double[2];

        private readonly int[] midOffset = new int[2];

        private WindowSize window;

        public double DiscreteX => discretisedSpace[0];
        public double DiscreteY => discretisedSpace[1];
        public int MidOffsetX => midOffset[0];
        public int MidOffsetY => midOffset[1];
        public WindowSize Window => window;

        public GameGrid(WindowSize window)
        {
            this.window = window;
            UpdateWindow();
        }

        // Floors a co-ordinate to give the index the entity is in in the grid
        public void CalculateIndex(ref Point gi, Vector2 pos)
        {
            gi.X = (int)(pos.X / DiscreteX);
            gi.Y = (int)(pos.Y / DiscreteY);
        }

        // Gets the position in the window wrt to the grid
        public Point GetPosition(Point gi) => (new Point((int)(gi.X * DiscreteX), (int)(gi.Y * DiscreteY)));

        // Gets the snapped position in the window on the grid
        public Point SnapPosition(Point pos)
        {
            int xOffset = pos.X % (int)DiscreteX;
            int yOffset = pos.Y % (int)DiscreteY;
            return new Point(pos.X - xOffset, pos.Y - yOffset);
        }

        // Updates the spacing between grid indices wrt window size
        public void UpdateWindow()
        {
            discretisedSpace[0] = window.Width / GRID_SIZE;
            midOffset[0] = (int)(discretisedSpace[0] / 2);
            discretisedSpace[1] = window.Height / GRID_SIZE;
            midOffset[1] = (int)(discretisedSpace[1] / 2);
            System.Diagnostics.Debug.WriteLine($"Grid size snapped at ({discretisedSpace[0]}, {discretisedSpace[1]}) for W:{window.Width} H:{window.Height}");
        }
    }
}
