using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace SnakeGame
{
    /// <summary>
    /// Helper class to calculate the position of objects within a grid
    /// </summary>
    internal class GameGrid
    {
        /// <summary>
        /// The number of spaces in the grid
        /// </summary>
        private const int GRID_SIZE = 15;

        /// <summary>
        /// Discretise the window size to get the grid numbers <br/> Spacing is updated with the
        /// window, else is read only to other objects
        /// </summary>
        private readonly double[] discretisedSpace = new double[2];

        private readonly int[] midOffset = new int[2];

        private WindowSize window;

        /// <summary>
        /// Gets the grid spacing in the X direction
        /// </summary>
        public double DiscreteX => discretisedSpace[0];

        /// <summary>
        /// Gets the grid spacing the Y direction
        /// </summary>
        public double DiscreteY => discretisedSpace[1];

        /// <summary>
        /// Gets the pre-calculated pixel offset for the midpoint of the grid X
        /// </summary>
        public int MidOffsetX => midOffset[0];

        /// <summary>
        /// Gets the pre-calculated pixel offset for the midpoint of the grid Y
        /// </summary>
        public int MidOffsetY => midOffset[1];

        /// <summary>
        /// Internal copy of the game window size
        /// </summary>
        public WindowSize Window => window;

        public GameGrid(WindowSize window)
        {
            this.window = window;
            UpdateWindow();
        }

        /// <summary>
        /// Floors a co-ordinate to give the index the entity is in in the grid
        /// </summary>
        public void CalculateIndex(ref Point gi, Vector2 pos)
        {
            gi.X = (int)(pos.X / DiscreteX);
            gi.Y = (int)(pos.Y / DiscreteY);
        }

        /// <summary>
        /// Helper method to convert Point type to Vector2
        /// </summary>
        public void CalculateIndex(ref Point gi, Point pos)
        {
            CalculateIndex(ref gi, pos.ToVector2());
        }

        public Vector2 GetGridCenterOffset(Point gridPosition, Vector2 position)
        {
            return new Vector2(gridPosition.X + MidOffsetX - position.X, gridPosition.Y + MidOffsetY - position.Y);
        }

        public Point GetCellCenter(Point gridPosition) {
            return new Point(gridPosition.X + MidOffsetX, gridPosition.Y + MidOffsetY);
        }

        /// <summary>
        /// Gets the position in the window wrt to the grid
        /// </summary>
        public Point GetPosition(Point gi) => new Point((int)(gi.X * DiscreteX), (int)(gi.Y * DiscreteY));

        /// <summary>
        /// Gets the snapped position in the window on the grid
        /// </summary>
        public Point SnapPosition(Point pos)
        {
            int xOffset = pos.X % (int)DiscreteX;
            int yOffset = pos.Y % (int)DiscreteY;
            return new Point(pos.X - xOffset, pos.Y - yOffset);
        }

        /// <summary>
        /// Updates the spacing between grid indices w.r.t. window size
        /// </summary>
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
