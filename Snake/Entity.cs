using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SnakeTest
{
    internal abstract class Entity
    {
        protected Rectangle boundingBox;
        protected Point size;
        public Rectangle BoundingBox { get => boundingBox; }
        public virtual Point Position { get; set; }
        public virtual Point Size { get => size; protected set => size = value; }

        /// <summary>
        /// Draw the entity; override to provide the correct texture for the object
        /// </summary>
        /// <param name="_spriteBatch"></param>
        /// <param name="tex"></param>
        public abstract void Draw(SpriteBatch _spriteBatch, Texture2D tex);

        /// <summary>
        /// Update size of entity based on grid size
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public abstract void UpdateSize(int x, int y);
    }
}
