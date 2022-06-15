using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;
using System.Diagnostics;

namespace SnakeTest
{
    public class SnakeGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private static int windowWidth = 800;
        private static int windowHeight = 600;

        Random rng;
        KeyboardState oldState;
        static readonly Keys[] movementKeys = new Keys[] { Keys.Left, Keys.Right, Keys.Up, Keys.Down };

        Texture2D white;

        Player player;
        Pellet pellet;

        public SnakeGame()
        {
            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = windowWidth,
                PreferredBackBufferHeight = windowHeight
            };
            _graphics.ApplyChanges();
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            rng = new Random();
            Point pStart = new Point(rng.Next(windowWidth), rng.Next(windowHeight));
            player = new Player(pStart);
            pellet = new Pellet();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            white = new Texture2D(GraphicsDevice, 1, 1);
            white.SetData<Color>(new Color[] { Color.White });

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            KeyboardState kbFrameState = Keyboard.GetState();

            for (int i = 0; i < movementKeys.Length; ++i)
            {
                if (oldState.IsKeyUp(movementKeys[i]) && kbFrameState.IsKeyDown(movementKeys[i]))
                {
                    player.ChangeDirection(i);
                    break;
                }
            }

            // Check collision
            if (pellet.BoundingBox.Intersects(player.BoundingBox)) {
                pellet.Active = false;
                player.AddSegment();
            }
            player.Update(windowWidth, windowHeight);

            if (!pellet.Active)
            {
                pellet.Spawn(pellet.GetRandomPos(rng, windowWidth - pellet.SIZE, windowHeight - pellet.SIZE));
                Debug.WriteLine("pellet spawned at: " + pellet.BoundingBox.X + "," + pellet.BoundingBox.Y);
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();
            player.Draw(_spriteBatch, white);
            pellet.Draw(_spriteBatch, white);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
