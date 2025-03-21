using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tetris
{
    public class Block
    {
        private Texture2D texture;
        private Rectangle hitbox;
        bool[,] tiles = new bool[4, 4];

        public void True(){
            tiles[0, 0] = true;
            tiles[2,3] = true;
        }

        public Block(Texture2D texture){
            this.texture = texture;
            hitbox = new Rectangle(50, 50, 20, 20);
        }

        public void Draw(SpriteBatch spriteBatch){
            True();
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if(tiles[i, j] == true){
                        spriteBatch.Draw(texture, new Rectangle(340 + 20*i,0 + 20*j,20,20), Color.Black);
                    }
                }
            }
        }
    }
}