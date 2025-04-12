
namespace OpenTKCubo3D
{
    public class Puntos
    {
        public float[] puntosP = new float[3];
        public static Puntos Zero => new Puntos(0, 0, 0);
        public Puntos(float x, float y, float z)
        {
            puntosP[0] = x;
            puntosP[1] = y;
            puntosP[2] = z;
        }

        // Propiedades para acceder a los elementos del vector puntosP 
        public float X
        {
            get { return puntosP[0]; }
            set { puntosP[0] = value; }
        }

        public float Y
        {
            get { return puntosP[1]; }
            set { puntosP[1] = value; }
        }

        public float Z
        {
            get { return puntosP[2]; }
            set { puntosP[2] = value; }
        }
    }
}