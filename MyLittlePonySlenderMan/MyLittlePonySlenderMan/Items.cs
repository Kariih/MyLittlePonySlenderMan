﻿using System;
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
    //This class is for the items that you need to collect in the game
    public class Items
    {
        //All the variables
        private Texture2D smallPack;
        private Texture2D bigPack;

        private bool[] found; 

        private Vector2[] PackList;
        private Vector2[] itemPosition;

        private int _height;
        private int _width;

        //This is used to place all the boxes in the game
        public Items()
        {
            found = new bool[5];

            _height = 30;
            _width = 30;

            PackList = new Vector2[5]
            {
                new Vector2(530, 0),
                new Vector2(580, 0),
                new Vector2(630, 0),
                new Vector2(680, 0),
                new Vector2(730, 0)
            };

            itemPosition = new Vector2[5]
            {
                new Vector2(250, 690),
                new Vector2(220, 370),
                new Vector2(950, 830),
                new Vector2(1700, 670),
                new Vector2(650, 670)
            };
        }

        //This is used to update the position of the mouse in the game space
        public void Update(Vector2 cameraPos)
        {
            //This is a mouse variable that in this contexted is used to check where on the map it is
            MouseState ms = Mouse.GetState();

            //This is a check to see if your mouse has clicked on the box, if so, it'll return a true statement
            for (int i = 0; i < itemPosition.Length; i++)
            {
               if (ms.LeftButton == ButtonState.Pressed &&
                   (new Rectangle((int)(itemPosition[i].X - cameraPos.X), (int)(itemPosition[i].Y - cameraPos.Y), _width, _height))
                   .Contains(new Point(ms.X, ms.Y)))
               {
                   found[i] = true;
               }
            }       
        }

        //This will load the boxes into the game
        public void LoadItems(ContentManager content) 
        {
            smallPack = content.Load<Texture2D>("smallpack");
            bigPack = content.Load<Texture2D>("bigpack");
        }

        //This will draw the boxes in the game 
        public void DrawItems(SpriteBatch spritebatch, Vector2 cameraPos)
        { 
            for(int i = 0; i < PackList.Length; i++)
            {
                if(!found[i])
                spritebatch.Draw(smallPack, new Rectangle((int)(itemPosition[i].X - cameraPos.X), (int)(itemPosition[i].Y - cameraPos.Y), _width, _height), Color.White);
            }
        }

        //This draws up the boxes found in the right corner
        public void DrawList(SpriteBatch spritebatch)
        {
            for (int i = 0; i < found.Length; i++)
            {
                if(found[i])
                spritebatch.Draw(bigPack, new Rectangle((int)PackList[i].X, (int)PackList[i].Y, 65, 65), Color.White);
            }
        }

        //When all boxes are collected, this will check if it's correct 
        public bool CollectedAll()
        {
            foreach (bool b in found)
            {
                if (!b)
                {
                    return false;
                }
            }
            return true;
        }

        //This is a counter for how many boxes that has been found
        public int CountCollected()
        { 
            int count = 0;
            foreach (bool b in found)
            {
                if (b)
                {
                    count++;
                }

            }
            return count;
        }
    }
}
