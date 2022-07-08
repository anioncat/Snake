using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SnakeTest
{
    internal class Player : SnakeSegment
    {
        private static float speedIncrease = 0.15f;
        private Point newGridPos;
        private Vector2 playerPos;
        private Point prevGridPos;
        private float speed = 1.7f;

        public bool MoveLock { get; private set; } = false;

        public Player(Point startingPosition)
        {
            Direction = 0;
            Position = startingPosition;
            playerPos.X = startingPosition.X;
            playerPos.Y = startingPosition.Y;
            newGridPos = new Point(-1, -1);
            boundingBox = new Rectangle(Position, new Point(Size.X, Size.Y));
        }

        public override void AddSegment()
        {
            IncreaseSpeed();
            base.AddSegment();
        }

        private bool BlockSelfTurn(MoveDirection kdi)
        {
            MoveDirection dirOrKdi = this.Direction | kdi;
            return (dirOrKdi == (MoveDirection.Right | MoveDirection.Left)) || (dirOrKdi == (MoveDirection.Up | MoveDirection.Down));
        }

        public void ChangeDirection(int i)
        {
            // Cast value to enum
            MoveDirection kdi = (MoveDirection)i;
            // Block turn back on self
            if (BlockSelfTurn(kdi)) return;
            Direction = kdi;
            MoveLock = true;
        }

        public void Die()
        { }

        public override void Draw(SpriteBatch _spriteBatch, Texture2D tex)
        {
            if (!(next is null)) next.Draw(_spriteBatch, tex);
            _spriteBatch.Draw(tex, boundingBox, Color.Red);
        }

        public void IncreaseSpeed()
        {
            speed += speedIncrease;
            // Diminishing speedup
            speedIncrease *= 0.9f;
        }

        public void Update(GameGrid gg)
        {
            switch (Direction)
            {
                case MoveDirection.Left:
                    playerPos.X -= speed;
                    if (playerPos.X <= 0.0) playerPos.X = gg.Window.Width - Size.X;
                    break;

                case MoveDirection.Right:
                    playerPos.X += speed;
                    if (playerPos.X >= gg.Window.Width) playerPos.X = 0.0f;
                    break;

                case MoveDirection.Up:
                    playerPos.Y -= speed;
                    if (playerPos.Y <= 0.0) playerPos.Y = gg.Window.Height - Size.Y;
                    break;

                case MoveDirection.Down:
                    playerPos.Y += speed;
                    if (playerPos.Y >= gg.Window.Height) playerPos.Y = 0.0f;
                    break;
            }
            // Determine whether the player has moved over a grid threshold. GridPos are struct so
            // should copy
            prevGridPos = newGridPos;
            gg.CalculateIndex(ref newGridPos, playerPos);

            if (!prevGridPos.Equals(newGridPos))
            {
                // Check player reaches next grid position. Need to update first before snapping
                // segment forwards
                if (!(next is null)) next.Update();
                // Move segment to next grid position
                Point nextPoint = gg.GetPosition(newGridPos);
                boundingBox.X = nextPoint.X + Padding.X;
                boundingBox.Y = nextPoint.Y + Padding.Y;
                playerPos.X = nextPoint.X + gg.MidOffsetX;
                playerPos.Y = nextPoint.Y + gg.MidOffsetY;
                Position = nextPoint;
                MoveLock = false;
            }
        }
    }
}
