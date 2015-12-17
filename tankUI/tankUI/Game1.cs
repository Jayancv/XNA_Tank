using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Threading;
using System.Threading.Tasks;
using tankUI.Inside;

namespace tankUI
{

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        GraphicsDevice device;
        Texture2D backgroundTexture;
        Texture2D foregroundTexture;
        int screenWidth;
        int screenHeight;
        Texture2D tankTexture;
        Texture2D brickTexture;
        Texture2D stoneTexture;
        Texture2D waterTexture;
        Texture2D healthPackTexture;
        Texture2D coinTexture;
        public List<Player> players;
       
        Client client;
        //public Player player0, player1, player2, player3, player4;
        private StringEvaluator eval;
        public List<Vector2> bricks;             //Store brick coordinates 
        public List<Vector2> stones;             //store Stone coordinates
        public List<Vector2> water1;             //store water corrdinates
        public List<Vector2> coins;
        public List<Vector2> lifePacks; 

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            client = new Client();
            eval = new StringEvaluator();
            players = new List<Player>();
            bricks = new List<Vector2>();
            stones = new List<Vector2>();
            water1 = new List<Vector2>();
            coins = new List<Vector2>();
            lifePacks = new List<Vector2>();
        }

        protected override void Initialize()
        {
           
            client.send("JOIN#");
            IsMouseVisible = true;
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 650;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
            Window.Title = "JC Tank";
            base.Initialize();

        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            device = graphics.GraphicsDevice;


            backgroundTexture = Content.Load<Texture2D>("background");
            foregroundTexture = Content.Load<Texture2D>("background2");
            screenWidth = device.PresentationParameters.BackBufferWidth;
            screenHeight = device.PresentationParameters.BackBufferHeight;
            tankTexture = Content.Load<Texture2D>("tank1");
            brickTexture = Content.Load<Texture2D>("brick1");
            stoneTexture = Content.Load<Texture2D>("stone1");
            waterTexture = Content.Load<Texture2D>("water");
            coinTexture = Content.Load<Texture2D>("coin");
            healthPackTexture = Content.Load<Texture2D>("health");


            
            CreateForeground();
            
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            eval.evaluate(client.data, this);
            ProcessKeyboard();
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            DrawScenery();
            
            DrawBricks();
            DrawStones();
            DrawWater();
            DrawPlayers();
            DrawCoin();
            DrawLife();
            spriteBatch.End();

            base.Draw(gameTime);
        }


        private void DrawScenery()
        {
            Rectangle screenRectangle = new Rectangle(1, 1, screenWidth, screenHeight);
            spriteBatch.Draw(backgroundTexture, screenRectangle, Color.White);
            spriteBatch.Draw(foregroundTexture, screenRectangle, Color.White);
        }

        private void DrawBricks()
        {
            try
            {
                Rectangle rect = new Rectangle(1, 1, 50, 50);
                foreach (Vector2 brick in bricks)
                {

                    spriteBatch.Draw(brickTexture, brick, rect, Color.White);
                }
            }
            catch (Exception e)
            {
            }
        }

        private void DrawStones()
        {
            try
            {
                Rectangle rect = new Rectangle(1, 1, 50,50);
                foreach (Vector2 stone in stones)
                {
                    spriteBatch.Draw(stoneTexture, stone, rect, Color.White);
                }
            }
            catch (Exception e)
            {
            }

        }
        private void DrawWater()            // method to draw water
        {
            try
            {
                Rectangle rect = new Rectangle(1, 1, 50,50);
                foreach (Vector2 water in water1)
                {
                    spriteBatch.Draw(waterTexture, water, rect, Color.White);
                }
            }
            catch (Exception e)
            {
            }
        }
        private void DrawLife()
        {
            try
            {
                Rectangle rect = new Rectangle(1, 1, 50, 50);
                foreach (Vector2 life in lifePacks)
                {
                    spriteBatch.Draw(healthPackTexture, life, Color.White);
                }
            }
            catch (Exception e)
            {
            }
        }
        private void DrawCoin()
        {
            try
            {
                Rectangle rect = new Rectangle(1, 1, 50, 50);
                foreach (Vector2 coin in coins)
                {
                    spriteBatch.Draw(coinTexture, coin, Color.White);
                }
            }
            catch (Exception e)
            {
            }
        }
        private void DrawPlayers()
        {
            
            foreach (Player player in players)
            {

                int d = player.getDirection();
                Vector2 po = player.getPossition();
                float angle = (float)Math.PI / 2.0f;
                check(po);
                spriteBatch.Draw(tankTexture, po, null, player.Color, angle * d, new Vector2(25,25),1, SpriteEffects.None,1);
            }
        }

        private void check(Vector2 vec)
        {
            bool ckCoin = coins.Contains(vec);
            if (ckCoin)
                coins.Remove(vec);
            bool ckLife = lifePacks.Contains(vec);
            if (ckLife)
                lifePacks.Remove(vec);

        }

        private void CreateForeground()
        {
            //Color[,] groundColors = TextureTo2DArray(groundTexture);
            Color[] foregroundColors = new Color[screenWidth * screenHeight];

            for (int x = 0; x <= 500; x++)
            {
                for (int y = 0; y <= 500; y++)
                {
                    if (y % 50 == 0 || x % 50 == 0)
                        foregroundColors[x + y * screenWidth] = Color.White;
                    else
                        foregroundColors[x + y * screenWidth] = Color.Transparent;
                }
            }

            foregroundTexture = new Texture2D(device, screenWidth, screenHeight, false, SurfaceFormat.Color);
            foregroundTexture.SetData(foregroundColors);
      
        }

        private void ProcessKeyboard()
        {
            KeyboardState keybState = Keyboard.GetState();
            if (keybState.IsKeyDown(Keys.Left))
                client.send("LEFT#");
            if (keybState.IsKeyDown(Keys.Right))
                client.send("RIGHT#");
            if (keybState.IsKeyDown(Keys.Down))
                client.send("DOWN#");
            if (keybState.IsKeyDown(Keys.Up))
                client.send("UP#");
            if (keybState.IsKeyDown(Keys.Enter) || keybState.IsKeyDown(Keys.Space))
                client.send("SHOOT#");
            
        }

    }
}
