using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tetris
{
    public class ScoreEntry
    {
        public string Name{get; set;}
        public int Score{get; set;}

        public ScoreEntry(string name, int score){
            Name = name;
            Score = score;
        }
    }
}
