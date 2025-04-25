

namespace OpenTKCubo3D{
    public static class GestorEscenarios
    {
        public static Dictionary<string, Escenario> _escenarios = new Dictionary<string, Escenario>();
        private static Escenario? _escenarioActual;
        public static Escenario EscenarioActual => _escenarioActual ?? throw new InvalidOperationException("No hay escenario cargado.");
        public static IReadOnlyDictionary<string, Escenario> Todos => _escenarios;

        public static void Cargar(string nombreArchivo)
        {
            var escenario = Utilidades.Cargar<Escenario>("escenarios", nombreArchivo);
            if (escenario.Objetos.Count == 0)
                escenario = new Escenario();

            _escenarios[nombreArchivo] = escenario;
            _escenarioActual = escenario;
        }

        public static void Guardar(string nombreArchivo)
        {
            if (_escenarioActual != null)
                Utilidades.Guardar(_escenarioActual, "escenarios", nombreArchivo);
        }

        public static void CrearEscenarioVacio(string nombreArchivo)
        {
            var nuevo = new Escenario(); 
            _escenarios[nombreArchivo] = nuevo;
            _escenarioActual = nuevo;
        }

        public static bool Cambiar(string nombreArchivo)
        {
            if (_escenarios.ContainsKey(nombreArchivo))
            {
                _escenarioActual = _escenarios[nombreArchivo];
                return true;
            }
            return false;
        }

        public static void GuardarEntidad<T>(T entidad, string carpeta, string nombreArchivo)
        {
            Utilidades.Guardar(entidad, carpeta, nombreArchivo);
        }

        public static T CargarEntidad<T>(string carpeta, string nombreArchivo) where T : new()
        {
            return Utilidades.Cargar<T>(carpeta, nombreArchivo);
        }

    } 
}