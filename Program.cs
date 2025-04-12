// Figura 3D
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL;

namespace OpenTKCubo3D
{
    class Program : GameWindow
    {
        private Matrix4 _view, _projection;
       
        private readonly Escenario escenario;

        public Program(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
            escenario = new Escenario();
        }

        protected override void OnLoad()
        {
           base.OnLoad();
            GL.ClearColor(0.0f, 0.0f, 0.1f, 0.1f);
            GL.Enable(EnableCap.DepthTest);

            escenario.Cargar("C:/Users/Usuario/Grafic_Figura/JsonClass/mi_escenario.json");
            //escenario.IniEscenario(2, Color4.Gold);
            //escenario.Guardar("C:/Users/Usuario/Grafic_Figura/JsonClass/mi_escenario.json");

            
            
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
                    
            escenario.DibujarTodo(); 
                         

          SwapBuffers();
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