// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");


using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OpenTKCubo3D
{
    class Program : GameWindow
    {
        private float _angleY; // Ángulo de rotación en el eje Y (izquierda/derecha)
        private float _angleX; // Ángulo de rotación en el eje X (arriba/abajo)
        private int _vertexBufferObject;
        private int _vertexArrayObject;
        private int _shaderProgram;
        private Matrix4 _view;
        private Matrix4 _projection;

        public Program(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
        }

        protected override void OnLoad()
        {
           base.OnLoad();
            GL.ClearColor(0.1f, 0.1f, 0.1f, 0.1f);
            GL.Enable(EnableCap.DepthTest);

            // Configurar los vértices del cubo
            float[] vertices = {
// Columna izquierda (frontal)
-0.5f, -0.5f,  0.5f,  1.0f, 0.0f, 0.0f,  // Inferior izquierda
-0.5f,  0.5f,  0.5f,  1.0f, 0.0f, 0.0f,  // Superior izquierda

//hola

// Columna derecha (frontal)
0.5f, -0.5f,  0.5f,  1.0f, 0.647f, 0.0f,  // Inferior derecha
0.5f,  0.5f,  0.5f,  0.0f, 0.0f, 1.0f,  // Superior derecha

// Base (frontal)
-0.5f, -0.5f,  0.5f,  1.0f, 0.0f, 0.0f,  // Izquierda
0.5f, -0.5f,  0.5f,  1.0f, 0.647f, 0.0f,  // Derecha

// Columna izquierda (trasera)
-0.5f, -0.5f,  0.2f,  1.0f, 0.0f, 0.0f,  // Inferior izquierda
-0.5f,  0.5f,  0.2f,  0.0f, 1.0f, 0.0f,  // Superior izquierda

// Columna derecha (trasera)
0.5f, -0.5f,  0.2f,  1.0f, 0.647f, 0.0f,  // Inferior derecha
0.5f,  0.5f,  0.2f,  0.0f, 0.0f, 1.0f,  // Superior derecha

// Base (trasera)
-0.5f, -0.5f, 0.2f,  1.0f, 0.0f, 0.0f,  // Izquierda
0.5f, -0.5f,  0.2f,  1.0f, 0.647f, 0.0f,  // Derecha

// Conexiones entre frente y atrás
-0.5f, -0.5f,  0.5f,  1.0f, 0.0f, 0.0f,  
-0.5f, -0.5f,  0.2f,  1.0f, 0.0f, 0.0f,  

0.5f, -0.5f,  0.5f,  1.0f, 0.647f, 0.0f,  
0.5f, -0.5f,  0.2f,  1.0f, 0.647f, 0.0f,  

-0.5f,  0.5f,  0.5f,  0.0f, 1.0f, 0.0f,  
-0.5f,  0.5f,  0.2f,  0.0f, 1.0f, 0.0f,  

0.5f,  0.5f,  0.5f,  0.0f, 0.0f, 1.0f,  
0.5f,  0.5f,  0.2f,  0.0f, 0.0f, 1.0f,  


//segundo 

// Columna izquierda (frontal)
-0.3f, -0.3f,  0.5f,  1.0f, 0.0f, 0.0f,  // Inferior izquierda
-0.3f,  0.5f,  0.5f,  0.0f, 1.0f, 0.0f,  // Superior izquierda

// Columna derecha (frontal)
 0.3f, -0.3f,  0.5f,  1.0f, 0.647f, 0.0f,  // Inferior derecha
 0.3f,  0.5f,  0.5f,  0.0f, 0.0f, 1.0f,  // Superior derecha

// Base (frontal)
-0.3f, -0.3f,  0.5f,  1.0f, 0.0f, 0.0f,  // Izquierda
 0.3f, -0.3f,  0.5f,  1.0f, 0.647f, 0.0f,  // Derecha

// Columna izquierda (trasera)
-0.3f, -0.3f, 0.2f,  1.0f, 0.0f, 0.0f,  // Inferior izquierda
-0.3f,  0.5f, 0.2f,  0.0f, 1.0f, 0.0f,  // Superior izquierda

// Columna derecha (trasera)
 0.3f, -0.3f, 0.2f,  1.0f, 0.647f, 0.0f,  // Inferior derecha
 0.3f,  0.5f, 0.2f,  0.0f, 0.0f, 1.0f,  // Superior derecha

// Base (trasera)
-0.3f, -0.3f, 0.2f,  1.0f, 0.0f, 0.0f,  // Izquierda
 0.3f, -0.3f, 0.2f,  1.0f, 0.647f, 0.0f,  // Derecha

// Conexiones entre frente y atrás
-0.3f, -0.3f,  0.5f,  1.0f, 0.0f, 0.0f,  
-0.3f, -0.3f,  0.2f,  1.0f, 0.0f, 0.0f,  

 0.3f, -0.3f,  0.5f,  1.0f, 0.647f, 0.0f,  
 0.3f, -0.3f,  0.2f,  1.0f, 0.647f, 0.0f,  

-0.3f,  0.5f,  0.5f,  0.0f, 1.0f, 0.0f,  
-0.3f,  0.5f,  0.2f,  0.0f, 1.0f, 0.0f,  

 0.3f,  0.5f,  0.5f,  0.0f, 0.0f, 1.0f,  
 0.3f,  0.5f,  0.2f,  0.0f, 0.0f, 1.0f, 

// Conexion entre los dos flujos
-0.5f,  0.5f,  0.5f,  0.0f, 1.0f, 0.0f,  // Columna Superior izquierda
-0.3f,  0.5f,  0.5f,  0.0f, 1.0f, 0.0f,  // s2 Columna Superior izquierda

-0.5f,  0.5f,  0.2f,  0.0f, 1.0f, 0.0f,  // trasera Superior izquierda
-0.3f,  0.5f, 0.2f,  0.0f, 1.0f, 0.0f,  // s2 atrasera Superior izquierda

0.5f,  0.5f,  0.5f,  0.0f, 0.0f, 1.0f,  // Columna Superior derecha
0.3f,  0.5f,  0.5f,  0.0f, 0.0f, 1.0f,  // s2 Columna Superior derecha

0.5f,  0.5f,  0.2f,  0.0f, 0.0f, 1.0f,  // trasera Superior derecha
 0.3f,  0.5f, 0.2f,  0.0f, 0.0f, 1.0f,  // s2 trasera Superior derecha

};

            // Crear y enlazar el VAO
            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);

            // Crear y enlazar el VBO
            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            // Configurar el atributo de posición (posición y color están intercalados)
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            // Compilar shaders
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

            // Configurar la vista y la proyección
            _view = Matrix4.LookAt(new Vector3(0, 0, 5), Vector3.Zero, Vector3.UnitY);
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

            // Rotar el cubo con las flechas del teclado
            if (input.IsKeyDown(Keys.Left))
            {
                _angleY -= 0.02f; // Rotación en el eje Y (izquierda)
            }
            if (input.IsKeyDown(Keys.Right))
            {
                _angleY += 0.02f; // Rotación en el eje Y (derecha)
            }
            if (input.IsKeyDown(Keys.Up))
            {
                _angleX -= 0.02f; // Rotación en el eje X (arriba)
            }
            if (input.IsKeyDown(Keys.Down))
            {
                _angleX += 0.02f; // Rotación en el eje X (abajo)
            }
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.UseProgram(_shaderProgram);

            // Configurar las matrices de transformación
            Matrix4 model = Matrix4.CreateRotationY(_angleY) * Matrix4.CreateRotationX(_angleX);
            GL.UniformMatrix4(GL.GetUniformLocation(_shaderProgram, "model"), false, ref model);
            GL.UniformMatrix4(GL.GetUniformLocation(_shaderProgram, "view"), false, ref _view);
            GL.UniformMatrix4(GL.GetUniformLocation(_shaderProgram, "projection"), false, ref _projection);

            // Dibujar el cubo
            GL.BindVertexArray(_vertexArrayObject);
            GL.DrawArrays(PrimitiveType.Lines, 0, 72); 


            SwapBuffers();
        }

        static void Main(string[] args)
        {
            var nativeWindowSettings = new NativeWindowSettings()
            {
                ClientSize = new Vector2i(800, 600),
                Title = "Cubo 3D con OpenTK y Shaders",
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