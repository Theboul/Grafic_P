
using OpenTK.Mathematics;
namespace OpenTKCubo3D
{
    public class Puntos
    {
        
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public static Puntos Zero => new Puntos(0, 0, 0);

        public Puntos(){}

        public Puntos(float x, float y, float z)
        {
            X = x; Y = y; Z = z;
        }

        public  Vector3 ToVector3()
        {
            return new Vector3(X, Y, Z);
        }


        public static Vector3 CalcularCentro(IEnumerable<Puntos> puntos)
        {
            float minX = float.MaxValue, maxX = float.MinValue;
            float minY = float.MaxValue, maxY = float.MinValue;
            float minZ = float.MaxValue, maxZ = float.MinValue;

            bool hayPuntos = false;
            foreach (var p in puntos)
            {
                if (p.X < minX) minX = p.X;
                if (p.X > maxX) maxX = p.X;
                if (p.Y < minY) minY = p.Y;
                if (p.Y > maxY) maxY = p.Y;
                if (p.Z < minZ) minZ = p.Z;
                if (p.Z > maxZ) maxZ = p.Z;

                hayPuntos = true;
            }
            if(!hayPuntos) return Vector3.Zero;

            return new Vector3((minX + maxX) / 2, (minY + maxY) / 2, (minZ + maxZ) / 2);

        }         
    }
}