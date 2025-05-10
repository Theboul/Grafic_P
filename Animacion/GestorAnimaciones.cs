
namespace OpenTKCubo3D
{
    public static class GestorAnimaciones
    {
        private static readonly List<AnimacionObjeto> _animaciones = new();

        public static void Agregar(AnimacionObjeto animacion) => _animaciones.Add(animacion);

        public static void ActualizarTodo(float deltaTime)
        {
            foreach (var anim in _animaciones)
                anim.Actualizar(deltaTime);
        }

        public static void Limpiar() => _animaciones.Clear();

        public static void CargarTodasDesdeCarpeta()
        {
            Limpiar();

            string carpeta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "animaciones");
            if (!Directory.Exists(carpeta))
            {
                Console.WriteLine("No se encontró la carpeta de animaciones.");
                return;
            }

            string[] archivos = Directory.GetFiles(carpeta, "*.json");

            foreach (var archivo in archivos)
            {
                string nombreArchivo = Path.GetFileNameWithoutExtension(archivo);
                if (!GestorEscenarios.EscenarioActual.Objetos.ContainsKey(nombreArchivo))
                {
                    Console.WriteLine($"[Animación ignorada] No existe un objeto con el nombre '{nombreArchivo}' en el escenario.");
                    continue;
                }

                var libreto = Utilidades.Cargar<LibretoAnimacion>("animaciones", nombreArchivo);
                var objeto = GestorEscenarios.EscenarioActual.GetObjeto(nombreArchivo);
                var animacion = new AnimacionObjeto(objeto, libreto);
                Agregar(animacion);

                Console.WriteLine($"[Animación cargada] '{nombreArchivo}' aplicada al objeto.");
            }
        }
    }
}
