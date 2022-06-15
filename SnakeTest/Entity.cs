using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SnakeTest
{
    abstract class Entity
    {
        public Point Position { get; set; }
        public Rectangle BoundingBox {
            get{
                return boundingBox;
            }
        }
        protected Rectangle boundingBox;

        public Point GetRandomPos(Random rng, int w, int h) => new Point(rng.Next(w), rng.Next(h));
        public abstract void Draw(SpriteBatch _spriteBatch, Texture2D tex);

    }
}
