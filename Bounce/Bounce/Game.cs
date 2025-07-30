using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Bounce
{
    internal class Game : GameWindow
    {
        static int SCREENWIDTH;
        static int SCREENHEIGHT;  
        public Game(int width, int height) : base(GameWindowSettings.Default, NativeWindowSettings.Default) {
            this.CenterWindow(new Vector2i(width, height));
            SCREENHEIGHT = height;
            SCREENWIDTH = width;
        }
    }
}
