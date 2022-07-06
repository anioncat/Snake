using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SnakeTest
{
    internal abstract class Entity
    {
        protected Rectangle boundingBox;

        public Rectangle BoundingBox
        { get { return boundingBox; } }

        public virtual Point Position { get; set; }

        public abstract void Draw(SpriteBatch _spriteBatch, Texture2D tex);
    }
}
