using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SnakeTest
{
    internal class Player : SnakeSegment
    {
        private Point newGridPos;
        private Vector2 playerPos;
        private Point prevGridPos;
        private const float STARTING_SPEED = 1.7f;
        private const float SPEED_INCREASE = 0.1f;

        /// <summary>
        /// Bit flag to determine movement direction
        /// </summary>
        public MoveDirection Direction { get; set; }

        /// <summary>
        /// Bit to lock the player into moving the inputted direction 1 grid
        /// </summary>
        public bool MoveLock { get; private set; } = false;

        /// <summary>
        /// Speed of player
        /// </summary>
        public float Speed { get; private set; } = STARTING_SPEED;

        /// <summary>
        /// Speed increase per score
        /// </summary>
        public float SpeedIncrease { get; private set; } = SPEED_INCREASE;

        public Player(Point startingPosition)
        {
            Direction = 0;
            Position = startingPosition;
            playerPos = startingPosition.ToVector2();
            newGridPos = new Point(-1, -1);
            boundingBox = new Rectangle(Position, new Point(Size.X, Size.Y));
        }

        /// <summary>
        /// Blocks the player from giving input the turn back on them selves
        /// </summary>
        /// <param name="kdi">calculated move direction bit flag</param>
        /// <returns>true if the input means the player would move back into themselves</returns>
        private bool BlockSelfTurn(MoveDirection kdi)
        {
            MoveDirection dirOrKdi = this.Direction | kdi;
            return (dirOrKdi == (MoveDirection.Right | MoveDirection.Left)) || (dirOrKdi == (MoveDirection.Up | MoveDirection.Down));
        }

        public override void AddSegment()
        {
            IncreaseSpeed();
            base.AddSegment();
        }

        /// <summary>
        /// Changes the direction the player is moving
        /// </summary>
        /// <param name="i">The flag received as input as an int</param>
        public void ChangeDirection(int i)
        {
            // Cast value to enum
            var kdi = (MoveDirection)i;
            if (BlockSelfTurn(kdi)) return;
            Direction = kdi;
            MoveLock = true;
        }

        /// <summary>
        /// Checks if the player has collided with their body
        /// </summary>
        /// <returns>true is collision occurs; else false</returns>
        public bool CollideSelf()
        {
            if (!(next is null)) return next.CheckContains(Position);
            return false;
        }

        public override void Draw(SpriteBatch _spriteBatch, Texture2D tex)
        {
            if (!(next is null)) next.Draw(_spriteBatch, tex);
            _spriteBatch.Draw(tex, boundingBox, Color.Red);
        }

        private void IncreaseSpeed()
        {
            Speed += SpeedIncrease;
            // Diminishing speedup
            SpeedIncrease *= 0.9f;
        }

        public void Reset(Point newSPos)
        {
            Position = newSPos;
            playerPos = newSPos.ToVector2();
            boundingBox.X = newSPos.X;
            boundingBox.Y = newSPos.Y;
            next = null;
            Direction = 0;
            Speed = STARTING_SPEED;
            SpeedIncrease = SPEED_INCREASE;
        }

        public void Update(GameGrid gg)
        {
            switch (Direction)
            {
                case MoveDirection.Left:
                    playerPos.X -= Speed;
                    if (playerPos.X <= 0.0) playerPos.X = gg.Window.Width - Size.X;
                    break;

                case MoveDirection.Right:
                    playerPos.X += Speed;
                    if (playerPos.X >= gg.Window.Width) playerPos.X = 0.0f;
                    break;

                case MoveDirection.Up:
                    playerPos.Y -= Speed;
                    if (playerPos.Y <= 0.0) playerPos.Y = gg.Window.Height - Size.Y;
                    break;

                case MoveDirection.Down:
                    playerPos.Y += Speed;
                    if (playerPos.Y >= gg.Window.Height) playerPos.Y = 0.0f;
                    break;

                case MoveDirection.None:
                    break;
            }
            // Determine whether the player has moved over a grid threshold. GridPos are struct so
            // should copy
            prevGridPos = newGridPos;
            gg.CalculateIndex(ref newGridPos, playerPos);
            System.Diagnostics.Debug.WriteLine(newGridPos);

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
