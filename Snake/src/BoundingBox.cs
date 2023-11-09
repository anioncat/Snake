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
            this.Min = min;
            this.Max = max;
        }

        public BoundingBox(float xmin, float xmax, float ymin, float ymax)
        {
            this.Min = new Vector2(xmin, ymin);
            this.Max = new Vector2(xmax, ymax);
        }

        public bool Intersect(BoundingBox other)
        {
            if (this.Min.X < other.Max.X) { return true; }
            if (this.Max.X > other.Min.X) { return true; }
            if (this.Min.Y < other.Max.Y) { return true; }
            if (this.Max.Y > other.Min.Y) { return true; }
            return false;
        }
    }
}
