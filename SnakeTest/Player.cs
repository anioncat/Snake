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
            this.Direction = 0;

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
            if ((Direction | i) == (int)(KeyDirection.Right | KeyDirection.Left)) return;
            if ((Direction | i) == (int)(KeyDirection.Up | KeyDirection.Down)) return;
            Direction = i;
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
            if (Direction == (int)KeyDirection.Left)
            {
                this.boundingBox.X -= speed;
                if (this.boundingBox.X <= 0) this.boundingBox.X = w.Width - Size;
            }
            if (Direction == (int)KeyDirection.Right)
            {
                this.boundingBox.X += speed;
                if (this.boundingBox.X >= w.Width) this.boundingBox.X = 0;
            }
            if (Direction == (int)KeyDirection.Up)
            {
                this.boundingBox.Y -= speed;
                if (this.boundingBox.Y <= 0) this.boundingBox.Y = w.Height - Size;
            }
            if (Direction == (int)KeyDirection.Down)
            {
                this.boundingBox.Y += speed;
                if (this.boundingBox.Y >= w.Height) this.boundingBox.Y = 0;
            }
        }
    }
}