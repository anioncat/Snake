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

        public struct EntitySize
        {
            public int X { get; set; }

            public int Y { get; set; }

            public EntitySize(int x, int y)
            { X = x; Y = y; }
        }
    }
}
