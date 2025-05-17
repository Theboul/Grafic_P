using OpenTK.Mathematics;
using System.Text.Json.Serialization;

namespace OpenTKCubo3D
{
    public class LibretoKeyframes
    {
        public List<FotogramaTransformacion> Keyframes { get; set; } = new();

        public FotogramaTransformacion Interpolar(float tiempo)
        {
            if (Keyframes.Count == 0)
                return new FotogramaTransformacion(0, Vector3.Zero, Quaternion.Identity, Vector3.One);

            if (tiempo <= Keyframes[0].Tiempo)
                return Keyframes[0];

            if (tiempo >= Keyframes[^1].Tiempo)
                return Keyframes[^1];

            for (int i = 0; i < Keyframes.Count - 1; i++)
            {
                var a = Keyframes[i];
                var b = Keyframes[i + 1];
                if (tiempo >= a.Tiempo && tiempo <= b.Tiempo)
                {
                    float t = (tiempo - a.Tiempo) / (b.Tiempo - a.Tiempo);
                    return new FotogramaTransformacion(tiempo,
                        Vector3.Lerp(a.Posicion, b.Posicion, t),
                        Quaternion.Slerp(a.Rotacion, b.Rotacion, t),
                        Vector3.Lerp(a.Escala, b.Escala, t)
                    );
                }
            }

            return Keyframes[^1];
        }

        [JsonIgnore]
        public float Duracion => Keyframes.Count == 0 ? 0f : Keyframes[^1].Tiempo;
    }
}
