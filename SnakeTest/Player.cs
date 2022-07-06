using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SnakeTest
{
    internal class Player : SnakeSegment
    {
        private static readonly byte SPEED_INCREASE = 0;
        private int score;
        private byte speed = 3;
        private DoublePoint playerPos;

        public Player(Point startingPosition)
        {
            score = 0;
            Direction = 0;
            Position = startingPosition;
            playerPos = new DoublePoint(Position.X, Position.Y);
            boundingBox = new Rectangle(Position, new Point(Size.X, Size.Y));
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
            _spriteBatch.Draw(tex, boundingBox, Color.White);
            if (!(next is null)) next.Draw(_spriteBatch, tex);
        }

        public void IncreaseSpeed()
        {
            speed += SPEED_INCREASE;
        }

        public void Update(WindowSize w)
        {
            if (!(next is null)) next.Update();
            if (Direction == MoveDirection.Left)
            {
                boundingBox.X -= speed;
                if (boundingBox.X <= 0) boundingBox.X = w.Width - Size.X;
            }
            if (Direction == MoveDirection.Right)
            {
                boundingBox.X += speed;
                if (boundingBox.X >= w.Width - Size.X) boundingBox.X = 0;
            }
            if (Direction == MoveDirection.Up)
            {
                boundingBox.Y -= speed;
                if (boundingBox.Y <= 0) boundingBox.Y = w.Height - Size.Y;
            }
            if (Direction == MoveDirection.Down)
            {
                boundingBox.Y += speed;
                if (boundingBox.Y >= w.Height - Size.Y) boundingBox.Y = 0;
            }
        }

        // Mutable co-ordinates of double type
        public struct DoublePoint
        {
            public double X { get; set; }
            public double Y { get; set; }

            public DoublePoint(double x, double y)
            { X = x; Y = y; }

            public override string ToString() => $"({X}, {Y})";
        }
    }
}
