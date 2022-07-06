using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SnakeTest
{
    internal class Player : SnakeSegment
    {
        private static readonly byte SPEED_INCREASE = 0;
        private int score;
        private byte speed = 3;

        public Player(Point startingPosition)
        {
            score = 0;
            // LRUD
            this.Direction = 0;

            this.Position = startingPosition;
            this.boundingBox = new Rectangle(this.Position, new Point(Size.X, Size.Y));
        }

        public int Score
        { get { return score; } }

        public override void AddSegment()
        {
            score += 1;
            System.Diagnostics.Debug.WriteLine($"Score: {score}");
            base.AddSegment();
        }

        public void ChangeDirection(int i)
        {
            // Cast value to enum
            MoveDirection kdi = (MoveDirection)i;
            // Block turn back on self
            if ((Direction | kdi) == (MoveDirection.Right | MoveDirection.Left)) return;
            if ((Direction | kdi) == (MoveDirection.Up | MoveDirection.Down)) return;
            Direction = kdi;
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
            if (Direction == MoveDirection.Left)
            {
                this.boundingBox.X -= speed;
                if (this.boundingBox.X <= 0) this.boundingBox.X = w.Width - Size.X;
            }
            if (Direction == MoveDirection.Right)
            {
                this.boundingBox.X += speed;
                if (this.boundingBox.X >= w.Width) this.boundingBox.X = 0;
            }
            if (Direction == MoveDirection.Up)
            {
                this.boundingBox.Y -= speed;
                if (this.boundingBox.Y <= 0) this.boundingBox.Y = w.Height - Size.Y;
            }
            if (Direction == MoveDirection.Down)
            {
                this.boundingBox.Y += speed;
                if (this.boundingBox.Y >= w.Height) this.boundingBox.Y = 0;
            }
        }
    }
}
