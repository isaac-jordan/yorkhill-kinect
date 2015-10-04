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

using System.Threading.Tasks;
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
        int score = 0;

        static bool iCancelRequested = false;

        readonly Color iBackground = Color.Purple;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            IntPtr hWnd = this.Window.Handle;
            var control = System.Windows.Forms.Control.FromHandle(hWnd);
            var form = control.FindForm();
            form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;

            screenHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            screenWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;

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

            if (iKinect.IsAvailable)
            {
                if (gameTime.TotalGameTime.TotalMilliseconds > lastSpawnTimeStamp + millisecondSpawnTimer || lastSpawnTimeStamp < 0)
                {
                    // Small germs are inivisible, so lets only create big ones currently.
                    var germ = (rand.NextDouble() < 0.2) ? GermFactory.CreateBigGerm() : GermFactory.CreateSmallGerm();
                    germ.Load(Content);
                    germs.Add(germ);
                    lastSpawnTimeStamp = gameTime.TotalGameTime.TotalMilliseconds;
                }

                Point[] joints;
                lock (iKinect.KinectJoints)
                {
                    joints = iKinect.KinectJoints.ToArray();
                }

                if (germs.Count > 0)
                    Console.WriteLine("Last germ at: x:" + germs.Last().PosX + ", y:" + germs.Last().PosY);

                for (int i = germs.Count - 1; i >= 0; i--)
                {
                    var germ = (GermBase)germs[i];
                    germ.Update(gameTime);

                    foreach (Point p in joints)
                    {
                        // Check X bounds
                        if (germ.PosX - 12 < p.X && p.X < germ.PosX + 12)
                        {
                            // Check Y bounds
                            if (germ.PosY + 40 > p.Y &&
                                p.Y < germ.PosY + 88)
                            {
                                germ.Health -= 10000;
                            }
                        }
                    }

                    //If the germ is off the screen or has been killed - kill it off.
                    if (germ.IsDead)
                    {
                        Task.Factory.StartNew(() => germ.Unload());
                        germs.RemoveAt(i);

                        if (germ is BigGerm)
                        {
                            score += 25;
                        }
                        else
                        {
                            score += 10;
                        }
                        break;
                    }
                }

                iInputHelper.Update();

                iSceneManager.DoKeys(iInputHelper);
                iSceneManager.UpdateScene(gameTime);

                base.Update(gameTime);
            }
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
                    spriteBatch.Draw(createCircleTexture(30), new Rectangle(J.X, J.Y, 30, 30), Color.White);
                }
            }

            foreach (GermBase germ in germs)
            {
                germ.Draw(spriteBatch);
            }

            Entities.UI.BigLabel lab = new Entities.UI.BigLabel("Score: " + score, "label", 50, 50, 50);
            lab.Load(Content);
            lab.Draw(spriteBatch);

            if (!spriteBatch.IsDisposed)
                spriteBatch.End();

            // Reset the device to the back buffer
            GraphicsDevice.SetRenderTarget(null);


            spriteBatch.Begin();

            //entityManager.Draw(gameTime,spriteBatch);
            //Drawing the video feed if we have one available.
            spriteBatch.Draw(colorRenderTarget, new Rectangle(0, 0, screenWidth, screenHeight), Color.White);



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

        Texture2D createCircleTexture(int radius)
        {
            Texture2D texture = new Texture2D(GraphicsDevice, radius, radius);
            Color[] colorData = new Color[radius * radius];

            float diam = radius / 2f;
            float diamsq = diam * diam;

            for (int x = 0; x < radius; x++)
            {
                for (int y = 0; y < radius; y++)
                {
                    int index = x * radius + y;
                    Vector2 pos = new Vector2(x - diam, y - diam);
                    if (pos.LengthSquared() <= diamsq)
                    {
                        colorData[index] = Color.White;
                    }
                    else
                    {
                        colorData[index] = Color.Transparent;
                    }
                }
            }

            texture.SetData(colorData);
            return texture;
        }
    }
}
