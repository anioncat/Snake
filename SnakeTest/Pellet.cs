using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SnakeTest
{
    internal class Pellet : Entity
    {
        public int SIZE { get; } = 20;
        public bool Active { get; set; }

        public Pellet()
        { this.Active = false; }

        public void Spawn(Point pos)
        {
            this.Position = pos;
            this.boundingBox = new Rectangle(this.Position, new Point(this.SIZE));
            this.Active = true;
        }

        public override void Draw(SpriteBatch _spriteBatch, Texture2D tex)
        {
            _spriteBatch.Draw(tex, this.boundingBox, Color.White);
        }
    }
}