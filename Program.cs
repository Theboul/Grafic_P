// Figura 3D
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL;
using OpenTKCubo3D.UI;
using Keys = OpenTK.Windowing.GraphicsLibraryFramework.Keys;


namespace OpenTKCubo3D
{
    
    class Program : GameWindow
    {
        private Matrix4 _view, _projection;
        private ImGuiController _imgui = null!;
        private PanelTransformaciones panel = null!;
        private Vector3 _cameraPosition = new Vector3(0f, 20f, 20f);
        private Vector3 _cameraFront = -Vector3.UnitZ;
        private Vector3 _cameraUp = Vector3.UnitY;
        private float _yaw = -90f;
        private float _pitch = 0f;
        private float _rotSpeed = 50f;
        private float _cameraSpeed = 10f;
        private bool _espacioPresionado = false;
        private AnimacionAutoConGiro? _animacionAuto;
        private List<Vector3> _rutaBase = new(); // lista original

        private List<Vector3> _ruta = new();



       
        public Program(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
        }

        protected override void OnLoad()
        {
           base.OnLoad();

            GL.ClearColor(0.196078f, 0.6f, 0.8f, 0.0f);
            GL.Enable(EnableCap.DepthTest);            
            //GL.Enable(EnableCap.CullFace);              
            GL.DepthFunc(DepthFunction.Less);           
            //GL.CullFace(TriangleFace.Back);             
            GL.FrontFace(FrontFaceDirection.Ccw);  

            GestorEscenarios.Cargar("Pista_1");
            //GestorEscenarios.CrearEscenarioVacio("Pista2");


            if(GestorEscenarios.EscenarioActual.Objetos.TryGetValue("arbol", out var objetoArbol)){
                objetoArbol.RecalcularCentroDeMasa();
                objetoArbol.Transform.Position = new Vector3(-7f, 0.2f, 10f);
            }

            if (GestorEscenarios.EscenarioActual.Objetos.TryGetValue("auto", out var auto))
            {
                auto.RecalcularCentroDeMasa();
                auto.Transform.Position = new Vector3(-0.5f, 0.2f, 0f); 
                _rutaBase = RutaAuto.ObtenerRuta();
                _ruta = SplineGenerator.GenerarRutaSuavizada(_rutaBase, pasosPorTramo: 15);
                _animacionAuto = new AnimacionAutoConGiro(auto, _ruta, velocidad: 2f);
                _animacionAuto.Activa = false;
            }

            /*
            GestorEscenarios.CrearEscenarioVacio("objetos_U1");
            var nuevoObjeto = new ObjetoU( new Puntos(0.0f, 0.0f, -2.0f), 1.0f, 1.0f, 0.3f, Color4.Purple);
            var nuevoObjeto1 = new ObjetoU( new Puntos(0.0f, 0.0f, 0.0f), 1.0f, 1.0f, 0.3f, Color4.SkyBlue);
            var nuevoObjeto2 = new ObjetoU( new Puntos(0.0f, 1.0f, -1.0f), 1.0f, 1.0f, 0.3f, Color4.Orange);
            GestorEscenarios.EscenarioActual.AgregarObjeto("u1_morado", nuevoObjeto);
            GestorEscenarios.EscenarioActual.AgregarObjeto("u2_celeste", nuevoObjeto1);
            GestorEscenarios.EscenarioActual.AgregarObjeto("u3_naranja", nuevoObjeto2);
            GestorEscenarios.EscenarioActual.RecalcularCentroDeMasa();
            GestorEscenarios.Guardar("objetos_U1");
            GestorEscenarios._escenarios["objetos_U1"].Objetos.Add("u1_5", objetoNuevo3);
            */
            
            panel = new PanelTransformaciones(GestorEscenarios.EscenarioActual);
           _imgui = new ImGuiController(this);

            //_view = Matrix4.LookAt(new Vector3(0, 20, 20), Vector3.Zero, Vector3.UnitY);
            _projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, Size.X / (float)Size.Y, 0.1f, 100f);
                        
        }

        
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            GL.Enable(EnableCap.DepthTest);
            
            _imgui.Update((float)args.Time); // actualizamos ImGui
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);


            _view = Matrix4.LookAt(_cameraPosition, _cameraPosition + _cameraFront, _cameraUp);

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


        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            float deltaTime = (float)args.Time;
            var input = KeyboardState;

            // === ROTACIÓN CON FLECHAS ===
            if (input.IsKeyDown(Keys.Left))
                _yaw -= _rotSpeed * deltaTime;

            if (input.IsKeyDown(Keys.Right))
                _yaw += _rotSpeed * deltaTime;

            if (input.IsKeyDown(Keys.Up))
                _pitch += _rotSpeed * deltaTime;

            if (input.IsKeyDown(Keys.Down))
                _pitch -= _rotSpeed * deltaTime;

            _pitch = MathHelper.Clamp(_pitch, -89f, 89f);

            // Recalcular la dirección de la cámara
            Vector3 front;
            front.X = MathF.Cos(MathHelper.DegreesToRadians(_yaw)) * MathF.Cos(MathHelper.DegreesToRadians(_pitch));
            front.Y = MathF.Sin(MathHelper.DegreesToRadians(_pitch));
            front.Z = MathF.Sin(MathHelper.DegreesToRadians(_yaw)) * MathF.Cos(MathHelper.DegreesToRadians(_pitch));
            _cameraFront = Vector3.Normalize(front);

            Vector3 right = Vector3.Normalize(Vector3.Cross(_cameraFront, _cameraUp));

            // === MOVIMIENTO CON WASDQE ===
            if (input.IsKeyDown(Keys.W))
                _cameraPosition += _cameraFront * _cameraSpeed * deltaTime;

            if (input.IsKeyDown(Keys.S))
                _cameraPosition -= _cameraFront * _cameraSpeed * deltaTime;

            if (input.IsKeyDown(Keys.A))
                _cameraPosition -= right * _cameraSpeed * deltaTime;

            if (input.IsKeyDown(Keys.D))
                _cameraPosition += right * _cameraSpeed * deltaTime;

            if (input.IsKeyDown(Keys.E))
                _cameraPosition += _cameraUp * _cameraSpeed * deltaTime;

            if (input.IsKeyDown(Keys.Q))
                _cameraPosition -= _cameraUp * _cameraSpeed * deltaTime;

            
            bool estaPresionandoEspacio = input.IsKeyDown(Keys.Space);

            if (estaPresionandoEspacio && !_espacioPresionado)
            {
                if (_animacionAuto != null)
                {
                    _animacionAuto.Activa = !_animacionAuto.Activa;
                    Console.WriteLine(_animacionAuto.Activa ? " Animación activada" : " Animación detenida");
                }
            }

            _espacioPresionado = estaPresionandoEspacio;

            //_animacionAuto?.Actualizar(deltaTime);
            _animacionAuto?.Actualizar((float)args.Time);

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