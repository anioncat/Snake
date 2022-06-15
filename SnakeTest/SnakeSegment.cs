using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;

namespace SnakeTest
{
    class SnakeSegment : Entity
    {
        public int SIZE { get; } = 15;
        
        protected SnakeSegment next;
        protected SnakeSegment prev;
        public bool[] Direction { get; set; }
        public SnakeSegment() { }

        public SnakeSegment(SnakeSegment prevSegment, Point pos)
        {
            this.next = null;
            this.prev = prevSegment;
            this.Position = pos;
            this.boundingBox = new Rectangle(this.Position, new Point(SIZE));
        }

        public virtual void AddSegment()
        {
            if (this.next is null)
            {
                this.next = new SnakeSegment(this, new Point(boundingBox.X, boundingBox.Y));
                System.Diagnostics.Debug.WriteLine("Created new segment @ " + next.boundingBox.X + ", " + next.boundingBox.Y);
            }
            else
            {
                this.next.AddSegment();
            }
        }
        public virtual void Update(int xSize, int ySize)
        {
            // Move in same direction
            // Update direction to head's direction
            // Rotate if direction changed
            // Cascade
            if (!(this.next is null))
            {
                this.next.Update(xSize, ySize);
            }
            this.boundingBox.X = prev.boundingBox.X;
            this.boundingBox.Y = prev.boundingBox.Y;
            
        }
        public override void Draw(SpriteBatch _spriteBatch, Texture2D tex)
        {
            _spriteBatch.Draw(tex, this.boundingBox, Color.White);
            if (!(next is null))
            {
                next.Draw(_spriteBatch, tex);
            }
        }
    }
}
