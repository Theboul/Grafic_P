using OpenTK.Mathematics;

namespace OpenTKCubo3D
{
    /// Utilidades para crear acciones de animación bajo el modelo procesador centralizado.
    /// Las acciones creadas deben ser añadidas a EscenaAnimacion.AccionesActivas.

    public static class UtilidadesAnimacion
    {
        public static AccionIndividual CrearAccionTraslacion(string nombreObjeto, float tiempoInicio, Vector3 desplazamiento, float duracion)
        {
            return CrearAccionTransformacion(nombreObjeto, tiempoInicio, desplazamiento, null, null, duracion);
        }

        public static AccionIndividual CrearAccionRotacion(string nombreObjeto, float tiempoInicio, Vector3 rotacionDeltaEuler, float duracion)
        {
            return CrearAccionTransformacion(nombreObjeto, tiempoInicio, null, rotacionDeltaEuler, null, duracion);
        }

        public static AccionIndividual CrearAccionEscala(string nombreObjeto, float tiempoInicio, Vector3 escalaFinal, float duracion)
        {
            return CrearAccionTransformacion(nombreObjeto, tiempoInicio, null, null, escalaFinal, duracion);
        }

        /// Crea una acción completa de transformación (traslación, rotación, escala).
        /// Nota: El objeto debe existir en el escenario actual al momento de crear la acción.
        public static AccionIndividual CrearAccionTransformacion(
        string nombreObjeto,
        float tiempoInicio,
        Vector3? desplazamiento = null,
        Vector3? rotacionDeltaEuler = null,
        Vector3? escalaFinal = null,
        float duracion = 1f)
        {
            if (!GestorEscenarios.EscenarioActual.Objetos.TryGetValue(nombreObjeto, out var objeto))
                throw new InvalidOperationException($"Objeto '{nombreObjeto}' no encontrado en el escenario.");

            var libreto = new LibretoKeyframes();

            // Keyframe inicial: identidad (no afecta nada)
            libreto.Keyframes.Add(new FotogramaTransformacion(
                0f,
                Vector3.Zero,
                Quaternion.Identity,
                Vector3.One
            ));

            // Keyframe final: define solo los deltas
            libreto.Keyframes.Add(new FotogramaTransformacion(
                duracion,
                desplazamiento ?? Vector3.Zero,  // Solo traslación si corresponde
                rotacionDeltaEuler.HasValue
                    ? Quaternion.FromEulerAngles(rotacionDeltaEuler.Value)
                    : Quaternion.Identity,        // Solo rotación si corresponde
                escalaFinal ?? Vector3.One        // Solo escala si corresponde
            ));

            return new AccionIndividual
            {
                NombreObjeto = nombreObjeto,
                TiempoInicio = tiempoInicio,
                Libreto = libreto
            };
        }
    }
}
