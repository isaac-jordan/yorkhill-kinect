using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System.Threading.Tasks;
using SForms = System.Windows.Forms;
using SDrawing = System.Drawing;

using kinectApp.Entities;
using kinectApp.Utilities;
using kinectApp.Entities.UI;
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
        double millisecondsLeftOfGame = 60 * 1000;
        double restartTimer = 0.0;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        InputHelper iInputHelper;
        Texture2D jointMarker;
        Texture2D overlay;
        Texture2D room;
        Texture2D cloth;
        RenderTarget2D colorRenderTarget;
        SpriteFont font;
        public int screenHeight;
        public int screenWidth;
        public int depthHeight;
        public int depthWidth;

        public KinectAdapter iKinect;

        SceneManager iSceneManager;
        EntityManager entityManager;

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

            // Hardcoded values for 1080p screen.
            // You can use http://andrew.hedges.name/experiments/aspect_ratio/ to work out what to use.
            // Set H1: 424, W1: 512
            screenHeight = screenHeight - 100;
            screenWidth = (int)(screenHeight * (512 / (float)424));

            graphics.PreferredBackBufferHeight = screenHeight;
            graphics.PreferredBackBufferWidth = screenWidth;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            entityManager = new EntityManager();
            iSceneManager = new SceneManager(Content);

            iInputHelper = new InputHelper();

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
                ((SForms.Form)SForms.Form.FromHandle(Window.Handle)).Icon = new SDrawing.Icon(filename);
            });
            iKinect.OpenSensor();

            //Show Main menu
            iSceneManager.SetScene(new Entities.Scenes.GameInstance());
            colorRenderTarget = new RenderTarget2D(graphics.GraphicsDevice, 512, 424);

            //gestureRV = new GestureResultView(0, false, false, 0);
            //gestureDet = new GestureDetector(iKinect.iSensor, gestureRV);

            iKinect.OpenSensor();
            depthHeight = iKinect.iSensor.DepthFrameSource.FrameDescription.Height;
            depthWidth = iKinect.iSensor.DepthFrameSource.FrameDescription.Width;

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
            cloth = Content.Load<Texture2D>("cleaningcloth");
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

            if (iKinect.IsAvailable && iKinect.KinectJoints.Count > 0)
            {
                //A restart once the player has ended!
                if (millisecondsLeftOfGame <= 0)
                {
                    Point[] joints;
                    bool IsJointIn = false;

                    lock(iKinect.KinectJoints)  { joints = iKinect.KinectJoints.ToArray(); }
                    foreach (var j in joints)
                    {
                        //If the joint is in the corner
                        if ((j.X > screenWidth / 2) && (j.Y < 50)) {  IsJointIn = true; }
                    }

                    if (IsJointIn)
                    {
                        if ((gameTime.TotalGameTime.TotalMilliseconds - restartTimer) > 1500)
                        {
                            Restart();
                        }
                    }
                    else { restartTimer = gameTime.TotalGameTime.TotalMilliseconds; }
                }
                else
                {
                    //Update gametime
                    millisecondsLeftOfGame -= gameTime.ElapsedGameTime.TotalMilliseconds;

                    //Spawn some newbs
                    if (gameTime.TotalGameTime.TotalMilliseconds > lastSpawnTimeStamp + millisecondSpawnTimer || lastSpawnTimeStamp < 0)
                    {
                        var germ = (rand.NextDouble() < 0.2) ? GermFactory.CreateBigGerm() : GermFactory.CreateSmallGerm();
                        germ.Load(Content);
                        germs.Add(germ);
                        lastSpawnTimeStamp = gameTime.TotalGameTime.TotalMilliseconds;

                        millisecondSpawnTimer -= 9; // Reduce germ spawn timer by 0.09s each time one is spawned. 
                    }

                    //Get all the joints from the Kinect
                    Point[] joints;
                    lock (iKinect.KinectJoints)
                    {
                        joints = iKinect.KinectJoints.ToArray();
                    }

                    if (germs.Count > 0)
                        Console.WriteLine("Last germ at: x:" + germs.Last().PosX + ", y:" + germs.Last().PosY);

                    //Process all germs for hitting as well as killing them off
                    for (int i = germs.Count - 1; i >= 0; i--)
                    {
                        var germ = (GermBase)germs[i];
                        germ.Update(gameTime);

                        foreach (Point p in joints)
                        {
                            // Check X bounds
                            if (germ.PosX < p.X && p.X < germ.PosX + germ.Width + 30)
                            {
                                // Check Y bounds
                                if (germ.PosY < p.Y && p.Y < germ.PosY + germ.Height + 30)
                                {
                                    germ.Health -= 100;

                                    if (germ.IsDead)
                                    {
                                        if (germ is BigGerm)
                                        {
                                            score += 25;
                                        }
                                        else
                                        {
                                            score += 10;
                                        }
                                    }
                                }
                            }
                        }

                        //If the germ is off the screen or has been killed - kill it off.
                        if (germ.IsDead)
                        {
                            Task.Factory.StartNew(() => germ.Unload());
                            germs.RemoveAt(i);
                            break;
                        }
                    }
                }

                iInputHelper.Update();

                if (iInputHelper.IsNewPress(Keys.E))
                {
                    millisecondsLeftOfGame = 0;
                }

                iSceneManager.DoKeys(iInputHelper);
                iSceneManager.UpdateScene(gameTime);

                base.Update(gameTime);

            }
        }

        private void Restart()
        {
            millisecondsLeftOfGame = 60 * 1000;
            score = 0;
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

            spriteBatch.Begin();
            spriteBatch.Draw(room, new Rectangle(0, 0, depthWidth, depthHeight), Color.White);
            if (iKinect.KinectRGBVideo != null)
            {
                spriteBatch.Draw(iKinect.KinectRGBVideo, new Rectangle(0, 0, depthWidth, depthHeight), Color.White);
            }
            

            foreach (GermBase germ in germs)
            {
                germ.Draw(spriteBatch);
            }

            if (joints != null)
            {
                foreach (var J in joints)
                {
                    spriteBatch.Draw(cloth, new Rectangle(J.X - 15, J.Y - 15, 30, 30), Color.White);
                }
            }

            Label lab1 = new Label("Score: " + score, "label", 5, 5, 0);
            lab1.Load(Content);
            lab1.Draw(spriteBatch);

            ScoreLabel lab2 = new ScoreLabel("Time Left: " + (int)(millisecondsLeftOfGame / 1000), "label", depthWidth - 300, 5, 0);
            lab2.Load(Content);
            lab2.Draw(spriteBatch);

            if (millisecondsLeftOfGame <= 0)
            {
                var RestartLabel = new Label("Restart?", string.Empty, depthWidth - 100, 5, 0);
                RestartLabel.Load(Content);
                RestartLabel.Draw(spriteBatch);
            }


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
