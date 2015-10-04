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

using SForms = System.Windows.Forms;
using SDrawing = System.Drawing;

using kinectApp.Entities;
using kinectApp.Utilities;
using kinectApp.Entities.Scenes;
using kinectApp.Entities.Germs;

namespace kinectApp
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        int millisecondSpawnTimer = 1000;
        double lastSpawnTimeStamp = -1;
        Random rand = new Random();

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        InputHelper iInputHelper;
        Texture2D jointMarker;
        Texture2D overlay;
        Texture2D room;
        Texture2D smallGermTexture;
        Texture2D bigGermTexture;
        RenderTarget2D colorRenderTarget;
        SpriteFont font;
        public int screenHeight;
        public int screenWidth;

        public KinectAdapter iKinect;
        GestureResultView gestureRV;
        GestureDetector gestureDet;

        readonly SceneManager iSceneManager;
        readonly EntityManager entityManager;

        List<IEntity> germs = new List<IEntity>();

        static bool iCancelRequested = false;

        readonly Color iBackground = Color.Purple;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            screenHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 100;
            screenWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - 125;

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
                string title = null;
                string file = null;

                if (isAvail)
                {
                    title = "Connected";
                    file = "Germz.Icon.ico";

                    iSceneManager.HideOverlay();
                }
                else
                {
                    title = "NO KINECT FOUND";
                    file = "Germz.NoKintec.Icon.ico";

                    iSceneManager.ShowOverlay(new KinectDisconnect());
                }

                Window.Title = string.Format("Germz | Dynamic Dorks [{0}]", title);
                var filename = string.Format("Res/{0}", file);
                ((System.Windows.Forms.Form)System.Windows.Forms.Form.FromHandle(Window.Handle)).Icon = new SDrawing.Icon(filename);
            });
            iKinect.OpenSensor();

            //Show Main menu
            iSceneManager.SetScene(new Entities.Scenes.GameInstance());
            colorRenderTarget = new RenderTarget2D(graphics.GraphicsDevice, KinectAdapter.kWidth, KinectAdapter.kHeight);

            //gestureRV = new GestureResultView(0, false, false, 0);
            //gestureDet = new GestureDetector(iKinect.iSensor, gestureRV);

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
            room = Content.Load<Texture2D>("room");
            font = Content.Load<SpriteFont>("SpriteFont1");
            smallGermTexture = Content.Load<Texture2D>("SmallGerm");
            bigGermTexture = Content.Load<Texture2D>("SmallGerm"); // TODO: BigGerm texture?


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
            if (Keyboard.GetState().GetPressedKeys().Contains(Keys.Escape) || iCancelRequested)
            //Dectect a close, from outwith this class!
            {
                this.Exit();
            }

            if (gameTime.TotalGameTime.TotalMilliseconds > lastSpawnTimeStamp + millisecondSpawnTimer)
            {
                germs.Add(rand.Next(100) < 20 ? GermFactory.CreateBigGerm() : GermFactory.CreateSmallGerm());
                lastSpawnTimeStamp = gameTime.TotalGameTime.TotalMilliseconds;
            }

            Point[] joints;
            lock (iKinect.KinectJoints)
            {
                joints = iKinect.KinectJoints.ToArray();
            }

            for (int i=germs.Count - 1; i >= 0; i--)
            {
                germs[i].Update(gameTime);
                foreach (Point p in joints)
                {
                    if (germs[i].PosX + 20 > p.X && germs[i].PosX < p.X && germs[i].PosY + 20 > p.Y && germs[i].PosY < p.Y)
                    {
                        germs.RemoveAt(i);
                    }
                }
            }

            iInputHelper.Update();

            iSceneManager.DoKeys(iInputHelper);
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
            Point[] joints;
            lock (iKinect.KinectJoints)
            {
                joints = iKinect.KinectJoints.ToArray();
            }

            GraphicsDevice.Clear(iBackground);

            GraphicsDevice.SetRenderTarget(colorRenderTarget);

            int depthHeight = iKinect.iSensor.DepthFrameSource.FrameDescription.Height;
            int depthWidth = iKinect.iSensor.DepthFrameSource.FrameDescription.Width;

            spriteBatch.Begin();
            spriteBatch.Draw(room, new Rectangle(0, 0, KinectAdapter.kWidth, KinectAdapter.kHeight), Color.White);
            if (iKinect.KinectRGBVideo != null)
            {
                spriteBatch.Draw(iKinect.KinectRGBVideo, new Rectangle(0, 0, KinectAdapter.kWidth, KinectAdapter.kHeight), Color.White);
            }
            if (joints != null)
            {
                foreach (var J in joints)
                {
                    spriteBatch.Draw(jointMarker, new Rectangle(J.X - 50, J.Y, 10, 10), Color.White);
                }
            }
            spriteBatch.End();

            // Reset the device to the back buffer
            GraphicsDevice.SetRenderTarget(null);
            

            spriteBatch.Begin();

            //entityManager.Draw(gameTime,spriteBatch);
            //Drawing the video feed if we have one available.
            spriteBatch.Draw(colorRenderTarget, new Rectangle(0, 0, screenWidth, screenHeight), Color.White);

            foreach (GermBase germ in germs)
            {
                if (germ is SmallGerm)
                {
                    germ.Texture = smallGermTexture;
                }
                else if (germ is BigGerm)
                {
                    germ.Texture = bigGermTexture;
                }
                
                germ.Draw(spriteBatch);
            }
           
            //No longer displaying the connection status on the screen because we have the title bar now >=]
            //Now we draw whatever scene is currently in the game!
            //iSceneManager.DrawScene(gameTime, spriteBatch);

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
