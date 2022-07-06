using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SnakeTest
{
    internal enum KeyDirection
    {
        Left = 1 << 0,
        Right = 1 << 1,
        Up = 1 << 2,
        Down = 1 << 3
    }

    internal class SnakeSegment : Entity
    {
        protected SnakeSegment next;
        protected SnakeSegment prev;

        public SnakeSegment()
        { }

        public SnakeSegment(SnakeSegment prevSegment, Point pos)
        {
            this.next = null;
            this.prev = prevSegment;
            this.Position = pos;
            this.boundingBox = new Rectangle(this.Position, new Point(Size));
        }

        public int Direction { get; set; }
        public int Size { get; } = 15;

        public virtual void AddSegment()
        {
            if (this.next is null) this.next = new SnakeSegment(this, new Point(boundingBox.X, boundingBox.Y));
            else this.next.AddSegment();
        }

        public override void Draw(SpriteBatch _spriteBatch, Texture2D tex)
        {
            _spriteBatch.Draw(tex, this.boundingBox, Color.White);
            if (!(next is null)) next.Draw(_spriteBatch, tex);
        }

        public virtual void Update(WindowSize w)
        {
            // Move in same direction
            // Update direction to head's direction
            // Rotate if direction changed
            // Cascade
            if (!(this.next is null)) this.next.Update(w);

            this.boundingBox.X = prev.boundingBox.X;
            this.boundingBox.Y = prev.boundingBox.Y;
        }
    }
}