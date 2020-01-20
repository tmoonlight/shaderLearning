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

namespace Randomchaos2DGodRays
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        RenderTarget2D scene;

        PostProcessingManager ppm;
        CrepuscularRays rays;
        Vector2 rayStartingPos = Vector2.One * .5f;

        List<string> backgroundAssets = new List<string>();
        List<float> bgSpeeds = new List<float>();

        List<Vector2> bgPos = new List<Vector2>();
        List<Vector2> bgPos2 = new List<Vector2>();
        List<Vector2> bgPos2Base = new List<Vector2>();

        int bgCnt;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            IsMouseVisible = true;

            //graphics.IsFullScreen = true;
            
            graphics.PreferredBackBufferHeight = 900;
            graphics.PreferredBackBufferWidth = 1440;

            graphics.PreferredBackBufferHeight = 600;
            graphics.PreferredBackBufferWidth = 800;

            ppm = new PostProcessingManager(this);

            rays = new CrepuscularRays(this, Vector2.One * .5f, "Textures/flare", 1, .97f, .97f, .5f, .25f);
            
            ppm.AddEffect(rays);
            
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
            Services.AddService(spriteBatch.GetType(), spriteBatch);

            rays.lightSource = rayStartingPos;

            scene = new RenderTarget2D(GraphicsDevice, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, false, SurfaceFormat.Color, DepthFormat.None);

            AddBackground("Textures/Backgrounds/BGForeGround_1_1", 2f);
            AddBackground("Textures/Backgrounds/BGForeGround_1_2", 3f);
            AddBackground("Textures/Backgrounds/BGForeGround_1_3", 4f);
        }

        public virtual void AddBackground(string bgAsset, float speed)
        {
            backgroundAssets.Add(bgAsset);
            bgSpeeds.Add(speed);
            bgPos.Add(Vector2.Zero);
            bgCnt++;

            bgPos2.Add(new Vector2(GraphicsDevice.Viewport.Width, 0));
            bgPos2Base.Add(new Vector2(GraphicsDevice.Viewport.Width, 0));
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

            if (Keyboard.GetState().IsKeyDown(Keys.F1))
                rays.lightTexture = Content.Load<Texture2D>("Textures/flare");
            if (Keyboard.GetState().IsKeyDown(Keys.F2))
                rays.lightTexture = Content.Load<Texture2D>("Textures/flare2");
            if (Keyboard.GetState().IsKeyDown(Keys.F3))
                rays.lightTexture = Content.Load<Texture2D>("Textures/flare3");
            if (Keyboard.GetState().IsKeyDown(Keys.F4))
                rays.lightTexture = Content.Load<Texture2D>("Textures/flare4");

            if (Keyboard.GetState().IsKeyDown(Keys.W))
                rays.lightSource = new Vector2(rays.lightSource.X, rays.lightSource.Y - .01f);
            if (Keyboard.GetState().IsKeyDown(Keys.S))
                rays.lightSource = new Vector2(rays.lightSource.X, rays.lightSource.Y + .01f);
            if (Keyboard.GetState().IsKeyDown(Keys.A))
                rays.lightSource = new Vector2(rays.lightSource.X - .01f, rays.lightSource.Y);
            if (Keyboard.GetState().IsKeyDown(Keys.D))
                rays.lightSource = new Vector2(rays.lightSource.X + .01f, rays.lightSource.Y);

            if (Keyboard.GetState().IsKeyDown(Keys.Q))
                rays.Exposure += .01f;
            if (Keyboard.GetState().IsKeyDown(Keys.Z))
                rays.Exposure -= .01f;

            if (Keyboard.GetState().IsKeyDown(Keys.E))
                rays.LightSourceSize += .01f;
            if (Keyboard.GetState().IsKeyDown(Keys.C))
                rays.LightSourceSize -= .01f;

            if (Keyboard.GetState().IsKeyDown(Keys.R))
                rays.Density += .01f;
            if (Keyboard.GetState().IsKeyDown(Keys.V))
                rays.Density -= .01f;

            if (Keyboard.GetState().IsKeyDown(Keys.T))
                rays.Decay += .01f;
            if (Keyboard.GetState().IsKeyDown(Keys.B))
                rays.Decay -= .01f;

            if (Keyboard.GetState().IsKeyDown(Keys.Y))
                rays.Weight += .01f;
            if (Keyboard.GetState().IsKeyDown(Keys.N))
                rays.Weight -= .01f;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // This looks WAY more comlicated than it is, but it's pretty simple,
            // Draw the objects you wan to have occlude the rays in black and store them in an RT, this creates a mask.
            // Hand this RT onto the pp manager, it will render all the rays for you to a new RT
            // Re render the scene in color this time to another RT
            // Finaly do an additive blend with the godray RT and the scene you just rendered.
            // Give me a shout if you need any help implementing it :D

            // Draw the stuff that is infront of the rays source
            GraphicsDevice.SetRenderTarget(scene);
            GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
            for (int bg = 0; bg < bgCnt; bg++)
            {
                spriteBatch.Draw(Content.Load<Texture2D>(backgroundAssets[bg]), new Rectangle((int)bgPos[bg].X, (int)bgPos[bg].Y, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), new Rectangle(0, 0, Content.Load<Texture2D>(backgroundAssets[bg]).Width, Content.Load<Texture2D>(backgroundAssets[bg]).Height), Color.Black);
                spriteBatch.Draw(Content.Load<Texture2D>(backgroundAssets[bg]), new Rectangle((int)bgPos2[bg].X, (int)bgPos[bg].Y, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), new Rectangle(0, 0, Content.Load<Texture2D>(backgroundAssets[bg]).Width, Content.Load<Texture2D>(backgroundAssets[bg]).Height), Color.Black);
            }
            // Draw mouse icon mask.
            spriteBatch.Draw(Content.Load<Texture2D>("Textures/mousemask"), new Rectangle(Mouse.GetState().X - 64, Mouse.GetState().Y - 64, 128, 128), Color.Black);
            spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);

            // Apply the post processing maanger (just the rays in this one)
            ppm.Draw(gameTime, scene);


            // Now blend that source with the scene..
            GraphicsDevice.SetRenderTarget(scene);
            // Draw the scene in color now
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);

            for (int bg = 0; bg < bgCnt; bg++)
            {
                spriteBatch.Draw(Content.Load<Texture2D>(backgroundAssets[bg]), new Rectangle((int)bgPos[bg].X, (int)bgPos[bg].Y, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), new Rectangle(0, 0, Content.Load<Texture2D>(backgroundAssets[bg]).Width, Content.Load<Texture2D>(backgroundAssets[bg]).Height), Color.White);
                spriteBatch.Draw(Content.Load<Texture2D>(backgroundAssets[bg]), new Rectangle((int)bgPos2[bg].X, (int)bgPos[bg].Y, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), new Rectangle(0, 0, Content.Load<Texture2D>(backgroundAssets[bg]).Width, Content.Load<Texture2D>(backgroundAssets[bg]).Height), Color.White);
            }
            // Draw mouse icon.
            spriteBatch.Draw(Content.Load<Texture2D>("Textures/mousemask"), new Rectangle(Mouse.GetState().X - 64, Mouse.GetState().Y - 64, 128, 128), Color.White);
            spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);

            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);
            spriteBatch.Draw(ppm.Scene, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
            spriteBatch.Draw(scene, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
            spriteBatch.End();

            for (int b = 0; b < bgCnt; b++)
            {
                bgPos[b] -= new Vector2(bgSpeeds[b], 0);
                bgPos2[b] -= new Vector2(bgSpeeds[b], 0);

                if (bgPos[b].X < -bgPos2Base[b].X)
                    bgPos[b] = new Vector2(bgPos2[b].X + bgPos2Base[b].X, 0);

                if (bgPos2[b].X < -bgPos2Base[b].X)
                    bgPos2[b] = new Vector2(bgPos[b].X + bgPos2Base[b].X, 0);
            }

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);
            spriteBatch.DrawString(Content.Load<SpriteFont>("Fonts/font"), "F1 - F4 change light source texture", new Vector2(8, Content.Load<SpriteFont>("Fonts/font").LineSpacing * 1), Color.Gold);
            spriteBatch.DrawString(Content.Load<SpriteFont>("Fonts/font"), string.Format("WASD move light source [{0}]", rays.lightSource), new Vector2(8, Content.Load<SpriteFont>("Fonts/font").LineSpacing * 2), Color.Gold);
            spriteBatch.DrawString(Content.Load<SpriteFont>("Fonts/font"), string.Format("Q/Z Exposure up/down [{0}]", rays.Exposure), new Vector2(8, Content.Load<SpriteFont>("Fonts/font").LineSpacing * 3), Color.Gold);
            spriteBatch.DrawString(Content.Load<SpriteFont>("Fonts/font"), string.Format("E/C Size up/down [{0}]", rays.LightSourceSize), new Vector2(8, Content.Load<SpriteFont>("Fonts/font").LineSpacing * 4), Color.Gold);
            spriteBatch.DrawString(Content.Load<SpriteFont>("Fonts/font"), string.Format("R/V Density up/down [{0}]", rays.Density), new Vector2(8, Content.Load<SpriteFont>("Fonts/font").LineSpacing * 5), Color.Gold);
            spriteBatch.DrawString(Content.Load<SpriteFont>("Fonts/font"), string.Format("T/B Decay up/down [{0}]", rays.Decay), new Vector2(8, Content.Load<SpriteFont>("Fonts/font").LineSpacing * 6), Color.Gold);
            spriteBatch.DrawString(Content.Load<SpriteFont>("Fonts/font"), string.Format("Y/N Weight up/down [{0}]", rays.Weight), new Vector2(8, Content.Load<SpriteFont>("Fonts/font").LineSpacing * 7), Color.Gold);
            spriteBatch.End();
        }
    }
}
