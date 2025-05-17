using OpenTK.Mathematics;

namespace OpenTKCubo3D
{
    public class EscenaAcciones
    {
        public List<AccionIndividual> AccionesActivas { get; set; } = new();

        public float TiempoGlobal { get; private set; } = 0f;
        public bool Loop { get; set; } = false;

        private Dictionary<AccionIndividual, float> progresoAnterior = new();

        public void Actualizar(float deltaTime)
        {
            TiempoGlobal += deltaTime;
            float duracionTotal = ObtenerDuracionTotal();

            if (TiempoGlobal > duracionTotal)
            {
                if (Loop)
                    TiempoGlobal %= duracionTotal;
                else
                    TiempoGlobal = duracionTotal;
            }

            foreach (var accion in AccionesActivas)
            {
                if (accion.Finalizada) continue;
                if (TiempoGlobal < accion.TiempoInicio) continue;

                if (!GestorEscenarios.EscenarioActual.Objetos.TryGetValue(accion.NombreObjeto, out var objeto))
                    continue;

                float tiempoLocal = TiempoGlobal - accion.TiempoInicio;
                float tActual = Math.Clamp(tiempoLocal / accion.Libreto.Duracion, 0f, 1f);

                if (!progresoAnterior.TryGetValue(accion, out float tAnterior))
                    tAnterior = 0f;

                float deltaT = tActual - tAnterior;

                if (deltaT <= 0f)
                    continue;

                var frameFinal = accion.Libreto.Interpolar(accion.Libreto.Duracion);

                // Aplicar delta de Traslación
                var deltaPos = frameFinal.Posicion * deltaT;
                objeto.Transform.Transladate(deltaPos.X, deltaPos.Y, deltaPos.Z);

                // Aplicar delta de Rotación
                var deltaRot = Quaternion.Slerp(Quaternion.Identity, frameFinal.Rotacion, deltaT);
                objeto.Transform.Rotation = objeto.Transform.Rotation * deltaRot;

                // Aplicar delta de Escala
                var escalaAnterior = Vector3.One + (frameFinal.Escala - Vector3.One) * tAnterior;
                var escalaActual = Vector3.One + (frameFinal.Escala - Vector3.One) * tActual;

                var escalaDelta = new Vector3(
                    escalaActual.X / escalaAnterior.X,
                    escalaActual.Y / escalaAnterior.Y,
                    escalaActual.Z / escalaAnterior.Z
                );
                objeto.Transform.Scale *= escalaDelta;

                // Guardar progreso
                progresoAnterior[accion] = tActual;

                if (tActual >= 1f)
                    accion.MarcarFinalizada();
            }
        }

        private float ObtenerDuracionTotal()
        {
            float max = 0f;
            foreach (var accion in AccionesActivas)
                max = Math.Max(max, accion.TiempoInicio + accion.Libreto.Duracion);

            return max;
        }

        public void Reiniciar()
        {
            TiempoGlobal = 0f;
            progresoAnterior.Clear();

            foreach (var accion in AccionesActivas)
            {
                accion.ReiniciarEstado(); // Esto resetea Finalizada = false
            }

            // Opcional: reiniciar también el Transform a un estado base si vos querés
            // Si querés reiniciar todo al estado base de la escena:
            /*foreach (var obj in GestorEscenarios.EscenarioActual.Objetos.Values)
            {
                obj.Transform.Position = Vector3.Zero;
                obj.Transform.Rotation = Quaternion.Identity;
                obj.Transform.Scale = Vector3.One;
            }*/
        }

    }
}