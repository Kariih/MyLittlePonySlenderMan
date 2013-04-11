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
        private SoundEffectInstance _backgroundMusic;
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
        private bool won;
        private bool lost;
        private bool backgroundMusicPlaying = true;
        private bool isPaused;


        private KeyboardState previousKeyboardState;

        private bool _firstPlay;
        

        private bool _isPlaying;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            _firstPlay = true;

  //          graphics.PreferredBackBufferWidth = 500;
  //          graphics.PreferredBackBufferHeight = 400;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            if (_firstPlay)
            {
                _background = new Background();
                _ponyButtons = new Ponybuttons();
            }
           _cameraPosition = new Vector2(250, 250);
           _slender = new Slender(new Vector2(700, 700));
           _item = new Items();
           this.IsMouseVisible = false;


            // Calls LoadContent
           base.Initialize();

            // start the background music now that the media has been loaded
            _backgroundMusic.Play();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            if(_firstPlay){
                // Create a new SpriteBatch, which can be used to draw textures.
                spriteBatch = new SpriteBatch(GraphicsDevice);
                _background.LoadContent(Content);
                _ponyButtons.loadButtons(Content);
                _blackHole = Content.Load<Texture2D>("blackhole");
                _font = Content.Load<SpriteFont>("SpriteFont1");
                _cursor = Content.Load<Texture2D>("cursor");
                

                #region Sound
                _backgroundMusic = Content.Load<SoundEffect>("MySlenderPony2").CreateInstance();

                _backgroundMusic.IsLooped = true;
                #endregion
            }
            _slender.Load(Content);
            _item.LoadItems(Content);
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

            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.P) && previousKeyboardState.IsKeyUp(Keys.P))
            {
                if (this.backgroundMusicPlaying) _backgroundMusic.Pause();
                else _backgroundMusic.Play();

                backgroundMusicPlaying = !backgroundMusicPlaying;
            }

            if (keyboardState.IsKeyDown(Keys.M) && previousKeyboardState.IsKeyUp(Keys.M))
            {

                isPaused = !isPaused;
            }

            if (!isPaused)
            {
                if (keyboardState.IsKeyDown(Keys.Escape))
                {
                    Initialize();
                    _isPlaying = false;
                    _ponyButtons.HasChosen = false;
                    _firstPlay = false;
                    _pony = null;
                    won = false;
                    lost = false;
                }

                if (!won && !lost)
                {
                    if (_item.CollectedAll())
                    {
                        won = true;
                    }


                    if (_isPlaying)
                    {
                        _pony.Update(gameTime);

                        if (_item.CountCollected() > 1)
                            _slender.Update(gameTime, _cameraPosition + new Vector2(380, 220));


                        Rectangle playerBounds = _pony.Bounds;
                        playerBounds.Location = new Point((int)(_cameraPosition.X + 379), (int)(_cameraPosition.Y + 210));
                        if (playerBounds.Intersects(_slender.Bounds))
                        {
                            lost = true;
                        }
                        _item.Update(_cameraPosition);
                        #region keyboard


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

                }
            }
            this.previousKeyboardState = Keyboard.GetState();
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
            if (_pony != null)
            {
                _pony.Draw(spriteBatch);
                _slender.Draw(spriteBatch, _cameraPosition);
                _item.DrawItems(spriteBatch, _cameraPosition);
            }
            spriteBatch.Draw(_blackHole, Vector2.Zero, Color.White);
            if (!_isPlaying)
            {
                _ponyButtons.Draw(spriteBatch);
                spriteBatch.DrawString(_font, "Choose a pony", new Vector2(100, 5), Color.Gray);
                spriteBatch.DrawString(_font, "START THE GAME BY CLICKING ON A PONY", new Vector2(250, 300), Color.White);
            }
            else
            {
                spriteBatch.DrawString(_font, "Press \"esc\" to restart the game ", new Vector2(20, 0), Color.Gray);
                spriteBatch.DrawString(_font, "Press \"p\" to play/pause the music. Press \"M\" for play/pause the game", new Vector2(20, 450), Color.Gray);
            }
            _item.DrawList(spriteBatch);
            if (won)
            {
                spriteBatch.DrawString(_font, "YOU ESCAPED THE SLENDERMAN", new Vector2(280, 200), Color.Purple);
            }
            if (lost)
            {
                spriteBatch.DrawString(_font, "YOU WAS DESTROYD BY THE SLENDERMAN", new Vector2(260, 200), Color.Purple);
            }
            if(isPaused)
                spriteBatch.DrawString(_font, "PAUSE", new Vector2(370, 250), Color.White);

            spriteBatch.Draw(_cursor, _cursorPosition - new Vector2(30, 30), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
