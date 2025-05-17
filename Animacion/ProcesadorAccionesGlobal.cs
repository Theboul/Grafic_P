namespace OpenTKCubo3D
{
    public static class ProcesadorAccionesGlobal
    {
        private static Thread? _hiloAnimacion;
        private static bool _activo = true;
        public static EscenaAcciones Escena { get; private set; } = new();

        public static void IniciarHilo()
        {
            _activo = true;
            _hiloAnimacion = new Thread(() =>
            {
                var reloj = new System.Diagnostics.Stopwatch();
                reloj.Start();
                float ultimoTiempo = 0;

                while (_activo)
                {
                    float actual = reloj.ElapsedMilliseconds / 1000f;
                    float delta = actual - ultimoTiempo;
                    ultimoTiempo = actual;

                    Escena.Actualizar(delta);

                    Thread.Sleep(16); // Puedes ajustar si deseas más precisión
                }
            })
            {
                IsBackground = true // Permite que el hilo no bloquee la app al cerrar
            };

            _hiloAnimacion.Start();
        }

        public static void DetenerHilo()
        {
            _activo = false;
            _hiloAnimacion?.Join();
        }

        public static void AplicarTodo()
        {
            Escena.Actualizar(0f); 
        }
    }
}