using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SnakeTest
{
    class Player : SnakeSegment
    {
        private byte speed = 2;
        public Player(Point startingPosition)
        {
            this.Direction = new bool[] { false, false, false, false };
            this.Position = startingPosition;
            this.boundingBox = new Rectangle(this.Position, new Point(SIZE));
        }
        public void ChangeDirection(int i)
        {
            if (Direction[0] && i == 1) { return; }
            if (Direction[1] && i == 0) { return; }
            if (Direction[2] && i == 3) { return; }
            if (Direction[3] && i == 2) { return; }
            Array.Clear(Direction, 0, 4);
            Direction[i] = true;
        }

        public override void Update(int xSize, int ySize)
        {
            if (!(this.next is null))
            {
                this.next.Update(xSize, ySize);
            }
            if (Direction[0])
            {
                this.boundingBox.X -= speed;
                if (this.boundingBox.X <= 0)
                {
                    this.boundingBox.X = xSize;
                }
            }
            if (Direction[1])
            {
                this.boundingBox.X += speed;
                if (this.boundingBox.X >= xSize)
                {
                    this.boundingBox.X = 0;
                }
            }
            if (Direction[2])
            {
                this.boundingBox.Y -= speed;
                if (this.boundingBox.Y <= 0)
                {
                    this.boundingBox.Y = ySize;
                }
            }
            if (Direction[3])
            {
                this.boundingBox.Y += speed;
                if (this.boundingBox.Y >= ySize)
                {
                    this.boundingBox.Y = 0;
                }
            }
        }

        public override void Draw(SpriteBatch _spriteBatch, Texture2D tex)
        {
            _spriteBatch.Draw(tex, this.boundingBox, Color.White);
            if (!(next is null))
            {
                next.Draw(_spriteBatch, tex);
            }
        }

        public override void AddSegment() {
            base.AddSegment();
        }
    }
}
