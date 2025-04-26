// Figura 3D
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL;
using OpenTKCubo3D.UI;
 

namespace OpenTKCubo3D
{
    
    class Program : GameWindow
    {
        private Matrix4 _view, _projection;
        private ImGuiController _imgui = null!;
        private PanelTransformaciones panel = null!;
       
        public Program(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
        }

        protected override void OnLoad()
        {
           base.OnLoad();
           

            GL.ClearColor(0.0f, 0.0f, 0.1f, 0.1f);
            GL.Enable(EnableCap.DepthTest);
            //GL.Enable(EnableCap.CullFace);


            GestorEscenarios.Cargar("objetos_U");
            //GestorEscenarios.Cargar("objetos_U1");

            
            /*
            GestorEscenarios.CrearEscenarioVacio("objetos_U1");
            var nuevoObjeto = new ObjetoU( new Puntos(0.0f, 0.0f, -2.0f), 1.0f, 1.0f, 0.3f, Color4.Purple);
            var nuevoObjeto1 = new ObjetoU( new Puntos(0.0f, 0.0f, 0.0f), 1.0f, 1.0f, 0.3f, Color4.SkyBlue);
            var nuevoObjeto2 = new ObjetoU( new Puntos(0.0f, 1.0f, -1.0f), 1.0f, 1.0f, 0.3f, Color4.Orange);
            GestorEscenarios.EscenarioActual.AgregarObjeto("u1_2", nuevoObjeto);
            GestorEscenarios.EscenarioActual.AgregarObjeto("u1_3", nuevoObjeto1);
            GestorEscenarios.EscenarioActual.AgregarObjeto("u1_4", nuevoObjeto2);
            GestorEscenarios.EscenarioActual.RecalcularCentroDeMasa();
            GestorEscenarios.Guardar("objetos_U1");
            GestorEscenarios._escenarios["objetos_U1"].Objetos.Add("u1_5", objetoNuevo3);
            */
            

            panel = new PanelTransformaciones(GestorEscenarios.EscenarioActual);
           _imgui = new ImGuiController(this);

            _view = Matrix4.LookAt(new Vector3(2, 3, 5), Vector3.Zero, Vector3.UnitY);
            _projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, Size.X / (float)Size.Y, 0.1f, 100f);
                        
        }

        
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            _imgui.Update((float)args.Time); // actualizamos ImGui
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.LoadMatrix(ref _projection); // Usa la matriz de proyección que ya tenías

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.LoadMatrix(ref _view);

            UIEditor.Render(panel);     

            foreach (var escenario in GestorEscenarios.Todos.Values)
            {        
                escenario.DibujarTodo(Matrix4.Identity);
            }

            //GestorEscenarios.EscenarioActual?.DibujarTodo(Matrix4.Identity); 
           _imgui.Render(); // renderizamos ImGui
           SwapBuffers();
       }
           

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Size.X, Size.Y);
            _projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, Size.X / (float)Size.Y, 0.1f, 100f);
                 
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