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
    public class Ponybuttons
    {
        //Variables
        #region ponies
        private Texture2D _appleJack;
        private Texture2D _derpyHooves;
        private Texture2D _fluttershy;
        private Texture2D _pinkiePie;
        private Texture2D _rainbowDash;
        private Texture2D _rarity;
        private Texture2D _trixie;
        private Texture2D _twilightSparkle;

        private Texture2D[] _buttons = new Texture2D[8];
        #endregion

        // private int _choice;
        private Vector2 _buttonPosition;
        private int _height;
        private int _width;
        private static MouseState mouseState = Mouse.GetState();
        private Point mousePosition = new Point(mouseState.X, mouseState.Y);

        public int Choice { get; protected set; }

        public bool HasChosen { get; set; }

        //The positions of the button
        public Ponybuttons()
        {
            _buttonPosition = new Vector2(20, 30);
            _width = 50;
            _height = 60;
        }

        //Loads the texture, and the array with the ponies
        public void loadButtons(ContentManager content)
        {
           #region PonyPictures
           _appleJack = content.Load<Texture2D>("appleJack");
           _derpyHooves = content.Load<Texture2D>("derpyHooves");
           _fluttershy = content.Load<Texture2D>("fluttershy");
           _pinkiePie = content.Load<Texture2D>("pinkiePie");
           _rainbowDash = content.Load<Texture2D>("rainbowDash");
           _rarity = content.Load<Texture2D>("Rarity");
           _trixie = content.Load<Texture2D>("Trixie");
           _twilightSparkle = content.Load<Texture2D>("twilightSparkle");
           #endregion

           _buttons[0] = _twilightSparkle;  
           _buttons[1] = _appleJack;
           _buttons[2] = _fluttershy;
           _buttons[3] = _pinkiePie;
           _buttons[4] = _rarity;
           _buttons[5] = _rainbowDash; 
           _buttons[6] = _trixie;
           _buttons[7] = _derpyHooves;
        }

        //This updates the position of the picture of the ponies of choice
        public void Update() 
        {
            MouseState ms = Mouse.GetState();
            if (ms.LeftButton == ButtonState.Pressed && 
               (new Rectangle((int)_buttonPosition.X, (int)_buttonPosition.Y, _width * _buttons.Length, _height))
               .Contains(new Point(ms.X, ms.Y)))
            {
               Choice = (int)((ms.X - _buttonPosition.X) / _width);
               HasChosen = true;
            }
        }
        
        //This draws the position of the images
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < _buttons.Length; i++)
            {
                   spriteBatch.Draw(_buttons[i],
                    new Rectangle((int)_buttonPosition.X + (_width * i), (int)_buttonPosition.Y, _width, _height),
                       Color.White);
            }
        }
    }
}
