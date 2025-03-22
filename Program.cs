// Figura 3D
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OpenTKCubo3D
{
    class Program : GameWindow
    {
        private float _angleY; 
        private float _angleX; 
        private int _vertexBufferObject;
         private int _elementBufferObject;
        private int _vertexArrayObject;
        private int _shaderProgram;
        private Matrix4 _view;
        private Matrix4 _projection;
        private int _ejesVertexArrayObject;
        private int _ejesVertexBufferObject;
        private int _ejesElementBufferObject;
        private float _figureX = -1.0f;
        private float _figureY = -1.0f;
        private float _figureZ = 0.5f;


        public Program(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
        }

        protected override void OnLoad()
        {
           base.OnLoad();
            GL.ClearColor(0.1f, 0.1f, 0.1f, 0.1f);
            GL.Enable(EnableCap.DepthTest);

            //ejes X, Y, Z
            float[] ejesVertices = {
            // Eje X (Rojo)
           -2.0f,  0.0f,  0.0f,  1.0f, 0.0f, 0.0f, // Inicio (negativo)
            2.0f,  0.0f,  0.0f,  1.0f, 0.0f, 0.0f, // Fin (positivo)

            // Eje Y (Verde)
            0.0f, -2.0f,  0.0f,  0.0f, 1.0f, 0.0f, // Inicio (negativo)
            0.0f,  2.0f,  0.0f,  0.0f, 1.0f, 0.0f, // Fin (positivo)

            // Eje Z (Azul)
            0.0f,  0.0f, -2.0f,  0.0f, 0.0f, 1.0f, // Inicio (negativo)
            0.0f,  0.0f,  2.0f,  0.0f, 0.0f, 1.0f  // Fin (positivo)
            };

            uint[] ejesIndices = {
            0, 1, // Eje X
            2, 3, // Eje Y
            4, 5  // Eje Z
            };

         _ejesVertexArrayObject = GL.GenVertexArray();
         GL.BindVertexArray(_ejesVertexArrayObject);

         _ejesVertexBufferObject = GL.GenBuffer();
         GL.BindBuffer(BufferTarget.ArrayBuffer, _ejesVertexBufferObject);
         GL.BufferData(BufferTarget.ArrayBuffer, ejesVertices.Length * sizeof(float), ejesVertices, BufferUsageHint.StaticDraw);

         _ejesElementBufferObject = GL.GenBuffer();
         GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ejesElementBufferObject);
         GL.BufferData(BufferTarget.ElementArrayBuffer, ejesIndices.Length * sizeof(uint), ejesIndices, BufferUsageHint.StaticDraw);

         GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
         GL.EnableVertexAttribArray(0);

         GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
         GL.EnableVertexAttribArray(1);


            // Configurar los vértices Frontales Figura U
            float[] vertices = {
            // Columnas  
           -0.5f, -0.5f,  0.5f,  1.0f, 0.0f, 1.0f,   // 1 Inferior Izquierda frontal
           -0.5f,  0.5f,  0.5f,  1.0f, 0.0f, 1.0f,   // 2 Superior Izquierda frontal
            0.5f, -0.5f,  0.5f,  1.0f, 0.0f, 1.0f,   // 3 Inferior Derecha fontal
            0.5f,  0.5f,  0.5f,  1.0f, 0.0f, 1.0f,   // 4 Superior Derecha frontal
           -0.5f, -0.5f,  0.2f,  1.0f, 0.0f, 1.0f,   // 5 Inferior izquierda trasera
           -0.5f,  0.5f,  0.2f,  1.0f, 0.0f, 1.0f,   // 6 Superior izquierda trasera
            0.5f, -0.5f,  0.2f,  1.0f, 0.0f, 1.0f,   // 7 Inferior derecha trasera
            0.5f,  0.5f,  0.2f,  1.0f, 0.0f, 1.0f,   // 8 Superior derecha trasera

            // segundas Columnas
           -0.3f, -0.3f,  0.5f,  1.0f, 0.0f, 1.0f,   // 9 Inferior izquierda frontal
           -0.3f,  0.5f,  0.5f,  1.0f, 0.0f, 1.0f,   // 10 Superior izquierda frontal
            0.3f, -0.3f,  0.5f,  1.0f, 0.0f, 1.0f,   // 11 Inferior derecha frontal
            0.3f,  0.5f,  0.5f,  1.0f, 0.0f, 1.0f,   // 12 Superior derecha frontal
           -0.3f, -0.3f,  0.2f,  1.0f, 0.0f, 1.0f,   // 13 Inferior izquierda traseras
           -0.3f,  0.5f,  0.2f,  1.0f, 0.0f, 1.0f,   // 14 Superior izquierda traseras
            0.3f, -0.3f,  0.2f,  1.0f, 0.0f, 1.0f,   // 15 Inferior derecha traseras
            0.3f,  0.5f,  0.2f,  1.0f, 0.0f, 1.0f,   // 16 Superior derecha traseras
            
            // Base 
           -0.5f, -0.5f,  0.5f,  1.0f, 0.0f, 1.0f,   // 17 Izquierda frontal
            0.5f, -0.5f,  0.5f,  1.0f, 0.0f, 1.0f,   // 18 Derecha frontal
           -0.5f, -0.5f,  0.2f,  1.0f, 0.0f, 1.0f,   // 19 Izquierda trasera
            0.5f, -0.5f,  0.2f,  1.0f, 0.0f, 1.0f,   // 20 Derecha trasera

            // segunda Base 
           -0.3f, -0.3f,  0.5f,  1.0f, 0.0f, 1.0f,   // 21 Izquierda frontal
            0.3f, -0.3f,  0.5f,  1.0f, 0.0f, 1.0f,   // 22 Derecha frontal
           -0.3f, -0.3f,  0.2f,  1.0f, 0.0f, 1.0f,   // 23 Izquierda trasera
            0.3f, -0.3f,  0.2f,  1.0f, 0.0f, 1.0f,   // 24 Derecha trasera
            };

            uint[] indices = {
                
            //Cara Principal
            0,1,  1,9,  9,8,  8,10,  10,11,  11,3,  3,2,  0,2, 
            //Cara Trasera
            4,5,  5,13,  13,12,  12,14,  14,15,  15,7,  7,6,  4,6,
            //Conexiones
            0,4,  1,5,  9,13,  8,12,  10,14, 11,15,  3,7,  2,6  
            
            };

            
            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);

            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            _elementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            string vertexShaderSource = @"
                #version 330 core
                layout(location = 0) in vec3 aPosition;
                layout(location = 1) in vec3 aColor;
                out vec3 fragColor;
                uniform mat4 model;
                uniform mat4 view;
                uniform mat4 projection;
                void main()
                {
                    gl_Position = projection * view * model * vec4(aPosition, 1.0);
                    fragColor = aColor;
                }
            ";

            string fragmentShaderSource = @"
                #version 330 core
                in vec3 fragColor;
                out vec4 color;
                void main()
                {
                    color = vec4(fragColor, 1.0);
                }
            ";

            int vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, vertexShaderSource);
            GL.CompileShader(vertexShader);

            int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, fragmentShaderSource);
            GL.CompileShader(fragmentShader);

            _shaderProgram = GL.CreateProgram();
            GL.AttachShader(_shaderProgram, vertexShader);
            GL.AttachShader(_shaderProgram, fragmentShader);
            GL.LinkProgram(_shaderProgram);

            GL.DetachShader(_shaderProgram, vertexShader);
            GL.DetachShader(_shaderProgram, fragmentShader);
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);

            _view = Matrix4.LookAt(new Vector3(2, 3, 5), Vector3.Zero, Vector3.UnitY);
            _projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, Size.X / (float)Size.Y, 0.1f, 100f);
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Size.X, Size.Y);
            _projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, Size.X / (float)Size.Y, 0.1f, 100f);
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
            var input = KeyboardState;

            if (input.IsKeyDown(Keys.Escape))
            {
                Close();
            }

            if (input.IsKeyDown(Keys.Left)) _angleY -= 0.02f;
            if (input.IsKeyDown(Keys.Right)) _angleY += 0.02f;
            if (input.IsKeyDown(Keys.Up)) _angleX -= 0.02f;
            if (input.IsKeyDown(Keys.Down)) _angleX += 0.02f;

            if (input.IsKeyDown(Keys.W)) _figureZ -= 0.02f; 
            if (input.IsKeyDown(Keys.S)) _figureZ += 0.02f; 
            if (input.IsKeyDown(Keys.A)) _figureX -= 0.02f; 
            if (input.IsKeyDown(Keys.D)) _figureX += 0.02f; 
            if (input.IsKeyDown(Keys.Q)) _figureY += 0.02f; 
            if (input.IsKeyDown(Keys.E)) _figureY -= 0.02f; 
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.UseProgram(_shaderProgram);

            // Dibujar los ejes 
            Matrix4 axisModel = Matrix4.Identity; 
            GL.UniformMatrix4(GL.GetUniformLocation(_shaderProgram, "model"), false, ref axisModel);
            GL.UniformMatrix4(GL.GetUniformLocation(_shaderProgram, "view"), false, ref _view);
            GL.UniformMatrix4(GL.GetUniformLocation(_shaderProgram, "projection"), false, ref _projection);

            GL.BindVertexArray(_ejesVertexArrayObject);
            GL.DrawElements(PrimitiveType.Lines, 6, DrawElementsType.UnsignedInt, 0);

            // Dibujar la figura con rotación y traslación
            Matrix4 model = Matrix4.CreateTranslation(_figureX, _figureY, _figureZ) * Matrix4.CreateRotationY(_angleY) * Matrix4.CreateRotationX(_angleX);
            GL.UniformMatrix4(GL.GetUniformLocation(_shaderProgram, "model"), false, ref model);

            GL.BindVertexArray(_vertexArrayObject);
            GL.DrawElements(PrimitiveType.Lines, 48, DrawElementsType.UnsignedInt, 0);

            SwapBuffers();
        }

        static void Main(string[] args)
        {
            var nativeWindowSettings = new NativeWindowSettings()
            {
                ClientSize = new Vector2i(800, 600),
                Title = "Figura U 3D",
                Flags = ContextFlags.Default,
                Profile = ContextProfile.Core,
            };

            using (var window = new Program(GameWindowSettings.Default, nativeWindowSettings))
            {
                window.Run();
            }
        }
    }
}