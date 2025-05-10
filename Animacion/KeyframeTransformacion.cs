using OpenTK.Mathematics;
using System.Text.Json.Serialization;

namespace OpenTKCubo3D
{
    public class KeyframeTransformacion
    {
        public float Tiempo { get; set; }

        [JsonIgnore]
        public Vector3 Posicion { get; set; }

        [JsonIgnore]
        public Vector3 Rotacion { get; set; }

        [JsonIgnore]
        public Vector3 Escala { get; set; }

        // Serialización para JSON
        [JsonPropertyName("posicion")]
        public Puntos PosicionSerializable
        {
            get => new Puntos(Posicion.X, Posicion.Y, Posicion.Z);
            set => Posicion = value.ToVector3();
        }

        [JsonPropertyName("rotacion")]
        public Puntos RotacionSerializable
        {
            get => new Puntos(Rotacion.X, Rotacion.Y, Rotacion.Z);
            set => Rotacion = value.ToVector3();
        }

        [JsonPropertyName("escala")]
        public Puntos EscalaSerializable
        {
            get => new Puntos(Escala.X, Escala.Y, Escala.Z);
            set => Escala = value.ToVector3();
        }

        public KeyframeTransformacion() { }

        public KeyframeTransformacion(float tiempo, Vector3 posicion, Vector3 rotacion, Vector3 escala)
        {
            Tiempo = tiempo;
            Posicion = posicion;
            Rotacion = rotacion;
            Escala = escala;
        }
    }
}
