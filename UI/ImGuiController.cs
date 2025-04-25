using ImGuiNET;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Vector2Num = System.Numerics.Vector2;

namespace OpenTKCubo3D.UI
{
    public class ImGuiController
    {
        private GameWindow _window;
        private Vector2 _scaleFactor = Vector2.One;
  

        private int _vertexArray;
        private int _vertexBuffer;
        private int _indexBuffer;

        private int _shader;
        private int _attribLocationTex;
        private int _attribLocationProjMtx;
        private int _attribLocationPosition;
        private int _attribLocationUV;
        private int _attribLocationColor;

        private int _fontTexture;

        public ImGuiController(GameWindow window)
        {
            _window = window;

            ImGui.CreateContext();
            ImGui.StyleColorsDark();

            var io = ImGui.GetIO();
            io.ConfigFlags |= ImGuiConfigFlags.NavEnableKeyboard;
            io.BackendFlags |= ImGuiBackendFlags.HasMouseCursors;
            io.BackendFlags |= ImGuiBackendFlags.HasSetMousePos;

            _scaleFactor = Vector2.One;

            CreateDeviceResources();
            SetFontTexture();
        }

        public void Update(float deltaTime)
        {
            ImGuiIOPtr io = ImGui.GetIO();

            io.DisplaySize = new Vector2Num(_window.Size.X, _window.Size.Y);
            io.DisplayFramebufferScale = Vector2Num.One;

            io.DeltaTime = deltaTime > 0.0f ? deltaTime : 1.0f / 60.0f;

            UpdateInput(io);
            ImGui.NewFrame();
        }

        private void UpdateInput(ImGuiIOPtr io)
        {
            var mouse = _window.MouseState;
            var keyboard = _window.KeyboardState;

            io.MouseDown[0] = mouse.IsButtonDown(MouseButton.Left);
            io.MouseDown[1] = mouse.IsButtonDown(MouseButton.Right);
            io.MouseDown[2] = mouse.IsButtonDown(MouseButton.Middle);

            io.MousePos = new Vector2Num(mouse.X, mouse.Y);

            io.MouseWheel = mouse.ScrollDelta.Y;
            io.MouseWheelH = mouse.ScrollDelta.X;

            io.KeyCtrl = keyboard.IsKeyDown(Keys.LeftControl) || keyboard.IsKeyDown(Keys.RightControl);
            io.KeyAlt = keyboard.IsKeyDown(Keys.LeftAlt) || keyboard.IsKeyDown(Keys.RightAlt);
            io.KeyShift = keyboard.IsKeyDown(Keys.LeftShift) || keyboard.IsKeyDown(Keys.RightShift);
            io.KeySuper = keyboard.IsKeyDown(Keys.LeftSuper) || keyboard.IsKeyDown(Keys.RightSuper);
        }

        public void Render()
        {
            ImGui.Render();
            RenderDrawData(ImGui.GetDrawData());
        }

        private void RenderDrawData(ImDrawDataPtr drawData)
        {
            GL.Enable(EnableCap.Blend);
            GL.BlendEquation(BlendEquationMode.FuncAdd);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.Disable(EnableCap.CullFace);
            GL.Disable(EnableCap.DepthTest);
            GL.Enable(EnableCap.ScissorTest);

            drawData.ScaleClipRects(ImGui.GetIO().DisplayFramebufferScale);

            GL.Viewport(0, 0, _window.Size.X, _window.Size.Y);

            Matrix4 proj = Matrix4.CreateOrthographicOffCenter(0.0f, _window.Size.X, _window.Size.Y, 0.0f, -1.0f, 1.0f);
            GL.UseProgram(_shader);
            GL.Uniform1(_attribLocationTex, 0);
            GL.UniformMatrix4(_attribLocationProjMtx, false, ref proj);

            GL.BindVertexArray(_vertexArray);

            for (int n = 0; n < drawData.CmdListsCount; n++)
            {
                ImDrawListPtr cmdList = drawData.CmdLists[n];

                GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
                GL.BufferData(BufferTarget.ArrayBuffer, cmdList.VtxBuffer.Size * 20, cmdList.VtxBuffer.Data, BufferUsageHint.StreamDraw);

                GL.BindBuffer(BufferTarget.ElementArrayBuffer, _indexBuffer);
                GL.BufferData(BufferTarget.ElementArrayBuffer, cmdList.IdxBuffer.Size * sizeof(ushort), cmdList.IdxBuffer.Data, BufferUsageHint.StreamDraw);

                int idxOffset = 0;
                for (int cmdi = 0; cmdi < cmdList.CmdBuffer.Size; cmdi++)
                {
                    ImDrawCmdPtr drawCmd = cmdList.CmdBuffer[cmdi];
                    GL.Scissor((int)drawCmd.ClipRect.X,
                               (int)(_window.Size.Y - drawCmd.ClipRect.W),
                               (int)(drawCmd.ClipRect.Z - drawCmd.ClipRect.X),
                               (int)(drawCmd.ClipRect.W - drawCmd.ClipRect.Y));

                    GL.BindTexture(TextureTarget.Texture2D, (int)drawCmd.TextureId);
                    GL.DrawElementsBaseVertex(PrimitiveType.Triangles, (int)drawCmd.ElemCount,
                        DrawElementsType.UnsignedShort, (IntPtr)(idxOffset * sizeof(ushort)), 0);
                    idxOffset += (int)drawCmd.ElemCount;
                }
            }

            GL.Disable(EnableCap.ScissorTest);
            GL.BindVertexArray(0);
            GL.UseProgram(0);
        }

        public void Dispose()
        {
            ImGui.DestroyContext();
        }

        private void CreateDeviceResources()
        {
            string vertexShaderSource = @"#version 330 core
            layout (location = 0) in vec2 Position;
            layout (location = 1) in vec2 UV;
            layout (location = 2) in vec4 Color;
            uniform mat4 projection_matrix;
            out vec2 Frag_UV;
            out vec4 Frag_Color;
            void main()
            {
                Frag_UV = UV;
                Frag_Color = Color;
                gl_Position = projection_matrix * vec4(Position.xy, 0, 1);
            }";

                        string fragmentShaderSource = @"#version 330 core
            in vec2 Frag_UV;
            in vec4 Frag_Color;
            uniform sampler2D Texture;
            out vec4 Out_Color;
            void main()
            {
                Out_Color = Frag_Color * texture(Texture, Frag_UV.st);
            }";

            _shader = CompileProgram(vertexShaderSource, fragmentShaderSource);

            _attribLocationTex = GL.GetUniformLocation(_shader, "Texture");
            _attribLocationProjMtx = GL.GetUniformLocation(_shader, "projection_matrix");

            _attribLocationPosition = 0;
            _attribLocationUV = 1;
            _attribLocationColor = 2;

            _vertexBuffer = GL.GenBuffer();
            _indexBuffer = GL.GenBuffer();
            _vertexArray = GL.GenVertexArray();

            GL.BindVertexArray(_vertexArray);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
            GL.EnableVertexAttribArray(_attribLocationPosition);
            GL.EnableVertexAttribArray(_attribLocationUV);
            GL.EnableVertexAttribArray(_attribLocationColor);

            GL.VertexAttribPointer(_attribLocationPosition, 2, VertexAttribPointerType.Float, false, 20, 0);
            GL.VertexAttribPointer(_attribLocationUV, 2, VertexAttribPointerType.Float, false, 20, 8);
            GL.VertexAttribPointer(_attribLocationColor, 4, VertexAttribPointerType.UnsignedByte, true, 20, 16);

            GL.BindVertexArray(0);
        }

        private int CompileProgram(string vertexCode, string fragmentCode)
        {
            int vertex = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertex, vertexCode);
            GL.CompileShader(vertex);

            int fragment = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragment, fragmentCode);
            GL.CompileShader(fragment);

            int program = GL.CreateProgram();
            GL.AttachShader(program, vertex);
            GL.AttachShader(program, fragment);
            GL.LinkProgram(program);

            GL.DeleteShader(vertex);
            GL.DeleteShader(fragment);

            return program;
        }

        private void SetFontTexture()
        {
            ImGuiIOPtr io = ImGui.GetIO();
            io.Fonts.GetTexDataAsRGBA32(out IntPtr pixels, out int width, out int height, out _);

            _fontTexture = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, _fontTexture);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, width, height, 0,
                PixelFormat.Rgba, PixelType.UnsignedByte, pixels);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            io.Fonts.SetTexID(_fontTexture);
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }
    }
}
