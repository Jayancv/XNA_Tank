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
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    /// 


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
        Player[] players = new Player[5];
        int[] terrainContour;
        Client client;
        public Player player0, player1, player2, player3, player4;
        private StringEvaluator eval;
        public List<Vector2> bricks;             //Store brick coordinates 
        public List<Vector2> stones;             //store Stone coordinates
        public List<Vector2> water1;             //store water corrdinates


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            client = new Client();
            eval = new StringEvaluator();
            bricks = new List<Vector2>();
            stones = new List<Vector2>();
            water1 = new List<Vector2>();


        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            client.send("JOIN#");
            IsMouseVisible = true;
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 650;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
            Window.Title = "JC Rocket";


            base.Initialize();

        }


        private void SetUpPlayers()
        {


            player0 = new Player();
            player1 = new Player();
            player2 = new Player();
            player3 = new Player();
            player4 = new Player();

            
            player1.Color = Color.Red;
            player2.Color = Color.Green;
            player3.Color = Color.Blue;
            player4.Color = Color.Purple;
            player0.Color = Color.Yellow;
            
            /*
            player1.Position = new Vector2(100, 193);
            player2.Position = new Vector2(200, 212);
            player3.Position = new Vector2(300, 361);
            player4.Position = new Vector2(400, 164);

             */

            players[0] = player0;
            players[1] = player1;
            players[2] = player2;
            players[3] = player3;
            players[4] = player4;


            for (int i = 0; i < 4; i++)
            {
                // players[i].IsAlive = true;

                //players[i].IsAlive = true;
                // players[i].Color = playerColors[i];


            }


        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            device = graphics.GraphicsDevice;


            //  Texture2D texture;



            backgroundTexture = Content.Load<Texture2D>("background");
            // backgroundTexture = Content.Load<Texture2D>("background");
            foregroundTexture = Content.Load<Texture2D>("background2");
            screenWidth = device.PresentationParameters.BackBufferWidth;
            screenHeight = device.PresentationParameters.BackBufferHeight;
            tankTexture = Content.Load<Texture2D>("tank1");
            brickTexture = Content.Load<Texture2D>("brick1");
            stoneTexture = Content.Load<Texture2D>("stone1");
            waterTexture = Content.Load<Texture2D>("water");

            //GenerateTerrainContour();
            CreateForeground();
            SetUpPlayers();
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

            // TODO: Add your update logic here
            eval.evaluate(client.data, this);
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
            DrawPlayers();
            DrawBricks();
            DrawStones();
            DrawWater();
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
                Rectangle rect = new Rectangle(1, 1, 48, 48);
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
                Rectangle rect = new Rectangle(1, 1, 48, 48);
                foreach (Vector2 stone in stones)
                {
                    spriteBatch.Draw(stoneTexture, stone, rect, Color.White);
                }
            }
            catch (Exception e)
            {


            }

        }
        private void DrawWater()
        {
            try
            {
                Rectangle rect = new Rectangle(1, 1, 48, 48);
                foreach (Vector2 water in water1)
                {
                    spriteBatch.Draw(waterTexture, water, rect, Color.White);
                }
            }
            catch (Exception e)
            {
            }
        }
        private void DrawPlayers()
        {
            Rectangle rect = new Rectangle(1, 1, 48,48);
            foreach (Player player in players)
            {
                spriteBatch.Draw(tankTexture, player.Position, rect, player.Color);
            }
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
            // foregroundColorArray = TextureTo2DArray(foregroundTexture);
        }

       
    }
}
