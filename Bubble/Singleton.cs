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
        public MouseState _previousMouse, _currentMouse;

        public Random rnd = new Random();

        public Color[] allColor = {Color.Red, Color.Green, Color.Blue, Color.Yellow};
        public int[,] _bbColor = { { 0, -4, 1, -4, 2, -4, 3, -4, 1, -4, 2, -4, 3, -4, 2, -4, 3, -4, 3, -4 }
                                , { -4, 0, -4, 0, -4, 3, -4, 0, -4, 3, -4, 2, -4, 0, -4, 3, -4, 2, -4, 1 }
                                , { 2, -4, 3, -4, 0, -4, 1, -4, 3, -4, 0, -4, 1, -4, 0, -4, 1, -4, 1, -4 }
                                , { -4, 2, -4, 2, -4, 1, -4, 2, -4, 1, -4, 0, -4, 2, -4, 1, -4, 0, -4, 3}
                                , { 0, -4, 3, -4, 1, -4, 2, -4, 0, -4, 3, -4, 1, -4, 0, -4, 3, -4, 0, -4}
                                , { -4, 1, -4, 2, -4, 0, -4, 3, -4, 1, -4, 2, -4, 0, -4, 1, -4, 2, -4, 1} 
                                , { -1, -4, -1, -4, -1, -4, -1, -4, -1, -4, -1, -4, -1, -4, -1, -4, -1, -4, -1, -4}
                                , { -4, -1, -4, -1, -4, -1, -4, -1, -4, -1, -4, -1, -4, -1, -4, -1, -4, -1, -4, -1}
                                , { -1, -4, -1, -4, -1, -4, -1, -4, -1, -4, -1, -4, -1, -4, -1, -4, -1, -4, -1, -4}
                                , { -4, -1, -4, -1, -4, -1, -4, -1, -4, -1, -4, -1, -4, -1, -4, -1, -4, -1, -4, -1}
                                , { -1, -4, -1, -4, -1, -4, -1, -4, -1, -4, -1, -4, -1, -4, -1, -4, -1, -4, -1, -4} };

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
