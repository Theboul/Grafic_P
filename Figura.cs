using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL;

namespace OpenTKCubo3D
{
    public class Figura{
        public List<Caras> caras;
        private List<Puntos> verx;
        public Puntos origen;
        public float ancho, alto, profundidad;
        public Color4 color;

        public Figura(Puntos origen,float ancho,float alto, float profundidad , Color4 color){
            
            this.origen = origen;
            this.ancho = ancho;
            this.alto = alto;
            this.profundidad = profundidad;
            this.color = color;

            verx = new List<Puntos>();
            caras = new List<Caras>();

            GenerarCubo();

        }

        private void GenerarCubo(){

          //vertices
            verx.Clear();
            verx.Add(new Puntos(origen.X, origen.Y, origen.Z));
            verx.Add(new Puntos(origen.X + ancho, origen.Y, origen.Z));
            verx.Add(new Puntos(origen.X + ancho, origen.Y + alto, origen.Z));
            verx.Add(new Puntos(origen.X, origen.Y + alto, origen.Z));
            verx.Add(new Puntos(origen.X, origen.Y, origen.Z + profundidad));
            verx.Add(new Puntos(origen.X + ancho, origen.Y, origen.Z + profundidad));
            verx.Add(new Puntos(origen.X + ancho, origen.Y + alto, origen.Z + profundidad));
            verx.Add(new Puntos(origen.X, origen.Y + alto, origen.Z + profundidad));

            //caras
            caras.Clear();
            caras.Add(new Caras(new List<Puntos>{verx[0], verx[1], verx[2], verx[3]}, origen, color));
            caras.Add(new Caras(new List<Puntos>{verx[4], verx[5], verx[6], verx[7]}, origen, color));
            caras.Add(new Caras(new List<Puntos>{verx[0], verx[1], verx[5], verx[4]}, origen, color));
            caras.Add(new Caras(new List<Puntos>{verx[3], verx[2], verx[6], verx[7]}, origen, color));
            caras.Add(new Caras(new List<Puntos>{verx[1], verx[2], verx[6], verx[5]}, origen, color));
            caras.Add(new Caras(new List<Puntos>{verx[0], verx[3], verx[7], verx[4]}, origen, color));
        }
        
        public void Dibujar(){
            foreach (Caras caras in caras){
                caras.Dibujar();
            }
        }
    }
}