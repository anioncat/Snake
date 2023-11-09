using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace SnakeGame
{
    internal class UI
    {
        private SpriteFont font;
        private Vector2 scorePos;
        private Vector2 highScorePos;

        public int Score { get; private set; }
        public int HighScore { get; private set; }

        public UI()
        {
            Score = 0;
            scorePos = new Vector2(100f, 100f);
            highScorePos = new Vector2(100f, 150f);
        }

        public void AddScore()
        { Score += 1; }

        public void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.DrawString(font, $"Score: {Score}", scorePos, Color.White);
            _spriteBatch.DrawString(font, $"High Score: {HighScore}", highScorePos, Color.White);
        }

        public void Load(ContentManager cm)
        { font = cm.Load<SpriteFont>("Score"); }

        public void Update()
        { }

        public void UpdateHighScore()
        {
            if (Score > HighScore) HighScore = Score;
            Score = 0;
        }
    }
}
