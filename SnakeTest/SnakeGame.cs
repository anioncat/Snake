using System;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SnakeTest
{
    internal struct WindowSize
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public WindowSize(int w, int h)
        { Width = w; Height = h; }
    }

    public class SnakeGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private static readonly WindowSize window = new WindowSize(800, 600);

        private static readonly GameGrid gg = new GameGrid(window.Width, window.Height);

        private Random rng;
        private KeyboardState oldState;
        private static readonly Keys[] movementKeys = new Keys[] { Keys.Left, Keys.Right, Keys.Up, Keys.Down };

        private Texture2D white;

        private Player player;
        private Pellet pellet;

        public SnakeGame()
        {
            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = window.Width,
                PreferredBackBufferHeight = window.Height
            };
            _graphics.ApplyChanges();
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // Initalise random number generator
            rng = new Random();
            // Initialise player position
            Point pStart = new Point(rng.Next(window.Width), rng.Next(window.Height));
            player = new Player(pStart);
            pellet = new Pellet();

            gg.UpdateEntity(player, gg.CalculateIndex(player.Position));
            gg.UpdateEntity(pellet, gg.CalculateIndex(pellet.Position));

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
            // Exit game
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Get current keyboard state
            KeyboardState kbFrameState = Keyboard.GetState();

            // Check state of movement keys i = left right up down
            for (int i = 0; i < movementKeys.Length; ++i)
            {
                // Blocks a key being held down blocking movement in other directions
                if (oldState.IsKeyUp(movementKeys[i]) && kbFrameState.IsKeyDown(movementKeys[i]))
                {
                    player.ChangeDirection(1 << i);
                    break;
                }
            }
            // Update old state
            oldState = kbFrameState;

            // Check collision
            if (pellet.BoundingBox.Intersects(player.BoundingBox))
            {
                pellet.Active = false;
                player.AddSegment();
                player.IncreaseSpeed();
            }

            // Update player
            player.Update(window);

            // Spawn pellet if unavailable
            if (!pellet.Active)
            {
                Point randomPos = gg.SnapPosition(GetRandomPos(rng, window.Width - pellet.Size.X, window.Height - pellet.Size.Y));
                pellet.Spawn(randomPos);
                Debug.WriteLine($"pellet spawned at: ({pellet.BoundingBox.X}, {pellet.BoundingBox.Y})");
            }
            base.Update(gameTime);
        }

        private Point GetRandomPos(Random rng, int w, int h) => new Point(rng.Next(w), rng.Next(h));

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
