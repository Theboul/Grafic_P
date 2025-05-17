using OpenTK.Mathematics;
using System.Text.Json.Serialization;

namespace OpenTKCubo3D
{
    public class FotogramaTransformacion
    {
        public float Tiempo { get; set; }

        [JsonIgnore]
        public Vector3 Posicion { get; set; }

        [JsonIgnore]
        public Quaternion Rotacion { get; set; } = Quaternion.Identity;

        [JsonIgnore]
        public Vector3 Escala { get; set; }

        [JsonPropertyName("posicion")]
        public Puntos PosicionSerializable
        {
            get => new Puntos(Posicion.X, Posicion.Y, Posicion.Z);
            set => Posicion = value.ToVector3();
        }

        [JsonPropertyName("rotacion")]
        public Puntos RotacionEulerSerializable
        {
            get => Rotacion.ToEulerAngles().ToPuntos();
            set => Rotacion = Quaternion.FromEulerAngles(value.ToVector3());
        }

        [JsonPropertyName("escala")]
        public Puntos EscalaSerializable
        {
            get => new Puntos(Escala.X, Escala.Y, Escala.Z);
            set => Escala = value.ToVector3();
        }

        public FotogramaTransformacion() { }

        public FotogramaTransformacion(float tiempo, Vector3 posicion, Quaternion rotacion, Vector3 escala)
        {
            Tiempo = tiempo;
            Posicion = posicion;
            Rotacion = rotacion;
            Escala = escala;
        }
    }

}
