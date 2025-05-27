using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Tetris
{
    public enum BlockType{O, I, S, Z, L, J, T, Q}
    public class Block
    {
        private Texture2D texture;
        public bool[,] tiles;
        public int X{get; set;} = 4;
        public int Y{get; set;} = 0;
        public BlockType Type { get; set; }

        public bool[,] BlockShape(BlockType blockType)
        {
            return blockType switch
            {
                BlockType.O => new bool[,]
                {
                    {true, true},
                    {true, true}
                },
                BlockType.I => new bool[,]
                {
                    {true},
                    {true},
                    {true},
                    {true}
                },
                BlockType.S => new bool[,]
                {
                    {false, true, true},
                    {true, true, false}
                },
                BlockType.Z => new bool[,]
                {
                    {true, true, false},
                    {false, true, true}
                },
                BlockType.L => new bool[,]
                {
                    {true, false},
                    {true, false},
                    {true, true}
                },
                BlockType.J => new bool[,]
                {
                    {false, true},
                    {false, true},
                    {true, true}
                },
                BlockType.T => new bool[,]
                {
                    {true, true, true},
                    {false, true, false}
                },
                BlockType.Q => new bool[,]
                {
                    {true, false, true},
                    {true, true, true}
                },
                _ => throw new NotImplementedException()
            };
        }

        public Block(Texture2D texture, BlockType blockType){
            this.texture = texture;
            Type = blockType;
            tiles = BlockShape(blockType);
        }

        static public BlockType RandomType(){
        Random rng = new Random();
        return (BlockType)rng.Next(1, 1);
        }

        public bool[,] Rotate(){
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
            return rotatedTiles;
        }

        public Block Clone(Texture2D texture2D){
            return new Block(texture2D, this.Type);
        }

        public void Draw(SpriteBatch spriteBatch){
            for (int i = 0; i < tiles.GetLength(0); i++)
            {
                for (int j = 0; j < tiles.GetLength(1); j++)
                {
                    if(tiles[i, j] == true){
                        spriteBatch.Draw(texture, new Rectangle(X*20 + 200 + 20*j, Y*20+ 20*i, 20, 20), Color.Aquamarine);
                    }
                }
            }
        }
    }
}