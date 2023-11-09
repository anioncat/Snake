using System;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SnakeGame
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
        private static readonly Keys[] s_movementKeys = new Keys[] { Keys.Left, Keys.Right, Keys.Up, Keys.Down };
        private static GameGrid s_gg;
        private static WindowSize s_window;
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private KeyboardState oldState;
        private Pellet pellet;
        private Player player;
        private Random rng;
        private UI ui;
        private Texture2D white;

        public SnakeGame()
        {
            s_window = new WindowSize(600, 600);
            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = s_window.Width,
                PreferredBackBufferHeight = s_window.Height
            };
            _graphics.ApplyChanges();
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        /// <summary>
        /// Add a snake body segment; add 1 to score
        /// </summary>
        private void AddTail()
        {
            pellet.Active = false;
            ui.AddScore();
            player.AddSegment();
        }

        /// <summary>
        /// Checks if the player has "eaten" a pellet
        /// </summary>
        private bool CheckPlayerCollidePellet() => pellet.BoundingBox.Intersects(player.BoundingBox);

        /// <summary>
        /// Generates a random point the is free on the grid
        /// </summary>
        /// <returns>a Point on the grid that is empty</returns>
        private Point GenerateValidPelletSpawn()
        {
            Point randomPos = s_gg.SnapPosition(GetRandomPos(s_window.Width - pellet.Size.X, s_window.Height - pellet.Size.Y));
            while (player.CheckContains(randomPos))
            {
                Debug.WriteLine($"pellet failed to spawn. retrying.");
                randomPos = s_gg.SnapPosition(GetRandomPos(s_window.Width - pellet.Size.X, s_window.Height - pellet.Size.Y));
            }
            return randomPos;
        }

        /// <summary>
        /// Generates a random point with the random number generator
        /// </summary>
        /// <returns>a new random point</returns>
        private Point GetRandomPos(int w, int h) => new Point(rng.Next(w), rng.Next(h));

        /// <summary>
        /// Checks if the player has collided with themselves for a game over
        /// </summary>
        private bool PlayerSelfCollision() => player.CollideSelf();

        /// <summary>
        /// Reset the game to a starting state
        /// </summary>
        private void ResetGame()
        {
            var pStart = new Point(rng.Next(s_window.Width), rng.Next(s_window.Height));
            player.Reset(pStart);
            pellet.Active = false;
            ui.UpdateHighScore();
        }

        /// <summary>
        /// If the player scored a point, respawn the pellet at ta random location
        /// </summary>
        private void SpawnNewPellet()
        {
            pellet.Spawn(GenerateValidPelletSpawn());
            Debug.WriteLine($"pellet spawned at: ({pellet.BoundingBox.X}, {pellet.BoundingBox.Y})");
        }

        private void UpdateEntitySize(GameGrid gg, Entity e) => e.UpdateSize((int)gg.DiscreteX, (int)gg.DiscreteY);

        /// <summary>
        /// Checks that the player has given input to change the snake move direction
        /// </summary>
        private void UpdatePlayerMovementDirection()
        {
            // Get current keyboard state
            var kbFrameState = Keyboard.GetState();

            // Check state of movement keys i = left right up down
            for (int i = 0; i < s_movementKeys.Length; ++i)
            {
                // Blocks a key being held down blocking movement in other directions
                if (!player.MoveLock && oldState.IsKeyUp(s_movementKeys[i]) && kbFrameState.IsKeyDown(s_movementKeys[i]))
                {
                    player.ChangeDirection(1 << i, s_gg);
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
            ui.Draw(_spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        protected override void Initialize()
        {
            s_gg = new GameGrid(s_window);
            // Initalise random number generator
            rng = new Random();
            // Initialise player position
            var pStart = new Point(rng.Next(s_window.Width), rng.Next(s_window.Height));
            player = new Player(pStart);
            pellet = new Pellet();
            ui = new UI();

            UpdateEntitySize(s_gg, player);
            UpdateEntitySize(s_gg, pellet);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            white = new Texture2D(GraphicsDevice, 1, 1);
            white.SetData<Color>(new Color[] { Color.White });
            ui.Load(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            // Exit game
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Update player
            UpdatePlayerMovementDirection();
            if (PlayerSelfCollision()) ResetGame();
            if (CheckPlayerCollidePellet()) AddTail();
            player.Update(s_gg);

            // Spawn pellet if eaten
            if (!pellet.Active) SpawnNewPellet();

            base.Update(gameTime);
        }
    }
}
