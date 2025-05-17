namespace OpenTKCubo3D
{
    public static class GestorAnimaciones
    {
        public static void GuardarLibreto(string nombreArchivo, List<AccionIndividual> acciones)
        {
            var libreto = new ArchivoLibreto
            {
                Acciones = acciones.Select(a => new AccionSerializada
                {
                    NombreObjeto = a.NombreObjeto,
                    TiempoInicio = a.TiempoInicio,
                    Keyframes = a.Libreto.Keyframes
                }).ToList()
            };

            Utilidades.Guardar(libreto, "animaciones", nombreArchivo);

            Console.WriteLine($"[Libreto guardado en animaciones/{nombreArchivo}.json]");
        }

        public static void CargarLibreto(string nombreArchivo)
        {
            var libreto = Utilidades.Cargar<ArchivoLibreto>("animaciones", nombreArchivo);

            foreach (var accionSerializada in libreto.Acciones)
            {
                if (!GestorEscenarios.EscenarioActual.Objetos.TryGetValue(accionSerializada.NombreObjeto, out var objeto))
                {
                    Console.WriteLine($"[Animación ignorada] No existe el objeto '{accionSerializada.NombreObjeto}' en el escenario.");
                    continue;
                }

                var accion = new AccionIndividual
                {
                    NombreObjeto = accionSerializada.NombreObjeto,
                    TiempoInicio = accionSerializada.TiempoInicio,
                    EstadoInicial = new EstadoTransformacionInicial
                    {
                        Posicion = objeto.Transform.Position,
                        Rotacion = objeto.Transform.Rotation,
                        Escala = objeto.Transform.Scale
                    },
                    Libreto = new LibretoKeyframes { Keyframes = accionSerializada.Keyframes }
                };

                ProcesadorAccionesGlobal.Escena.AccionesActivas.Add(accion);
            }

            Console.WriteLine($"[Libreto cargado desde animaciones/{nombreArchivo}.json]");
        }



        public class ArchivoLibreto
        {
            public List<AccionSerializada> Acciones { get; set; } = new();
        }

        public class AccionSerializada
        {
            public string NombreObjeto { get; set; } = string.Empty;
            public float TiempoInicio { get; set; }
            public List<FotogramaTransformacion> Keyframes { get; set; } = new();
        }
    }
}
