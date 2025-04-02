using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tetris
{
    public class GameField
    {
        private int rows = 24;
        private int cols = 10;
        public bool[,] field;
        public GameField(){
            field = new bool[rows, cols];
        }

        public bool CheckCollision(Block block, int X, int Y){
            for (int i = 0; i < block.tiles.GetLength(0); i++)
            {
                for (int j = 0; j < block.tiles.GetLength(1); j++)
                {
                    if(block.tiles[i, j]){
                        int nyX = X +i;
                        int nyY = Y + j;

                        if(nyX < 0 || nyX >= cols || nyY >= rows || nyY < 0 || field[nyY, nyX]){
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public void Place(Block block){
            for (int i = 0; i < block.tiles.GetLength(0); i++)
            {
                for (int j = 0; j < block.tiles.GetLength(1); j++)
                {
                    if(block.tiles[i, j]){
                        field[block.Y + i, block.X + j] = true;
                    }
                }
            }
        }
    }
}