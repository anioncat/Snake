using Microsoft.Xna.Framework;

namespace SnakeGame
{
    class BoundingBox
    {
        public Vector2 Min { get; set; }
        public Vector2 Max { get; set; }

        public BoundingBox() { }
        public BoundingBox(Vector2 min, Vector2 max)
        {
            Min = min;
            Max = max;
        }

        public BoundingBox(float xMin, float xMax, float yMin, float yMax)
        {
            Min = new Vector2(xMin, yMin);
            Max = new Vector2(xMax, yMax);
        }

        public bool Intersect(BoundingBox other)
        {
            if (Min.X < other.Max.X) { return true; }
            if (Max.X > other.Min.X) { return true; }
            if (Min.Y < other.Max.Y) { return true; }
            if (Max.Y > other.Min.Y) { return true; }
            return false;
        }
    }
}
