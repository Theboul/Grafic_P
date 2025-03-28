using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL;
namespace OpenTKCubo3D
{
    public class ObjetoU
    {
        private List<Figura> partes; 
        private Puntos centro;
        private float anchoTotal;
        private float altoTotal;
        private float profundidad;
        private Color4 color;

        public float _x, _y, _z;
        public float _angleX, _angleY;

        public ObjetoU(Puntos centro, float anchoTotal, float altoTotal, float profundidad, Color4 color)
        {
            this.centro = centro;
            this.anchoTotal = anchoTotal;
            this.altoTotal = altoTotal;
            this.profundidad = profundidad;
            this.color = color;
            
            _x = centro.X;
            _y = centro.Y;
            _z = centro.Z;
            _angleX = 0f;
            _angleY = 0f;

            this.centro = new Puntos(_x, _y, _z);
            partes = new List<Figura>();
            CalcularPartes();
        }
        

        public float PosX{ get => _x; set { _x = value; ActualizarCentro(); } }
        public float PosY{ get => _y; set { _y = value; ActualizarCentro(); }}
        public float PosZ{ get => _z; set { _z = value; ActualizarCentro(); } }
        public float RotX{ get => _angleX; set => _angleX = value;}
        public float RotY{ get => _angleY; set => _angleY = value;}

        private void ActualizarCentro()
        {
            centro = new Puntos(_x, _y, _z);
            CalcularPartes();
        }

        private void CalcularPartes()
        {
            partes.Clear();
            // Dimensiones de las partes
            float anchoLateral = anchoTotal / 4f;
            float altoLateral = altoTotal;
            float anchoBase = anchoTotal;
            float altoBase = anchoLateral;

            // Posiciones (ajustadas desde el centro)
            float xIzquierdo = _x - anchoTotal / 2f;
            float xDerecho = _x + anchoTotal / 2f - anchoLateral;
            float yBase = _y - altoTotal / 2f;
            
            // Lateral izquierdo
            partes.Add(new Figura(new Puntos(xIzquierdo, yBase, _z - profundidad/2f),anchoLateral,altoLateral,profundidad,color));
            // Lateral derecho
            partes.Add(new Figura(new Puntos(xDerecho, yBase, _z - profundidad/2f),anchoLateral,altoLateral,profundidad,color));
            // Base inferior
            partes.Add(new Figura(new Puntos(xIzquierdo, yBase, _z - profundidad/2f),anchoBase,altoBase,profundidad,color));

        }

        public void Dibujar()
        {
           GL.PushMatrix();
           GL.Translate(_x, _y, _z); // Aplicar traslación global
           GL.Rotate(_angleX, 1.0f, 0.0f, 0.0f); // Rotación X
           GL.Rotate(_angleY, 0.0f, 1.0f, 0.0f); // Rotación Y
            foreach (var parte in partes)
            {
                parte.Dibujar();
            }

            GL.PopMatrix();
        }
            

        

            public void Mover(float dx, float dy, float dz)
            {
               PosX += dx;
               PosY += dy;
               PosZ += dz;
            }

            public void Rotar(float deltaX, float deltaY)
            {
               RotX += deltaX*10;
               RotY += deltaY*10;
            }
        /*public void Mover(Puntos nuevoCentro)
        {
            centro = nuevoCentro;
            CalcularPartes(); // Recalcula las posiciones
        }*/
    }
}