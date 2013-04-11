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

    class Items
    {
        private Texture2D smallPack;
        private Texture2D bigPack;

        private bool[] found; 

        private Vector2[] PackList;
        private Vector2[] itemPosition;

        private int _height;
        private int _width;

        public Items()
        {

            found = new bool[5];

            _height = 30;
            _width = 30;

            PackList = new Vector2[5]
             {
            new Vector2(650, 0),
            new Vector2(710, 0),
            new Vector2(760, 0),
            new Vector2(710, 0),
            new Vector2(760, 0)
            };

            itemPosition = new Vector2[5]
             {
            new Vector2(200, 180),
            new Vector2(220, 200),
            new Vector2(240, 220),
            new Vector2(260, 240),
            new Vector2(280, 250)
            };


        }

        public void Update()
        {
           MouseState ms = Mouse.GetState();

           for (int i = 0; i < itemPosition.Length; i++)
           {
               if (ms.LeftButton == ButtonState.Pressed &&
                   (new Rectangle((int)itemPosition[i].X, (int)itemPosition[i].Y, _width, _height))
                   .Contains(new Point(ms.X, ms.Y)))
               {
                   found[i] = true;
               }
           }       
        }

        public void LoadItems(ContentManager content) 
        {
            smallPack = content.Load<Texture2D>("smallpack");
            bigPack = content.Load<Texture2D>("bigpack");


        }
        public void DrawItems(SpriteBatch spritebatch)
        { 
            for(int i = 0; i < PackList.Length; i++)
            {
                spritebatch.Draw(smallPack, new Rectangle((int)itemPosition[i].X, (int)itemPosition[i].X, _width, _height), Color.White);

            }
        }
        public void DrawList(SpriteBatch spritebatch)
        {
            for (int i = 0; i < found.Length; i++)
            {
                spritebatch.Draw(bigPack, new Rectangle((int)PackList[i].X, (int)PackList[i].X, 65, 65), Color.White);

            }
        
        }
    }
}
