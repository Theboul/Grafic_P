
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
            float totalX = 0, totalY = 0, totalZ = 0;
            int cantPts = 0;
            foreach (var punto in puntos)
            {
                
                totalX += punto.X;
                totalY += punto.Y;
                totalZ += punto.Z;
                cantPts++;
            }

            if (cantPts == 0) return Vector3.Zero; 
            
            float promedioX = totalX / cantPts;
            float promedioY = totalY / cantPts;
            float promedioZ = totalZ / cantPts;

            return new Vector3(promedioX, promedioY, promedioZ);

        }         
    }
}