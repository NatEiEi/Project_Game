using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bubble
{
    class Singleton
    {
        private static Singleton instance;

        public const int TileSize = 60;
        public const int ScreenWidth = 1200;
        public const int ScreenHeight = 900;

        public const int LeftMargin = 285;
        public const int RightMargin = LeftMargin + 630;
        public const int RedLine = (TileSize * 10) + 5;

        public KeyboardState _previousKey, _currentKey;

        public Random rnd = new Random();

        public Color[] allColor = {Color.Red, Color.Green, Color.Blue, Color.Yellow};
        public int[,] _bbColor = { { 0, 1, 2, 3, 1, 2, 3, 2, 3, 3 }
                                , { 0, 0, 3, 0, 3, 2, 0 ,3, 2, 1}
                                , { 2, 3, 0, 1, 3, 0, 1, 0, 1, 1}
                                , { 2, 2, 1, 2, 1, 0, 2, 1, 0, 3}
                                , { 0, 3, 1, 2, 0, 3, 1, 0, 3, 0}
                                , { 1, 2, 0, 3, 1, 2, 0, 1, 2, 1} 
                                , { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1}
                                , { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1}
                                , { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1}
                                , { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1}
                                , { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1} };

        private Singleton() { }

        public static Singleton Instance
        {
            get 
            { 
                if(instance == null)
                {
                    instance = new Singleton();
                }
                return instance;
            }
        }
    }
}
