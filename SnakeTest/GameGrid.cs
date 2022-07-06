using Microsoft.Xna.Framework;

namespace SnakeTest
{
    // Encapsulate co-ordinates of the grid
    internal readonly struct GridIndex
    {
        public int X { get; }
        public int Y { get; }

        public GridIndex(int x, int y)
        { X = x; Y = y; }

        public override string ToString() => $"({X}, {Y})";
    }

    internal class GameGrid
    {
        // The number of spaces in the grid
        private static readonly int GRID_SIZE = 20;

        // Discretise the window size to get the grid numbers Spacing is updated with the window,
        // else is read only to other objects
        private readonly double[] discretisedSpace = new double[2];

        // The grid containing pointers to Entities
        private readonly Entity[,] grid = new Entity[GRID_SIZE, GRID_SIZE];

        public double DiscreteX
        { get { return discretisedSpace[0]; } }

        public double DiscreteY
        { get { return discretisedSpace[1]; } }

        public GameGrid(int windowWidth, int windowHeight)
        { UpdateWindow(windowWidth, windowHeight); }

        // Floors a co-ordinate to give the index the entity is in in the grid
        public GridIndex CalculateIndex(Point pos) => new GridIndex((int)(pos.X / DiscreteX), (int)(pos.Y / DiscreteY));

        // Gets the position in the window wrt to the grid
        public Point GetPosition(GridIndex gi) => (new Point((int)(gi.X * DiscreteX), (int)(gi.Y * DiscreteY)));

        // Gets the snapped position in the window on the grid
        public Point SnapPosition(Point pos)
        {
            int xOffset = pos.X % (int)DiscreteX;
            int yOffset = pos.Y % (int)DiscreteY;
            return new Point(pos.X - xOffset, pos.Y - yOffset);
        }

        public Entity GetEntity(int x, int y)
        { return grid[x, y]; }

        public Entity GetEntity(GridIndex gi)
        { return grid[gi.X, gi.Y]; }

        public void UpdateEntity(Entity e, GridIndex gi)
        {
            grid[gi.X, gi.Y] = e;
        }

        // Updates the spacing between grid indices wrt window size
        public void UpdateWindow(int windowWidth, int windowHeight)
        {
            discretisedSpace[0] = windowWidth / GRID_SIZE;
            discretisedSpace[1] = windowHeight / GRID_SIZE;
        }
    }
}
