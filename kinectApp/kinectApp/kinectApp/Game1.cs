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

using System.Windows.Forms;
using SDrawing = System.Drawing;

using kinectApp.Entities;
using kinectApp.Utilities;

namespace kinectApp
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        InputHelper iInputHelper;
        Texture2D jointMarker;
        Texture2D overlay;
        RenderTarget2D colorRenderTarget;
        SpriteFont font;
        int screenHeight;
        int screenWidth;

        KinectAdapter iKinect;
        GestureResultView gestureRV;
        GestureDetector gestureDet;

        readonly SceneManager iSceneManager;
        readonly EntityManager entityManager;

        static bool iCancelRequested = false;

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

            iInputHelper = new InputHelper();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            
            iKinect = new KinectAdapter(graphics.GraphicsDevice, (isAvail) =>
            {
                Window.Title = string.Format("Germz | Dynamic Dorks [{0}]", (isAvail) ? "Connected" : "NO KINECT FOUND");

                var filename = string.Format("Res/{0}", (isAvail) ? "Germz.Icon.ico" : "Germz.NoKintec.Icon.ico");

                ((System.Windows.Forms.Form)System.Windows.Forms.Form.FromHandle(Window.Handle)).Icon = new SDrawing.Icon(filename);
            });
            iKinect.OpenSensor();

            //Show Main menu
            iSceneManager.SetScene(new Entities.Scenes.Menu());
            colorRenderTarget = new RenderTarget2D(graphics.GraphicsDevice, KinectAdapter.kWidth, KinectAdapter.kHeight);

            gestureRV = new GestureResultView(0, false, false, 0);
            gestureDet = new GestureDetector(iKinect.iSensor, gestureRV);

            iKinect.OpenSensor();

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
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().GetPressedKeys().Contains(Microsoft.Xna.Framework.Input.Keys.Escape) || iCancelRequested || gestureRV.Detected)
            //Dectect a close, from outwith this class!
            {
                this.Exit();
            }

            iInputHelper.Update();

            iSceneManager.DoKeys(iInputHelper);
            iSceneManager.UpdateScene(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            Point[] joints;
            lock (iKinect.KinectJoints)
            {
                joints = iKinect.KinectJoints.ToArray();
            }

            GraphicsDevice.SetRenderTarget(colorRenderTarget);

            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            if (iKinect.KinectRGBVideo != null)
            {
                spriteBatch.Draw(iKinect.KinectRGBVideo, new Rectangle(0, 0, KinectAdapter.kWidth, KinectAdapter.kHeight), Color.White);
            }
            if (joints != null)
            {
                foreach (var J in joints)
                {
                    spriteBatch.Draw(jointMarker, new Rectangle(J.X, J.Y, 10, 10), Color.White);
                }
            }
            spriteBatch.End();

            // Reset the device to the back buffer
            GraphicsDevice.SetRenderTarget(null);

            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            //Drawing the video feed if we have one available.
            spriteBatch.Draw(colorRenderTarget, new Rectangle(0, 0, screenWidth, screenHeight), Color.White);

            //Drawing the connection string on top of screen
            spriteBatch.DrawString(font, iKinect.ConnectedStatus, new Vector2(0, 0), Color.White);
            

            //Now we draw whatever scene is currently in the game!
            iSceneManager.DrawScene(gameTime, spriteBatch);

            spriteBatch.End();
            base.Draw(gameTime);
        }

        //Allows A forced close of the application.
        public static void ForceClose()
        {
            iCancelRequested = true;
        }
    }
}
