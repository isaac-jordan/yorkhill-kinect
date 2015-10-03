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
using Microsoft.Kinect;

using kinectApp.Entities;

namespace kinectApp
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D jointMarker;
        Texture2D overlay;
        SpriteFont font;
        int screenHeight;
        int screenWidth;

        KinectAdapter iKinect;
        readonly SceneManager iSceneManager;
        readonly EntityManager entityManager;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            screenHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 125;
            screenWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - 100;

            graphics.PreferredBackBufferHeight = screenHeight;
            graphics.PreferredBackBufferWidth = screenWidth;
            Content.RootDirectory = "Content";

            entityManager = new EntityManager();
            iSceneManager = new SceneManager(Content);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            iKinect = new KinectAdapter(graphics.GraphicsDevice);
            iKinect.OpenSensor();

            //Show Main menu
            iSceneManager.SetScene(new Entities.Scenes.Menu());

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            jointMarker = new Texture2D(GraphicsDevice, 50, 50);
            Color[] data = new Color[50 * 50];
            for (int i = 0; i < data.Length; ++i) data[i] = Color.Red;
            jointMarker.SetData(data);

            overlay = Content.Load<Texture2D>("overlay");
            font = Content.Load<SpriteFont>("SpriteFont1");

            // TODO: use this.Content to load your game content here#
            //entityManager.Load(Content);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here

            iSceneManager.Dispose();
            iKinect.Dispose();

            //entityManager.Unload();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            var PKeys = Keyboard.GetState().GetPressedKeys();

            foreach (var k in PKeys)
            {
                switch (k)
                {
                    case Keys.Escape:
                    case Keys.Q:
                    {
                        this.Exit();
                        break;
                    }

                    //DEBUG SCENE CHANGES

                    case Keys.Space:
                    {
                        iSceneManager.SetScene(new Entities.Scenes.GameInstance());
                        break;
                    }

                    case Keys.LeftAlt:
                    {
                        iSceneManager.SetScene(new Entities.Scenes.Menu());
                        break;
                }
            }



            // TODO: Add your update logic here
            iSceneManager.UpdateScene(gameTime);

            //entityManager.Update(gameTime);

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

            //Drawing the video feed if we have one available.
            if (iKinect.KinectRGBVideo != null)
            {
                spriteBatch.Draw(iKinect.KinectRGBVideo, new Rectangle(0, 0, screenWidth, screenHeight), Color.White);
            }

            //Drawing the connection string on top of screen
            spriteBatch.DrawString(font, iKinect.ConnectedStatus, new Vector2(0, 0), Color.White);

            //spriteBatch.Draw(kinectRGBVideo, new Rectangle(0, 0, 1900, 1000), Color.White);
            //spriteBatch.Draw(overlay, new Rectangle(0, 0, 640, 480), Color.White);
            Joint[] joints = iKinect.KinectJoints.ToArray();

            if (joints != null)
            {
                foreach (var J in joints)
                {
                    int x = screenWidth / 2 + (int)(J.Position.X * screenWidth);
                    int y = screenHeight / 2 - (int)(J.Position.Y * screenHeight);

                    spriteBatch.Draw(jointMarker, new Rectangle(x, y, 10, 10), Color.White);
#if DEBUG
                    Console.WriteLine(string.Format("Joint at: {0},{1}", J.Position.X, J.Position.Y));
#endif
                }
            }

            //Now we draw whatever scene is currently in the game!
            iSceneManager.DrawScene(gameTime, spriteBatch);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
