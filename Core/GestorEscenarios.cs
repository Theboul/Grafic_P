using OpenTK.Mathematics;
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
            {
                CrearEscenarioVacio(nombreArchivo);
            }

            _escenarios[nombreArchivo] = escenario;
            _escenarioActual = escenario;
        }

        public static void AgregarArchObj(string nombre, string ruta, Vector3? posicion = null, float? escala = null)
        {
            var objetos = LectorModeloObj.CargarObjetoDesdeObj(ruta);

            if (objetos.Count > 0)
            {
                var objeto = objetos.Values.First(); // Da igual como se llame internamente

                // Aplicar transformaciones si se pide
                if (escala.HasValue)
                    objeto.Transform.Escalate(escala.Value);

                if (posicion.HasValue)
                    objeto.Transform.Position = posicion.Value;

                objeto.RecalcularCentroDeMasa();

                // Aquí lo guardas en tu escenario con el nombre que TÚ quieres
                GestorEscenarios.EscenarioActual.AgregarObjeto(nombre, objeto);

            }
            else
            {
                Console.WriteLine($"❗ No se encontraron objetos en el archivo: {ruta}");
            }
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
        
        public static Dictionary<string, ObjetoU> BuscarObjetosPorPrefijo(string prefijo)
        {
            return EscenarioActual.Objetos
                .Where(kv => kv.Key.StartsWith(prefijo, StringComparison.OrdinalIgnoreCase))
                .ToDictionary(kv => kv.Key, kv => kv.Value);
        }
    } 
}