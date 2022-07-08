using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace SnakeTest
{
    internal class UI
    {
        private SpriteFont font;
        private Vector2 pos;
        private int score;

        public UI()
        {
            score = 0;
            pos = new Vector2(100, 100);
        }

        public void AddScore()
        { score += 1; }

        public void Draw(SpriteBatch _spriteBatch)
        { _spriteBatch.DrawString(font, $"Score: {score}", pos, Color.White); }

        public void Load(ContentManager cm)
        { font = cm.Load<SpriteFont>("Score"); }

        public void Update()
        { }
    }
}
