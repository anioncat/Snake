using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace SnakeTest
{
    internal class UI
    {
        private SpriteFont font;
        private int score = 0;
        private Vector2 pos = new Vector2(100, 100);

        public UI()
        { }

        public void Update()
        { }

        public void Draw(SpriteBatch _spriteBatch)
        { _spriteBatch.DrawString(font, $"Score: {score}", pos, Color.White); }

        public void Load(ContentManager cm, SpriteBatch _spriteBatch)
        { font = cm.Load<SpriteFont>("Score"); }

        public void AddScore()
        { score += 1; }
    }
}
