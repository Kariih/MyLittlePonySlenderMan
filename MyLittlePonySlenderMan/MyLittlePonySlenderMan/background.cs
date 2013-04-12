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
    public class Background
    {
        //Variables
        private Texture2D _map;

        public Rectangle Bounds { get { return _map.Bounds; } }
        public List<Rectangle> collitionRec = new List<Rectangle>();
        bool _crash;

        //This is the background collition rectangle
        public Background() 
        {
            collitionRec.Add(new Rectangle(1, 1, 6, 6));
        }

        //This estimates the collision points in the game
        public void Update(GameTime gametime, Rectangle playerPosition)
        {
            playerPosition.Location = new Point((int)(playerPosition.X + 379), (int)(playerPosition.Y + 210));

            foreach (Rectangle rec in collitionRec)
            {
                if (playerPosition.Intersects(rec))
                {
                    
                }
            }
        }

        //This loads the map in the game with the texture
        public void LoadContent(ContentManager content)
        {
            _map = content.Load<Texture2D>("background - Finished Darker Smaller 3");
        }
       
        //This draws the map in the game
        public void Draw(SpriteBatch spritBatch, Vector2 camera)
        {
           spritBatch.Draw(_map, -camera, Color.White);
           
        }
    }
}
