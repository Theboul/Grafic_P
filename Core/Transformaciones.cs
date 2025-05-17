using System.Text.Json.Serialization;
using OpenTK.Mathematics;

namespace OpenTKCubo3D
{
    public class Transformaciones
    {
        [JsonIgnore]
        public Vector3 Position { get; set; } = Vector3.Zero;

        [JsonIgnore]
        public Quaternion Rotation { get; set; } = Quaternion.Identity; // Ahora Quaternion

        [JsonIgnore]
        public Vector3 Scale { get; set; } = Vector3.One;
        [JsonIgnore]
        public Vector3 Pivot { get; set; } = Vector3.Zero;
        [JsonPropertyName("position")]
        public Puntos PositionSerializable
        {
            get => new Puntos(Position.X, Position.Y, Position.Z);
            set => Position = value.ToVector3();
        }

        [JsonPropertyName("rotation")]
        public Puntos RotationSerializable
        {
            get => Rotation.ToEulerAngles().ToPuntos(); // Serializar como Euler
            set => Rotation = Quaternion.FromEulerAngles(value.ToVector3());
        }

        [JsonPropertyName("scale")]
        public Puntos ScaleSerializable
        {
            get => new Puntos(Scale.X, Scale.Y, Scale.Z);
            set => Scale = value.ToVector3();
        }

        public Matrix4 GetMatrix(Vector3 centro)
        {
            return Matrix4.CreateTranslation(-centro) *
                   Matrix4.CreateFromQuaternion(Rotation) *  // Usamos Quaternion directamente
                   Matrix4.CreateScale(Scale) *
                   Matrix4.CreateTranslation(centro + Position);
        }

        public void Transladate(float x, float y, float z)
        {
            Position += new Vector3(x, y, z);
        }

        public void Rotate(float xDeg, float yDeg, float zDeg)
        {
            var deltaQuat = Quaternion.FromEulerAngles(MathHelper.DegreesToRadians(xDeg),
                                                        MathHelper.DegreesToRadians(yDeg),
                                                        MathHelper.DegreesToRadians(zDeg));
            Rotation = Rotation * deltaQuat;
        }

        public void RotateA(Vector3 centro, float xDeg, float yDeg, float zDeg)
        {
            Rotate(xDeg, yDeg, zDeg);
        }

        public void Escalate(float n)
        {
            if (n != 0)
                Scale *= new Vector3(n);
        }
    }

    public static class ExtensionesVector
    {
        public static Puntos ToPuntos(this Vector3 v)
        {
            return new Puntos(v.X, v.Y, v.Z);
        }
    }

}
