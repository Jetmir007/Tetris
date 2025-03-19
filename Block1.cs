using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tetris
{
    public class Block1
    {
        private Texture2D texture;
        private Rectangle hitbox;
        private Vector2 position;
        private float rotation;
        private Rectangle[,] tiles = new Rectangle[4, 4];

        public Rectangle Hitbox{
            get{return hitbox;}
        }

        public Block1(Texture2D texture){
            
        }


        public void Draw(SpriteBatch spriteBatch){
            spriteBatch.Draw(texture, hitbox, Microsoft.Xna.Framework.Color.Yellow);
        }
        
    }
}