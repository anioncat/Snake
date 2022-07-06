﻿using Microsoft.Xna.Framework;

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

        // The grid containing pointers to Entities
        private readonly Entity[,] grid = new Entity[GRID_SIZE, GRID_SIZE];

        // Discretise the window size to get the grid numbers
        // Spacing is updated with the window, else is read only to other objects
        private readonly double[] discretisedSpace = new double[2];

        private double DiscreteX
        { get { return discretisedSpace[0]; } }

        private double DiscreteY
        { get { return discretisedSpace[1]; } }

        public GameGrid(int windowWidth, int windowHeight)
        { UpdateWindow(windowWidth, windowHeight); }

        // Updates the spacing between grid indices wrt window size
        public void UpdateWindow(int windowWidth, int windowHeight)
        {
            discretisedSpace[0] = windowWidth / GRID_SIZE;
            discretisedSpace[1] = windowHeight / GRID_SIZE;
        }

        public GridIndex CalculateIndex(Point pos)
        {
            int row = (int)(pos.Y / DiscreteY);
            int col = (int)(pos.X / DiscreteX);
            return new GridIndex(row, col);
        }

        public void UpdateEntity(Entity e, GridIndex gi)
        { grid[gi.X, gi.Y] = e; }

        public Entity GetEntity(int x, int y)
        { return grid[x, y]; }

        public Entity GetEntity(GridIndex gi)
        { return grid[gi.X, gi.Y]; }
    }
}