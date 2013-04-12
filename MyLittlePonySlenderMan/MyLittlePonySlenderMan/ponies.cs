using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace MyLittlePonySlenderMan
{
    public class Ponies
    {
        //All the variable
        private Texture2D _ponies;
        private float _animTimer;
        private int _frame;
        private Rectangle[,] _frames;
        private Vector2 _position;
        private int _wayToGo;

        protected Rectangle _relativeBounds;
        
        public Rectangle Bounds
        {
            get
            {
                return new Rectangle((int)(_relativeBounds.X + _position.X), (int)(_relativeBounds.Y + _position.Y), _relativeBounds.Width, _relativeBounds.Height);
            }
        }

        private Point _ponyPoint;
        
        //Each of the point represents a pony on the spritebatch
        private Point[] PonyList = { new Point(0,0),
                             new Point(192, 0),
                             new Point(384, 0),
                             new Point(576, 0),
                             new Point(0,256),
                             new Point(192, 256),
                             new Point(384, 256),
                             new Point(576, 256),
                           };

        //This determines the position of where the pony is place in the beginning of the game
        public Ponies(int pony)
        {
            _relativeBounds = new Rectangle(0, 0, 20, 30);
            _ponyPoint = PonyList[pony];
            _position = new Vector2(365, 220);
        }

        //This loads the animation according to Point above
        public void LoadPonies(ContentManager content)
        {
            _ponies = content.Load<Texture2D>("myLittlePony");
            _frames = new Rectangle[4,4];
            _frames[0, 0] = new Rectangle(0, 0, 64, 64);
            _frames[0, 1] = new Rectangle(64, 0, 64, 64);
            _frames[0, 2] = new Rectangle(128, 0, 64, 64);
            _frames[0, 3] = new Rectangle(64, 0, 64, 64);
            _frames[1, 0] = new Rectangle(0, 128, 64, 64);
            _frames[1, 1] = new Rectangle(64, 128, 64, 64);
            _frames[1, 2] = new Rectangle(128, 128, 64, 64);
            _frames[1, 3] = new Rectangle(64, 128, 64, 64);
            _frames[2, 0] = new Rectangle(0, 192, 64, 64);
            _frames[2, 1] = new Rectangle(64, 192, 64, 64);
            _frames[2, 2] = new Rectangle(128, 192, 64, 64);
            _frames[2, 3] = new Rectangle(64, 192, 64, 64);
            _frames[3, 0] = new Rectangle(0, 64, 64, 64);
            _frames[3, 1] = new Rectangle(64, 64, 64, 64);
            _frames[3, 2] = new Rectangle(128, 64, 64, 64);
            _frames[3, 3] = new Rectangle(64, 64, 64, 64);
        }

        //This updates the movement of the pony
        public void Update(GameTime gameTime)
        {
            bool moving = false;
            KeyboardState keyboardState = Keyboard.GetState();
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
            Vector2 movement = gamePadState.ThumbSticks.Left;

            #region KeyboardMovementKeys
            if (keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S))
            {
                movement.Y = 1;
            }
            
            if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D))
            {
                movement.X = 1;
            }

            if (keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W))
            {
                movement.Y = -1;
            }

            if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A))
            {
                movement.X = -1;
            }
            movement.Normalize();
            #endregion

            #region MovementOfTheCharacter
            if (movement.Y > 0 && Math.Abs(movement.Y) >= Math.Abs(movement.X))
            {
                _wayToGo = 0;
                moving = true;
            }

            if (movement.X > 0 && Math.Abs(movement.X) >= Math.Abs(movement.Y))
            {
                _wayToGo = 1;
                moving = true;
            }

            if (movement.Y < 0 && Math.Abs(movement.Y) >= Math.Abs(movement.X))
            {
                _wayToGo = 2;
                moving = true;
            }

            if (movement.X < 0 && Math.Abs(movement.X) >= Math.Abs(movement.Y))
            {
                _wayToGo = 3;
                moving = true;
            }
            #endregion

            #region TheSpeedOfTheCharactersMovement
            if (moving)
            {
                _animTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (_animTimer > 0.25f)
                {
                    _frame++;
                    _frame %= _frames.GetLength(1);
                    _animTimer -= 0.25f;
                }
            }

            else
            {
                _frame = 1;
                _animTimer = 0;
            }
            #endregion
        }

        //Draws the pony's movement
        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle target = _frames[_wayToGo , _frame];
            target.X += _ponyPoint.X;
            target.Y += _ponyPoint.Y;
            spriteBatch.Draw(_ponies, new Rectangle((int)_position.X, (int)_position.Y, 64, 64),
                target, Color.White);
        }
    }
}
