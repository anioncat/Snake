using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SnakeTest
{
    internal class Pellet : Entity
    {
        private Point size = new Point(20, 20);

        public bool Active { get; set; }

        public Point Size => size;

        public Pellet()
        { Active = false; }

        public override void Draw(SpriteBatch _spriteBatch, Texture2D tex)
        { _spriteBatch.Draw(tex, this.boundingBox, Color.White); }

        public void Spawn(Point pos)
        {
            this.Position = pos;
            this.boundingBox = new Rectangle(Position, new Point(size.X));
            this.Active = true;
        }

        public override void UpdateSize(int x, int y)
        {
            size.X = x;
            size.Y = y;
        }
    }
}
