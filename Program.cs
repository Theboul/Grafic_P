// Figura 3D
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL;

namespace OpenTKCubo3D
{
    class Program : GameWindow
    {
        private Matrix4 _view;
        private Matrix4 _projection;
        //
        //private Figura? Figura_U;
        private List<ObjetoU> objetos = new List<ObjetoU>();
        private int cantidadCopias = 4; 
        //private ObjetoU ObjetoU = null!;

        public Program(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
        }

        protected override void OnLoad()
        {
           base.OnLoad();
            GL.ClearColor(0.0f, 0.0f, 0.1f, 0.1f);
            GL.Enable(EnableCap.DepthTest);
            
           /* for (int i = 0; i < cantidadCopias; i++)
            {
                float angle = i * (2 * MathHelper.Pi / cantidadCopias);
                float x = (float)Math.Cos(angle) * 1.1f;
                float z = (float)Math.Sin(angle) * 1.1f;
    
                objetos.Add(new ObjetoU(new Puntos(x, 0.0f, z), 1.0f, 1.0f, 0.3f, Color4.Red));
            }*/
            
            var random = new Random();
            float areaDistribucion = 2.0f; 

            for (int i = 0; i < cantidadCopias; i++)
            {
            
            float x = (float)(random.NextDouble() * areaDistribucion - areaDistribucion/2);
            float y = (float)(random.NextDouble() * areaDistribucion - areaDistribucion/2);
            float z = (float)(random.NextDouble() * areaDistribucion - areaDistribucion/2);
        
            objetos.Add(new ObjetoU(new Puntos(x, y, z), 1.5f, 1.5f, 0.3f, Color4.Blue));             
            }

           // ObjetoU = new ObjetoU(new Puntos(0, 0, 0), 1.5f, 1.5f, 0.3f, Color4.Red);

            _view = Matrix4.LookAt(new Vector3(2, 3, 5), Vector3.Zero, Vector3.UnitY);

            _projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, Size.X / (float)Size.Y, 0.1f, 100f);
                        
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Size.X, Size.Y);
            _projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, Size.X / (float)Size.Y, 0.1f, 100f);
                 
        }


        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref _projection); // Usa la matriz de proyección que ya tenías

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref _view);

            DibujarEjes();
          
           foreach (var objeto in objetos)
           {
             objeto.Dibujar();
           }
           // ObjetoU?.Dibujar();

          SwapBuffers();
       }
           
        private void DibujarEjes()
        {
          GL.Begin(PrimitiveType.Lines);
          {
          // Eje X (Rojo)
          GL.Color3(1.0f, 0.0f, 0.0f);
          GL.Vertex3(-2.0f, 0.0f, 0.0f);
          GL.Vertex3(2.0f, 0.0f, 0.0f);

          // Eje Y (Verde)
          GL.Color3(0.0f, 1.0f, 0.0f);
          GL.Vertex3(0.0f, -2.0f, 0.0f);
          GL.Vertex3(0.0f, 2.0f, 0.0f);

          // Eje Z (Azul)
          GL.Color3(0.0f, 0.0f, 1.0f);
          GL.Vertex3(0.0f, 0.0f, -2.0f);
          GL.Vertex3(0.0f, 0.0f, 2.0f);
          }
          GL.End();
       }

        static void Main(string[] args)
        {
            var nativeWindowSettings = new NativeWindowSettings()
            {
                ClientSize = new Vector2i(800, 600),
                Title = "Figura U 3D",
                Flags = ContextFlags.Default,
                Profile = ContextProfile.Compatability,
            };

            using (var window = new Program(GameWindowSettings.Default, nativeWindowSettings))
            {
                window.Run();
            }
        }
    }
}