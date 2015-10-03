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
        Texture2D kinectRGBVideo;
        Texture2D overlay;
        SpriteFont font;

        KinectAdapter iKinect;

        KinectSensor sensor;
        ColorFrameReader cfReader;
        string connectedStatus;
        byte[] _colorImageBuffer;
        bool _colorIsDrawing;

        readonly SceneManager iSceneManager;
        readonly EntityManager entityManager;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            int actualHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 256;
            int actualWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - 256;

            graphics.PreferredBackBufferHeight = actualHeight;
            graphics.PreferredBackBufferWidth = actualWidth;
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
            // TODO: Add your initialization logic here
            iKinect = new KinectAdapter(graphics.GraphicsDevice);

            iKinect.OpenSensor();

            //sensor = KinectSensor.GetDefault();
            //sensor.IsAvailableChanged += KinectSensors_StatusChanged;
            
            //sensor.Open();
            //cfReader = sensor.ColorFrameSource.OpenReader();
            //cfReader.FrameArrived += kinectSensor_ColorFrameArrived;


            iSceneManager.SetScene(new Entities.Scenes.Menu());

            //for (int i = 0; i < 250; i++)
            //{
            //    entityManager.AddEntity(Entities.Germs.GermFactory.CreateSmallGerm());
            //}

            //entityManager.AddEntity(new Entities.UI.Button("Test", string.Empty, 20, 20, 0));

            base.Initialize();
        }

        private void KinectSensors_StatusChanged(object sender, IsAvailableChangedEventArgs e)
        {
            connectedStatus = e.IsAvailable ? "Sensor is available." : "**Sensor is not available**";
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            kinectRGBVideo = new Texture2D(GraphicsDevice, 1920, 1080);

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
            if (Keyboard.GetState().GetPressedKeys().Contains(Keys.Escape))
                this.Exit();

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

            if (iKinect.KinectRGBVideo != null)
            {
                spriteBatch.Draw(iKinect.KinectRGBVideo, new Rectangle(0, 0, 1900, 1000), Color.White);
            }

            //spriteBatch.Draw(kinectRGBVideo, new Rectangle(0, 0, 1900, 1000), Color.White);
            //spriteBatch.Draw(overlay, new Rectangle(0, 0, 640, 480), Color.White);
            spriteBatch.DrawString(font, iKinect.ConnectedStatus, new Vector2(20, 80), Color.White);

            iSceneManager.DrawScene(gameTime, spriteBatch);
            //entityManager.Draw(gameTime,spriteBatch);

            spriteBatch.End();

            // TODO: Add your drawing code here
            base.Draw(gameTime);
        }

        protected override void EndDraw()
        {
            _colorIsDrawing = false;
            base.EndDraw();
        }

        private void kinectSensor_ColorFrameArrived(object sender, ColorFrameArrivedEventArgs e)
        {
            if (_colorIsDrawing) return;
            _colorIsDrawing = true;
            using (ColorFrame colorImageFrame = e.FrameReference.AcquireFrame())
            {
                if (colorImageFrame != null)
                {
                    int width = colorImageFrame.FrameDescription.Width;
                    int height = colorImageFrame.FrameDescription.Height;

                    if ((_colorImageBuffer == null) || (_colorImageBuffer.Length != width * height * /*colorImageFrame.FrameDescription.BytesPerPixel*/ 4))
                    {
                        _colorImageBuffer = new byte[width * height * /*colorImageFrame.FrameDescription.BytesPerPixel*/ 4];
                    }
                    
                    colorImageFrame.CopyConvertedFrameDataToArray(_colorImageBuffer, ColorImageFormat.Rgba);

                    Color[] color = new Color[height * width];
                    kinectRGBVideo = new Texture2D(graphics.GraphicsDevice, width, height);

                    // Go through each pixel and set the bytes correctly
                    // Remember, each pixel got a Red, Green and Blue
                    int index = 0;
                    for (int y = 0; y < width; y++)
                    {
                        for (int x = 0; x < height; x++)
                        {
                            Color c = new Color(_colorImageBuffer[index + 0], _colorImageBuffer[index + 1], _colorImageBuffer[index + 2], _colorImageBuffer[index + 3]);
                            color[y * height + x] = c;
                            index += 4;
                        }
                    }

                    // Set pixeldata from the ColorImageFrame to a Texture2D
                    kinectRGBVideo.SetData(color);
                    this.BeginDraw();
                }
            }
        }
    }
}
