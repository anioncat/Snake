using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SnakeGame
{
    internal class SnakeSegment : Entity
    {
        /// <summary>
        /// Padding to center the segment on the grid
        /// </summary>
        private Point padding;

        protected SnakeSegment next;
        protected SnakeSegment prev;

        protected Point Padding => padding;

        public SnakeSegment()
        { next = null; prev = null; }

        public SnakeSegment(SnakeSegment prevSegment)
        {
            next = null;
            prev = prevSegment;
            size = prevSegment.Size;
            padding = prevSegment.Padding;
            // Place out of bounds
            Position = new Point(-20, -20);
            boundingBox = prevSegment.boundingBox;
        }

        /// <summary>
        /// Recursive call with precalced values
        /// </summary>
        /// <param name="x">size x</param>
        /// <param name="y">size y</param>
        /// <param name="sx">bbox size x</param>
        /// <param name="sy">bbox size y</param>
        /// <param name="px">padding x</param>
        /// <param name="py">padding y</param>
        private void UpdateSize(int x, int y, int sx, int sy, int px, int py)
        {
            size.X = x;
            size.Y = y;
            boundingBox.Width = sx;
            boundingBox.Height = sy;
            padding.X = px;
            padding.Y = py;
            if (!(next is null)) next.UpdateSize(x, y, sx, sy, px, py);
        }

        /// <summary>
        /// Adds a segment to the end of the LL
        /// </summary>
        public virtual void AddSegment()
        {
            if (next is null) next = new SnakeSegment(this);
            else next.AddSegment();
        }

        /// <summary>
        /// Checks each segment for an intersection with a co-ordinate
        /// </summary>
        /// <param name="other">the co-ordinate (position) of the other object</param>
        /// <returns></returns>
        public virtual bool CheckContains(Point other)
        {
            var curSeg = new Rectangle(Position, Size);
            bool intersect = curSeg.Contains(other);
            if (!(next is null)) intersect |= next.CheckContains(other);
            return intersect;
        }

        public override void Draw(SpriteBatch _spriteBatch, Texture2D tex)
        {
            _spriteBatch.Draw(tex, boundingBox, Color.White);
            if (!(next is null)) next.Draw(_spriteBatch, tex);
        }

        public virtual void Update()
        {
            if (!(next is null)) next.Update();
            boundingBox = prev.boundingBox;
            Position = prev.Position;
        }

        // This should be called at head
        public override void UpdateSize(int x, int y)
        {
            size.X = x;
            size.Y = y;
            boundingBox.Width = (int)(x * 0.8);
            boundingBox.Height = (int)(y * 0.8);
            padding.X = (size.X - boundingBox.Width) / 2;
            padding.Y = (size.Y - boundingBox.Height) / 2;
            if (!(next is null)) next.UpdateSize(x, y, size.X, size.Y, padding.X, padding.Y);
        }
    }
}
