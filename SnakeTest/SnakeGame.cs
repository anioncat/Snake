using System;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SnakeTest
{
    internal struct WindowSize
    {
        public int Height { get; set; }
        public int Width { get; set; }

        public WindowSize(int w, int h)
        { Width = w; Height = h; }
    }

    public class SnakeGame : Game
    {
        private static readonly Keys[] movementKeys = new Keys[] { Keys.Left, Keys.Right, Keys.Up, Keys.Down };
        private static GameGrid gg;
        private static int score;
        private static WindowSize window = new WindowSize(600, 600);
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private KeyboardState oldState;
        private Pellet pellet;
        private Player player;
        private Random rng;
        private Texture2D white;

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

        private void CheckPlayerCollision()
        {
            // Check collision
            if (pellet.BoundingBox.Intersects(player.BoundingBox))
            {
                pellet.Active = false;
                score += 1;
                player.AddSegment();
                player.IncreaseSpeed();
            }
        }

        private Point GenerateValidPelletSpawn()
        {
            Point randomPos = gg.SnapPosition(GetRandomPos(rng, window.Width - pellet.Size.X, window.Height - pellet.Size.Y));
            while (player.CheckContains(randomPos))
            {
                Debug.WriteLine($"pellet failed to spawn. retrying.");
                randomPos = gg.SnapPosition(GetRandomPos(rng, window.Width - pellet.Size.X, window.Height - pellet.Size.Y));
            }
            return randomPos;
        }

        private Point GetRandomPos(Random rng, int w, int h) => new Point(rng.Next(w), rng.Next(h));

        private void SpawnNewPellet()
        {
            pellet.Spawn(GenerateValidPelletSpawn());
            Debug.WriteLine($"pellet spawned at: ({pellet.BoundingBox.X}, {pellet.BoundingBox.Y})");
        }

        private void UpdateEntitySize(GameGrid gg, Entity e)
        {
            e.UpdateSize((int)gg.DiscreteX, (int)gg.DiscreteY);
        }

        private void UpdatePlayerMovementDirection()
        {
            // Get current keyboard state
            KeyboardState kbFrameState = Keyboard.GetState();

            // Check state of movement keys i = left right up down
            for (int i = 0; i < movementKeys.Length; ++i)
            {
                // Blocks a key being held down blocking movement in other directions
                if (!player.MoveLock && oldState.IsKeyUp(movementKeys[i]) && kbFrameState.IsKeyDown(movementKeys[i]))
                {
                    player.ChangeDirection(1 << i);
                    break;
                }
            }
            // Update old state
            oldState = kbFrameState;
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

        protected override void Initialize()
        {
            gg = new GameGrid(window.Width, window.Height);
            // Initalise random number generator
            rng = new Random();
            // Initialise player position
            Point pStart = new Point(rng.Next(window.Width), rng.Next(window.Height));
            player = new Player(pStart);
            pellet = new Pellet();

            UpdateEntitySize(gg, player);
            UpdateEntitySize(gg, pellet);

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

            // Update player
            UpdatePlayerMovementDirection();
            CheckPlayerCollision();
            player.Update(window, gg);

            // Spawn pellet if eaten
            if (!pellet.Active) SpawnNewPellet();

            base.Update(gameTime);
        }
    }
}
