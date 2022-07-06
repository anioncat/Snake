using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SnakeTest
{
    internal class Pellet : Entity
    {
        private Point size;

        public bool Active { get; set; }

        public Point Size
        { get { return size; } }

        public Pellet()
        {
            this.Active = false;
            size = new Point(20, 20);
        }

        public override void Draw(SpriteBatch _spriteBatch, Texture2D tex)
        {
            _spriteBatch.Draw(tex, this.boundingBox, Color.White);
        }

        public void Spawn(Point pos)
        {
            this.Position = pos;
            this.boundingBox = new Rectangle(Position, new Point(size.X));
            this.Active = true;
        }

        public void UpdateSize(GameGrid gg)
        {
            size.X = (int)gg.DiscreteX;
            size.Y = (int)gg.DiscreteY;
        }
    }
}
