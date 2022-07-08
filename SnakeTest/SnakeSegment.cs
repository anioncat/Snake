using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SnakeTest
{
    internal class SnakeSegment : Entity
    {
        // Padding the center the segment on the grid
        private Point padding;

        // Size of the object on screen
        private Point size;

        protected SnakeSegment next;
        protected SnakeSegment prev;

        protected Point Padding => padding;

        // Bit flag to determine movement direction
        public MoveDirection Direction { get; set; }

        // The size of the object. It can only be updated via the UpdateSize method
        public Point Size => size;

        public SnakeSegment()
        { next = null; prev = null; }

        public SnakeSegment(SnakeSegment prevSegment)
        {
            next = null;
            prev = prevSegment;
            size = prevSegment.Size;
            padding = prevSegment.Padding;
            // Place out of bounds
            Position = new Point(-20, -20);
            boundingBox = prevSegment.BoundingBox;
        }

        // Recursive call with precalced values
        private void UpdateSize(int x, int y, int sx, int sy, int px, int py)
        {
            size.X = x;
            size.Y = y;
            boundingBox.Width = sx;
            boundingBox.Height = sy;
            padding.X = px;
            padding.Y = py;
            if (!(next is null)) next.UpdateSize(x, y, sx, sy, px, py);
        }

        // Adds a segment to the end of the LL
        public virtual void AddSegment()
        {
            if (next is null) next = new SnakeSegment(this);
            else next.AddSegment();
        }

        // Checks each segment for an intersection with other
        public virtual bool CheckContains(Point other)
        {
            Rectangle curSeg = new Rectangle(Position, Size);
            bool intersect = curSeg.Contains(other);
            if (!(next is null)) intersect |= next.CheckContains(other);
            return intersect;
        }

        public override void Draw(SpriteBatch _spriteBatch, Texture2D tex)
        {
            _spriteBatch.Draw(tex, boundingBox, Color.White);
            if (!(next is null)) next.Draw(_spriteBatch, tex);
        }

        public virtual void Update()
        {
            // Move in same direction Update direction to head's direction Rotate if direction
            // changed Cascade
            if (!(next is null)) next.Update();

            boundingBox.X = prev.boundingBox.X;
            boundingBox.Y = prev.boundingBox.Y;
            Position = prev.Position;
        }

        // This should be called at head
        public override void UpdateSize(int x, int y)
        {
            size.X = x;
            size.Y = y;
            boundingBox.Width = (int)(x * 0.8);
            boundingBox.Height = (int)(y * 0.8);
            padding.X = (size.X - boundingBox.Width) / 2;
            padding.Y = (size.Y - boundingBox.Height) / 2;
            if (!(next is null)) next.UpdateSize(x, y, size.X, size.Y, padding.X, padding.Y);
        }
    }
}
