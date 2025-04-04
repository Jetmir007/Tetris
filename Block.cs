using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Tetris
{
    public enum BlockType{O, I, S, Z, L, J, T}
    public class Block
    {
        private Texture2D texture;
        public bool[,] tiles;
        public int X{get; set;} = 3;
        public int Y{get; set;} = 0;
        public BlockType Type { get; set; }

        private bool[,] BlockShape(BlockType blockType)
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
                },
                _ => throw new NotImplementedException()
            };
        }

        public Block(Texture2D texture, BlockType blockType){
            this.texture = texture;
            Type = blockType;
            tiles = BlockShape(blockType);
        }

        public void Rotate(){
            int rows = tiles.GetLength(0);
            int cols = tiles.GetLength(1);

            bool[,] rotatedTiles = new bool[cols, rows];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    rotatedTiles[j, rows - 1 - i] = tiles[i, j];
                }
            }
            tiles = rotatedTiles;
        }


        public void Draw(SpriteBatch spriteBatch){
            for (int i = 0; i < tiles.GetLength(0); i++)
            {
                for (int j = 0; j < tiles.GetLength(1); j++)
                {
                    if(tiles[i, j] == true){
                        spriteBatch.Draw(texture, new Rectangle(X*20 + 20*j, Y*20+ 20*i, 20, 20), Color.Black);
                    }
                }
            }
        }
    }
}