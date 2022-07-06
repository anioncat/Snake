using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SnakeTest
{
    internal class Player : SnakeSegment
    {
        private static readonly byte SPEED_INCREASE = 0;
        private byte speed = 2;

        public Player(Point startingPosition)
        {
            // LRUD
            this.Direction = new bool[] { false, false, false, false };

            this.Position = startingPosition;
            this.boundingBox = new Rectangle(this.Position, new Point(Size));
        }

        public override void AddSegment()
        {
            base.AddSegment();
        }

        public void ChangeDirection(int i)
        {
            // Block turn back on self
            if (Direction[0] && i == 1) return;
            if (Direction[1] && i == 0) return;
            if (Direction[2] && i == 3) return;
            if (Direction[3] && i == 2) return;
            Array.Clear(Direction, 0, 4);
            Direction[i] = true;
        }

        public override void Draw(SpriteBatch _spriteBatch, Texture2D tex)
        {
            _spriteBatch.Draw(tex, this.boundingBox, Color.White);
            if (!(next is null)) next.Draw(_spriteBatch, tex);
        }

        public void IncreaseSpeed()
        {
            this.speed += SPEED_INCREASE;
        }

        public override void Update(WindowSize w)
        {
            if (!(this.next is null)) this.next.Update(w);
            if (Direction[0])
            {
                this.boundingBox.X -= speed;
                if (this.boundingBox.X <= 0) this.boundingBox.X = w.Width - Size;
            }
            if (Direction[1])
            {
                this.boundingBox.X += speed;
                if (this.boundingBox.X >= w.Width) this.boundingBox.X = 0;
            }
            if (Direction[2])
            {
                this.boundingBox.Y -= speed;
                if (this.boundingBox.Y <= 0) this.boundingBox.Y = w.Height - Size;
            }
            if (Direction[3])
            {
                this.boundingBox.Y += speed;
                if (this.boundingBox.Y >= w.Height) this.boundingBox.Y = 0;
            }
        }
    }
}