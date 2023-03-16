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
