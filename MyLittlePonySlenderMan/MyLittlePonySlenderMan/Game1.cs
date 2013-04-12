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
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //All the variables
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
        private Rectangle _crashRec;

        private KeyboardState previousKeyboardState;

        private bool _firstPlay;
        
        private bool _isPlaying;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            _firstPlay = true;
            _crashRec = new Rectangle((int)_cameraPosition.X, (int)_cameraPosition.Y, 20, 20);
        }

        protected override void Initialize()
        {
            //If _firstPlay is true, it'll run the game
            if (_firstPlay)
            {
                _background = new Background();
                _ponyButtons = new Ponybuttons();
            }
            _cameraPosition = new Vector2(360, 250);
            _slender = new Slender(new Vector2(700, 700));
            _item = new Items();
            this.IsMouseVisible = false;

            base.Initialize();

            //This will make the background music play
            _backgroundMusic.Play();
        }

        protected override void LoadContent()
        {
            if(_firstPlay){
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

        //Nonfunctional in this game
        protected override void UnloadContent()
        {
           
        }

        protected override void Update(GameTime gameTime)
        {
            //Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            MouseState mouseState = Mouse.GetState();
            _cursorPosition = new Vector2(mouseState.X, mouseState.Y);

            KeyboardState keyboardState = Keyboard.GetState();

            _background.Update(gameTime, _crashRec);

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
            
            //When the game isn't paused, you will get these different options
            if (!isPaused)
            {
                //If you press the Esc bottum, then you'll end the current game, and also be able to restart
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

                //If you haven't lost and haven't won, these things will occur...
                if (!won && !lost)
                {
                    //...When all items are collected, you win.
                    if (_item.CollectedAll())
                    {
                        won = true;
                    }

                    //...If you're still playing, the ponies position will be updated... 
                    if (_isPlaying)
                    {
                        _pony.Update(gameTime);

                        //...If you've found more than 1 item, slenderman will turn up, and follow you
                        if (_item.CountCollected() > 1)
                            _slender.Update(gameTime, _cameraPosition + new Vector2(380, 220));

                        //...This is the border of the whole game
                        Rectangle playerBounds = _pony.Bounds;
                        playerBounds.Location = new Point((int)(_cameraPosition.X + 379), (int)(_cameraPosition.Y + 210));
                        
                        //...If slenderman gets to you, you've lost the game.
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

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin();
            _background.Draw(spriteBatch, _cameraPosition);
            if (_pony != null)
            {
                _item.DrawItems(spriteBatch, _cameraPosition);
                _pony.Draw(spriteBatch);
                _slender.Draw(spriteBatch, _cameraPosition);
            }
            spriteBatch.Draw(_blackHole, Vector2.Zero, Color.White);
            
            //When you start the game...
            if (!_isPlaying)
            {
                //...this will be the first thing you'll see on the screen
                _ponyButtons.Draw(spriteBatch);
                spriteBatch.DrawString(_font, "Choose a pony", new Vector2(100, 5), Color.Gray);
                spriteBatch.DrawString(_font, "START THE GAME BY CLICKING ON A PONY", new Vector2(250, 300), Color.White);
            }
            
            else
            {
                //...this be visible when you've chosen a pony and have started the game
                spriteBatch.DrawString(_font, "Press \"esc\" to restart the game ", new Vector2(20, 0), Color.Gray);
                spriteBatch.DrawString(_font, "Press \"p\" to play/pause the music. Press \"M\" for play/pause the game", new Vector2(20, 450), Color.Gray);
            }

            _item.DrawList(spriteBatch);
            
            //If you win, you receive this message
            if (won)
            {
                spriteBatch.DrawString(_font, "YOU ESCAPED THE SLENDERMAN", new Vector2(280, 120), Color.Purple);
            }

            //If you lose, you receive this message
            if (lost)
            {
                spriteBatch.DrawString(_font, "YOU WAS DESTROYED BY THE SLENDERMAN", new Vector2(260, 120), Color.Purple);
            }

            //If you want to pause, this will pause your position in the game
            if(isPaused)
                spriteBatch.DrawString(_font, "PAUSE", new Vector2(370, 250), Color.White);

            spriteBatch.Draw(_cursor, _cursorPosition - new Vector2(30, 30), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
