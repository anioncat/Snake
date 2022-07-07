using System.Windows.Forms;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SnakeTest
{
    internal class Player : SnakeSegment
    {
        private static double speedIncrease = 0.1;
        private GridIndex newGridPos;
        private DoublePoint playerPos;
        private GridIndex prevGridPos;
        private int score;
        private double speed = 2.5;

        public int Score
        { get { return score; } }

        public Player(Point startingPosition)
        {
            score = 0;
            Direction = 0;
            Position = startingPosition;
            playerPos = startingPosition;
            newGridPos = new GridIndex();
            boundingBox = new Rectangle(Position, new Point(Size.X, Size.Y));
        }

        public override void AddSegment()
        {
            score += 1;
            System.Diagnostics.Debug.WriteLine($"Score: {score}");
            IncreaseSpeed();
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

        public void IncreaseSpeed()
        {
            speed += speedIncrease;
            // Diminishing speedup
            speedIncrease *= 0.9;
        }

        public void Update(WindowSize w, GameGrid gg)
        {
            switch (Direction)
            {
                case MoveDirection.Left:
                    playerPos.X -= speed;
                    if (playerPos.X <= 0.0) playerPos.X = w.Width - Size.X;
                    break;

                case MoveDirection.Right:
                    playerPos.X += speed;
                    if (playerPos.X >= w.Width - Size.X) playerPos.X = 0.0;
                    break;

                case MoveDirection.Up:
                    playerPos.Y -= speed;
                    if (playerPos.Y <= 0.0) playerPos.Y = w.Height - Size.Y;
                    break;

                case MoveDirection.Down:
                    playerPos.Y += speed;
                    if (playerPos.Y >= w.Height - Size.Y) playerPos.Y = 0.0;
                    break;
            }
            // Determine whether the player has moved over a grid threshold. GridPos are struct so
            // should copy
            prevGridPos = newGridPos;
            gg.CalculateIndex(ref newGridPos, playerPos);

            if (prevGridPos.NotEquals(newGridPos))
            {
                // Check player reaches next grid position. Need to update first before snapping
                // segment forwards
                if (!(next is null)) next.Update();
                // Move segment to next grid position
                Point nextPoint = gg.GetPosition(newGridPos);
                boundingBox.X = nextPoint.X + Padding.X;
                boundingBox.Y = nextPoint.Y + Padding.Y;
            }
        }

        public void Die()
        { }

        // Mutable co-ordinates of double type
        public struct DoublePoint
        {
            public double X { get; set; }
            public double Y { get; set; }

            public DoublePoint(double x, double y)
            { X = x; Y = y; }

            // Allows use of Point and DoublePoint with lossy interchangability
            public static implicit operator DoublePoint(Point p) => new DoublePoint(p.X, p.Y);

            // Allows use of Point and DoublePoint with lossy interchangability
            public static implicit operator Point(DoublePoint dp) => new Point((int)dp.X, (int)dp.Y);

            public bool Equals(DoublePoint other) => (X == other.X && Y == other.Y);

            public override string ToString() => $"({X}, {Y})";
        }
    }
}
