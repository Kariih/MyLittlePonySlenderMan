
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

namespace MyLittlePonySlenderMan
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

    #region Sound
        private SoundEffect _backgroundMusic;
    #endregion

        Background _background;
        Ponies _pony;
        Items _item;
        private Slender _slender;
        Ponybuttons _ponyButtons;
        private Vector2 _cameraPosition;
        private float _speed = 1.2f;
        private Texture2D _blackHole;
        SpriteFont _font;
        private Texture2D _cursor;
        private Vector2 _cursorPosition;
        

        private bool _isPlaying;

        public Game1()
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
           _background = new Background();
           _ponyButtons = new Ponybuttons();
           _cameraPosition = new Vector2(250, 250);
           _slender = new Slender(new Vector2(500, 500));
           _item = new Items();
           this.IsMouseVisible = false;

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
            _background.LoadContent(Content);
            _ponyButtons.loadButtons(Content);
            _blackHole = Content.Load<Texture2D>("blackhole");
            _font = Content.Load<SpriteFont>("SpriteFont1");
            _cursor = Content.Load<Texture2D>("cursor");
            _slender.Load(Content);

            #region Sound
            _backgroundMusic = Content.Load<SoundEffect>("MySlenderPony2");

            SoundEffectInstance instance = _backgroundMusic.CreateInstance();
            instance.IsLooped = true;

            _backgroundMusic.Play();            
            #endregion
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
           
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

            MouseState mouseState = Mouse.GetState();
            _cursorPosition = new Vector2(mouseState.X, mouseState.Y);

            if (_isPlaying)
            {
                _pony.Update(gameTime);
                _slender.Update(gameTime, _cameraPosition + new Vector2(380, 220));
                #region keyboard
                KeyboardState keyboardState = Keyboard.GetState();

                if (keyboardState.IsKeyDown(Keys.Escape))
                {
                    _isPlaying = false;
                    _ponyButtons.HasChosen = false;
                }

                if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D))
                {
                    _cameraPosition.X += _speed;
                }
                if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A))
                {
                    _cameraPosition.X -= _speed;
                }
                if (keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W))
                {
                    _cameraPosition.Y -= _speed;
                }
                if (keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S))
                {
                    _cameraPosition.Y += _speed;
                }
                #endregion

                GamePadState gs = GamePad.GetState(PlayerIndex.One);

                if (gs.ThumbSticks.Left.Length() != 0)
                {
                    _cameraPosition += gs.ThumbSticks.Left * new Vector2(_speed, -_speed);
                }

                float minX = -150;
                float maxX = _background.Bounds.Width - Window.ClientBounds.Width + 150;
                float minY = -70;
                float maxY = _background.Bounds.Height - Window.ClientBounds.Height + 70;


                if (_cameraPosition.X < minX)
                {
                    _cameraPosition.X = minX;
                }
                if (_cameraPosition.X >= maxX)
                {
                    _cameraPosition.X = maxX;
                }
                if (_cameraPosition.Y < minY)
                {
                    _cameraPosition.Y = minY;
                }
                if (_cameraPosition.Y >= maxY)
                {
                    _cameraPosition.Y = maxY;
                }
            }
            else
            {
                _ponyButtons.Update();
                if (_ponyButtons.HasChosen)
                {
                    _pony = new Ponies(_ponyButtons.Choice);
                    _pony.LoadPonies(Content);
                    _isPlaying = true;
                }
            }


            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin();
            _background.Draw(spriteBatch, _cameraPosition);
            if(_pony != null)
                _pony.Draw(spriteBatch);
            _slender.Draw(spriteBatch, _cameraPosition);
            _item.DrawItems(spriteBatch);
            spriteBatch.Draw(_blackHole, Vector2.Zero, Color.White);
            if (!_isPlaying)
            {
                _ponyButtons.Draw(spriteBatch);
                spriteBatch.DrawString(_font, "Velg en pony", new Vector2(100, 5), Color.White);
            }
            else
                spriteBatch.DrawString(_font, "Trykk \"esc\" hvis du vil velge ny pony", new Vector2(50, 0), Color.White);
            spriteBatch.Draw(_cursor, _cursorPosition - new Vector2(30, 30), Color.White);
            _item.DrawList(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
