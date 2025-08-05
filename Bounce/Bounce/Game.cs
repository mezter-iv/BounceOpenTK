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
        Vector2 position = new Vector2(0.0f, 0.0f);
        Vector2 velocity = new Vector2(1f, 1.5f);

        // Define the number of vertices for the circle's perimeter
        int numVertices = 120;
        // Define the radius of the circle
        float radius = 0.1f;

        // Create a list to store the vertices
        List<float> circleVertices = new List<float>();

        float[] vertices;

        int SCREENWIDTH, SCREENHEIGHT, vao, shaderprogram, vbo;

        public Game(int width, int height) : base(GameWindowSettings.Default, NativeWindowSettings.Default)
        {
            SCREENHEIGHT = height;
            SCREENWIDTH = width;

            circleVertices.Add(0f);
            circleVertices.Add(0f);
            circleVertices.Add(0f);

            for (int i = 0; i <= numVertices; i++)
            {
                float angle = 2.0f * MathF.PI * i / numVertices;
                float x = radius * MathF.Cos(angle);
                float y = radius * MathF.Sin(angle);

                circleVertices.Add(x);
                circleVertices.Add(y);
                circleVertices.Add(0f);
            }

            vertices = circleVertices.ToArray();

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

            vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length*sizeof(float), vertices, BufferUsageHint.StaticDraw);

            // bind the vao

            GL.BindVertexArray(vao);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
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

            // Get the uniform location
            int positionUniformLocation = GL.GetUniformLocation(shaderprogram, "uPosition");
            // Pass the position to the shader
            GL.Uniform2(positionUniformLocation, position);

            // draw triangle
            GL.UseProgram(shaderprogram);
            GL.BindVertexArray(vao);
            GL.DrawArrays(PrimitiveType.TriangleFan, 0, numVertices + 2);



            Context.SwapBuffers();

            base.OnRenderFrame(args);
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            position += velocity * (float)args.Time;
            float halfSizeX = 0.5f;
            float halfSizeY = 0.5f;

            if (position.X + halfSizeX > 1.0f || position.X - halfSizeX < -1.0f)
            {
                velocity.X *= -1;
            }

            if (position.Y + halfSizeY > 1.0f || position.Y - halfSizeY < -1.0f)
            {
                velocity.Y *= -1;
            }
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
