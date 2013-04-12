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
    public class Slender
    {
        //Variables
        private Texture2D _slender;
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

        //This show the position of the animation in the spritebatch to slenderman
        public Slender(Vector2 position)
        {
            _relativeBounds = new Rectangle(0, 0, 20, 30);

            _position = position;
            _frames = new Rectangle[4, 4];
            _frames[0, 0] = new Rectangle(24, 0, 24, 43);
            _frames[0, 1] = new Rectangle(0, 0, 24, 43);
            _frames[0, 2] = new Rectangle(48, 0, 24, 43);
            _frames[0, 3] = new Rectangle(0, 0, 24, 43);
            _frames[1, 0] = new Rectangle(24, 43, 24, 43);
            _frames[1, 1] = new Rectangle(0, 43, 24, 43);
            _frames[1, 2] = new Rectangle(48, 43, 24, 43);
            _frames[1, 3] = new Rectangle(0, 43, 24, 43);
            _frames[2, 0] = new Rectangle(24, 86, 24, 43);
            _frames[2, 1] = new Rectangle(0, 86, 24, 43);
            _frames[2, 2] = new Rectangle(48, 86, 24, 43);
            _frames[2, 3] = new Rectangle(0, 86, 24, 43);
            _frames[3, 0] = new Rectangle(48, 86, 24, 43);
            _frames[3, 1] = new Rectangle(0, 86, 24, 43);
            _frames[3, 2] = new Rectangle(24, 86, 24, 43);
            _frames[3, 3] = new Rectangle(0, 86, 24, 43);
        }

        //This loads the texture of slenderman
        public void Load(ContentManager content)
        {
            _slender = content.Load<Texture2D>("enderman");
        }

        //This updates his movement in the game
        public void Update(GameTime gameTime, Vector2 ponyPos)
        {
            bool moving = false;
            Vector2 movement = Vector2.Normalize(ponyPos - _position) * 1f;

            #region MovementOfSlender
            if (movement.Y < 0 && Math.Abs(movement.Y) >= Math.Abs(movement.X))
            {
                _wayToGo = 1;
                moving = true;
            }
        
            if (movement.X > 0 && Math.Abs(movement.X) >= Math.Abs(movement.Y))
            {
                _wayToGo = 2;
                moving = true;
            }
            
            if (movement.Y > 0 && Math.Abs(movement.Y) >= Math.Abs(movement.X))
            {
                _wayToGo = 0;
                moving = true;
            }
            
            if (movement.X < 0 && Math.Abs(movement.X) >= Math.Abs(movement.Y))
            {
                _wayToGo = 3;
                moving = true;
            }
            
            _position += movement;
            #endregion

            #region TheSpeedOfSlendermansMovement
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

        //Draws slendermans movement
        public void Draw(SpriteBatch spriteBatch, Vector2 cameraPos)
        {
            Rectangle target = _frames[_wayToGo, _frame];
            spriteBatch.Draw(_slender, new Rectangle((int)(_position.X - cameraPos.X), (int)(_position.Y - cameraPos.Y), 32, 64),
                target, Color.White);
        }
    }
}
