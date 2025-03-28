using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL;

namespace OpenTKCubo3D
{
    public class Figura{
        private Caras[] caras;
        private Puntos[] verx;
        public Puntos origen;
        public float ancho, alto, profundidad;
        public Color4 color;

        public Figura(Puntos origen,float ancho,float alto, float profundidad , Color4 color){
            
            this.origen = origen;
            this.ancho = ancho;
            this.alto = alto;
            this.profundidad = profundidad;
            this.color = color;

            verx = new Puntos[]{
                
                new (origen.X, origen.Y, origen.Z),
                new (origen.X + ancho, origen.Y, origen.Z),
                new (origen.X + ancho, origen.Y + alto, origen.Z), 
                new (origen.X, origen.Y + alto, origen.Z),
                new (origen.X, origen.Y, origen.Z + profundidad),
                new (origen.X + ancho, origen.Y, origen.Z + profundidad),
                new (origen.X + ancho, origen.Y + alto, origen.Z + profundidad),
                new (origen.X, origen.Y + alto, origen.Z + profundidad)
            };

            caras = new Caras[]{

                new (new Puntos[]{verx[0], verx[1], verx[2], verx[3]}, origen, color),
                new (new Puntos[]{verx[4], verx[5], verx[6], verx[7]}, origen, color),
                new (new Puntos[]{verx[0], verx[1], verx[5], verx[4]}, origen, color),
                new (new Puntos[]{verx[3], verx[2], verx[6], verx[7]}, origen, color),
                new (new Puntos[]{verx[1], verx[2], verx[6], verx[5]}, origen, color),
                new (new Puntos[]{verx[0], verx[3], verx[7], verx[4]}, origen, color)
            };

        }

        
        public void Dibujar(){
            foreach (Caras caras in caras){
                caras.Dibujar();
            }
        }
    }
}