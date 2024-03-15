using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace Hola_Triangulo_OpenTK
{
    public class Game : GameWindow
    {
        private int vertexbufferObject;
        private int shaderProgramObject;
        private int vertexArrayObject;


        public Game()
            : base(GameWindowSettings.Default, NativeWindowSettings.Default)
        {
            this.CenterWindow(new Vector2i(400, 400));

        }
        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
        }
        protected override void OnLoad()
        {
            GL.ClearColor(new Color4(0.5f, 0.4f, 0.8f, 1f));

            float[] vertices = new float[]
            {
                0.0f,0.5f,0f,
                0.5f,-0.5f,0f,
                -0.5f,-0.5f,0f
            };

            this.vertexbufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, this.vertexbufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            this.vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(this.vertexArrayObject);

            GL.BindBuffer(BufferTarget.ArrayBuffer, this.vertexbufferObject);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            GL.BindVertexArray(0);

            string vertexShaderCode =
                @"
                 #version 330 core

                 layout (location=0) in vec3 aPosition;
                 
                 void main(){

                 gl_Position=vec(aPosition,1f);

                 }";

            string pixelShaderCode =
                @"
                 #version 330 core

                 out vec4 pixelColor;

                 void main(){

                 pixelColor=vec4(0.8f,0.8f,0.1f,1f);
                 }
                 ";



            int vertexShaderObject = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexbufferObject, vertexShaderCode);
            GL.CompileShader(vertexShaderObject);


            int pixelShaderObject = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(pixelShaderObject, pixelShaderCode);
            GL.CompileShader(pixelShaderObject);

            this.shaderProgramObject = GL.CreateProgram();

            GL.AttachShader(this.shaderProgramObject, vertexShaderObject);
            GL.AttachShader(this.shaderProgramObject, pixelShaderObject);

            GL.LinkProgram(this.shaderProgramObject);

            GL.DetachShader(this.shaderProgramObject, vertexShaderObject);
            GL.DetachShader(this.shaderProgramObject, pixelShaderObject);

            GL.DeleteShader(vertexShaderObject);
            GL.DeleteShader(pixelShaderObject);

            base.OnLoad();
        }

        protected override void OnUnload()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DeleteBuffer(this.vertexbufferObject);

            GL.UseProgram(0);
            GL.DeleteProgram(this.shaderProgramObject);


            base.OnUnload();
        }
        protected override void OnResize(ResizeEventArgs e)
        {
            GL.Viewport(0, 0, e.Width, e.Height);
            base.OnResize(e);
        }


        protected override void OnRenderFrame(FrameEventArgs args)
        {

            GL.Clear(ClearBufferMask.ColorBufferBit);

            GL.UseProgram(this.shaderProgramObject);

            GL.BindVertexArray(this.vertexArrayObject);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

            this.Context.SwapBuffers();
            base.OnRenderFrame(args);
        }

    }
}
