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
        
        public bool crashUp { get; protected set; }
        public bool crashLeft { get; protected set; }
        public bool crashRight { get; protected set; }
        public bool crashDown { get; protected set; }
        private Ponies pony;
        private Ponybuttons button;
        private Rectangle playerCenter;

        #region CollisionMap
        private Texture2D _collisionMap;

        private Rectangle _characterCollisionMap;
        private Rectangle _previousCollisionMap;
        private List<Rectangle> _collision = new List<Rectangle>();
        #endregion

        //This is the background collition rectangle
        public Background() 
        {
            //collitionRec.Add(new Rectangle(1, 1, 6, 6));
            button = new Ponybuttons();
            pony = new Ponies(button.Choice);
        }

        //This estimates the collision points in the game
        public void Update(GameTime gametime, Rectangle playerPosition)
        {
            playerPosition.Location = new Point((int)(playerPosition.X + 379), (int)(playerPosition.Y + 210));

            playerCenter.X = playerPosition.X / 2;
            playerCenter.Y = playerPosition.Y / 2;

            crashUp = false;
            crashLeft = false;
            crashRight = false;
            crashDown = false;

            foreach (Rectangle rec in collitionRec)
            {
                if (playerPosition.Intersects(rec))
                {
                    if (rec.X > playerCenter.X)
                    {
                        crashRight = true;
                    }
                    
                    if (rec.X < playerCenter.X)
                    {
                        crashLeft = true;
                    }
                    
                    if (rec.Y > playerCenter.Y)
                    {
                        crashDown = true;
                    }
                    
                    if (rec.Y < playerCenter.Y)
                    {
                        crashUp = true;
                    }
                }
            }
        }

        public void Initialize()
        {
            #region CollisionMap
            _collision.Add(new Rectangle(0, 0, 2043, 1066));
            #endregion
        }

        //This loads the map in the game with the texture
        public void LoadContent(ContentManager content)
        {
            _map = content.Load<Texture2D>("background - Finished Darker Smaller 3");
/*            #region CollisionMap
            _collisionMap = content.Load<Texture2D>("background - Collision");
            #endregion
*/        }
       
        //This draws the map in the game
        public void Draw(SpriteBatch spritBatch, Vector2 camera)
        {
           spritBatch.Draw(_map, -camera, Color.White);
           foreach (Rectangle rec in collitionRec)
           {
               spritBatch.Draw(_map, new Rectangle((int)(rec.X - camera.X), (int)(rec.Y - camera.Y), rec.Width, rec.Height), Color.White);
           }
           
        }
    }
}
