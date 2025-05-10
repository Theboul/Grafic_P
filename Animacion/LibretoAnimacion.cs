using System.Text.Json.Serialization;
using OpenTK.Mathematics;

namespace OpenTKCubo3D
{
    public class LibretoAnimacion
    {

         public LibretoAnimacion()
        {
            // Animación: objeto empieza en (0, 0, -2)
            // A los 5s, llega a (0, 0, 3)
            // A los 10s, llega a (0, 0, 8)

            Keyframes.Add(new KeyframeTransformacion(
                0f,
                new Vector3(0f, 0f, -2f),
                Vector3.Zero,
                Vector3.One
            ));

            Keyframes.Add(new KeyframeTransformacion(
                5f,
                new Vector3(0f, 0f, 3f),
                Vector3.Zero,
                Vector3.One
            ));

            Keyframes.Add(new KeyframeTransformacion(
                10f,
                new Vector3(0f, 0f, 8f),
                Vector3.Zero,
                Vector3.One
            ));

            Keyframes.Sort((a, b) => a.Tiempo.CompareTo(b.Tiempo));
        }

        public List<KeyframeTransformacion> Keyframes { get; set; } = new();

        public void AgregarKeyframe(KeyframeTransformacion kf)
        {
            Keyframes.Add(kf);
            Keyframes.Sort((a, b) => a.Tiempo.CompareTo(b.Tiempo));
        }

        public KeyframeTransformacion Interpolar(float tiempo)
        {
            if (Keyframes.Count == 0)
                return new KeyframeTransformacion(0, Vector3.Zero, Vector3.Zero, Vector3.One);

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
                    return new KeyframeTransformacion(tiempo,
                        Vector3.Lerp(a.Posicion, b.Posicion, t),
                        Vector3.Lerp(a.Rotacion, b.Rotacion, t),
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
