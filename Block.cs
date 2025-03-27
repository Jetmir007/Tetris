using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tetris
{
    public class Block
    {
        private Texture2D texture;
        private Rectangle hitbox;
        private bool[,] tiles;

        public enum BlockType{O, I, S, Z, L, J, T}

        public bool[,] BlockShape(BlockType blockType)
        {
            return blockType switch
            {
                BlockType.O => new bool[,]
                {
                    {true, true, false, false},
                    {true, true, false, false},
                    {false, false, false, false},
                    {false, false, false, false}
                },
                BlockType.I => new bool[,]
                {
                    {true, false, false, false},
                    {true, false, false, false},
                    {true, false, false, false},
                    {true, false, false, false}
                },
                BlockType.S => new bool[,]
                {
                    {false, true, true, false},
                    {true, true, false, false},
                    {false, false, false, false},
                    {false, false, false, false}
                },
                BlockType.Z => new bool[,]
                {
                    {true, true, false, false},
                    {false, true, true, false},
                    {false, false, false, false},
                    {false, false, false, false}
                },
                BlockType.L => new bool[,]
                {
                    {true, false, false, false},
                    {true, false, false, false},
                    {true, true, false, false},
                    {false, false, false, false}
                },
                BlockType.J => new bool[,]
                {
                    {false, true, false, false},
                    {false, true, false, false},
                    {true, true, false, false},
                    {false, false, false, false}
                },
                BlockType.T => new bool[,]
                {
                    {true, true, true, false},
                    {false, true, false, false},
                    {false, false, false, false},
                    {false, false, false, false}
                }
            };
        }

        public Block(Texture2D texture, BlockType blockType){
            this.texture = texture;
            tiles = BlockShape(blockType);
        }

        public void Draw(SpriteBatch spriteBatch){
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