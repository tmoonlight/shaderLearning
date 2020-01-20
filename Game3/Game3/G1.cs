using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game3
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class G1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private SpriteFont spriteFont;
        private Texture2D frigateTexture;
        private Effect effect;

        public G1()
        {
            graphics = new GraphicsDeviceManager(this);
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
            frigateTexture = Content.Load<Texture2D>("testpng");
            effect = Content.Load<Effect>("File");
            spriteFont = Content.Load<SpriteFont>("myfont");
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            
            // TODO: Unload any non ContentManager content here
        }

        private string str = "";
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            
            // TODO: Add your update logic here
            str = gameTime.ElapsedGameTime.ToString()+" slow"+gameTime.IsRunningSlowly.ToString()+"thread:"+Thread.CurrentThread.ManagedThreadId;
            Thread.Sleep(1);//不管sleep多久
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(0xf0, 0xf0, 0xf0, 0xff));

            // TODO: Add your drawing code here
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            effect.CurrentTechnique.Passes[0].Apply();
            //spriteBatch.Draw(frigateTexture, Vector2.Zero, null, Color.White, 0.5f, Vector2.Zero, 0.3f, SpriteEffects.None, 0);
            spriteBatch.DrawString(spriteFont, str, new Vector2(100, 300), Color.Black);

            spriteBatch.DrawString(spriteFont, "thread:" + Thread.CurrentThread.ManagedThreadId, new Vector2(200, 400), Color.Black);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
