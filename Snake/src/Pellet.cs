﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SnakeGame
{
    internal class Pellet : Entity
    {
        public bool Active { get; set; }

        public Pellet()
        {
            Active = false;
            size = new Point(20, 20);
        }

        public override void Draw(SpriteBatch _spriteBatch, Texture2D tex)
        { _spriteBatch.Draw(tex, boundingBox, Color.White); }

        public void Spawn(Point pos)
        {
            Position = pos;
            boundingBox = new Rectangle(Position, new Point(size.X));
            Active = true;
        }

        public override void UpdateSize(int x, int y)
        {
            size.X = x;
            size.Y = y;
        }
    }
}
