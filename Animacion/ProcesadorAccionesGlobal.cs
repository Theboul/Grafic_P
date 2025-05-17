namespace OpenTKCubo3D
{
    /// Gestor global de animaciones.
    /// Gestiona el hilo de animación, avanzando el tiempo global y delegando la ejecución a EscenaAnimacion.
    public static class ProcesadorAccionesGlobal
    {
        private static Thread? _hiloAnimacion;
        private static bool _activo = true;

        /// Escena de animación centralizada, donde viven todas las acciones.

        public static EscenaAcciones Escena { get; private set; } = new();

        /// Inicia el hilo de animación que actualiza el tiempo global y procesa las acciones activas.
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


        /// Detiene el hilo de animación de manera segura.

        public static void DetenerHilo()
        {
            _activo = false;
            _hiloAnimacion?.Join();
        }


        /// Método opcional si quieres forzar la aplicación desde el hilo principal.
        /// En el modelo procesador puro no es obligatorio, pues Escena.Actualizar ya aplica.

        public static void AplicarTodo()
        {
            Escena.Actualizar(0f); // Puedes forzar un update en tiempo cero si deseas aplicar todo en el frame actual sin delta
        }
    }
}