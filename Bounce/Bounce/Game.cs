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
        float[] vertices = { 
            0f, 0.5f, 0f, // top vertex
            -0.5f, -0.5f, 0f, // bottom left
            0.5f, -0.5f, 0f // bottom right
        };

        int SCREENWIDTH, SCREENHEIGHT, vao, shaderprogram;

        public Game(int width, int height) : base(GameWindowSettings.Default, NativeWindowSettings.Default)
        {
            SCREENHEIGHT = height;
            SCREENWIDTH = width;

            // center screen
            this.CenterWindow(new Vector2i(SCREENWIDTH, SCREENHEIGHT));
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);
            SCREENWIDTH = e.Width;
            SCREENHEIGHT = e.Height;
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            vao = GL.GenVertexArray();

            int vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length*sizeof(float), vertices, BufferUsageHint.StaticDraw);

            // bind the vao

            GL.BindVertexArray(vao);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexArrayAttrib(vao, 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0); // unbinding the vbo
            GL.BindVertexArray(0);

            // create the shader program
            shaderprogram = GL.CreateProgram();

            int vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, LoadShaderSourse("Default.vert"));
            GL.CompileShader(vertexShader);

            int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, LoadShaderSourse("Default.frag"));
            GL.CompileShader(fragmentShader);

            GL.AttachShader(shaderprogram, vertexShader);
            GL.AttachShader(shaderprogram, fragmentShader);

            GL.LinkProgram(shaderprogram);

            // delete the shaders
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);
        }

        protected override void OnUnload()
        {
            base.OnUnload();

            GL.DeleteVertexArray(vao);
            GL.DeleteProgram(shaderprogram);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.ClearColor(0.6f, 0.3f, 1f, 1f);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            // draw triangle
            GL.UseProgram(shaderprogram);
            GL.BindVertexArray(vao);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);



            Context.SwapBuffers();

            base.OnRenderFrame(args);
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
        }

        public static string LoadShaderSourse(string filePath) {
            string shaderSourse = "";
            try
            {
                using (var r = new StreamReader("../../../Shaders/" + filePath))
                {
                    shaderSourse = r.ReadToEnd();
                }
            }
            catch (Exception e) {
                Console.WriteLine("Failed to load shader sourse file" + e.Message);
            }
            return shaderSourse;
        }
    }
}
