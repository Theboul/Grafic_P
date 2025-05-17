using OpenTK.Mathematics;

namespace OpenTKCubo3D
{
    /// Representa el estado completo de un objeto (posición, rotación, escala).

    public class EstadoTransformacionInicial
    {
        public Vector3 Posicion { get; set; } = Vector3.Zero;
        public Quaternion Rotacion { get; set; } = Quaternion.Identity;
        public Vector3 Escala { get; set; } = Vector3.One;
    }


    /// Representa una acción que afecta a un objeto específico en el tiempo.
    /// Gestiona su tiempo de ejecución, libreto y aplicación incremental.

    public class AccionIndividual
    {
        public string NombreObjeto { get; set; } = string.Empty;
        public float TiempoInicio { get; set; }
        public EstadoTransformacionInicial EstadoInicial { get; set; } = new();
        public LibretoKeyframes Libreto { get; set; } = new();

        public bool Finalizada { get; private set; } = false;

        public void MarcarFinalizada()
        {
            Finalizada = true;
        }

        public void AplicarInterpolacion(float tiempoLocal)
        {
            if (GestorEscenarios.EscenarioActual.Objetos.TryGetValue(NombreObjeto, out var objeto))
            {
                var frame = Libreto.Interpolar(tiempoLocal);
                objeto.Transform.Position = EstadoInicial.Posicion + frame.Posicion;
                objeto.Transform.Rotation = EstadoInicial.Rotacion * frame.Rotacion;
                objeto.Transform.Scale = EstadoInicial.Escala * frame.Escala;
            }
        }

        public void AplicaUltimoEstado()
        {
            if (Finalizada) return;
            if (GestorEscenarios.EscenarioActual.Objetos.TryGetValue(NombreObjeto, out var objeto))
            {
                var frame = Libreto.Interpolar(Libreto.Duracion);
                objeto.Transform.Position = frame.Posicion;
                objeto.Transform.Rotation = frame.Rotacion;
                objeto.Transform.Scale = frame.Escala;
            }
            Finalizada = true;
        }

        public void ReiniciarEstado()
        {
            if (GestorEscenarios.EscenarioActual.Objetos.TryGetValue(NombreObjeto, out var objeto))
            {
                objeto.Transform.Position = EstadoInicial.Posicion;
                objeto.Transform.Rotation = EstadoInicial.Rotacion;
                objeto.Transform.Scale = EstadoInicial.Escala;
            }
            Finalizada = false;
        }
    }
}
