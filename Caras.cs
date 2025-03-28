
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL;

namespace OpenTKCubo3D
{
    public class Caras
    {
        private Puntos[] vertices;
        private Puntos origen;
        private Color4 color;


        public Caras(Puntos[] vertices, Puntos origen, Color4 color)
        {
             if (vertices.Length != 4)
                throw new ArgumentException("Deben proporcionarse exactamente 4 vértices.");

            this.vertices = vertices;
            this.origen = origen;
            this.color = color;
        }

        public Puntos Origen
        {
            get { return origen;}
            set { origen = value;}
        }

        /*public void Dibujar(){
            GL.Begin(PrimitiveType.Quads);
            GL.Color4(color);
            GL.Vertex3(vertices[0].X, vertices[0].Y, vertices[0].Z);
            GL.Vertex3(vertices[1].X, vertices[1].Y, vertices[1].Z);
            GL.Vertex3(vertices[2].X, vertices[2].Y, vertices[2].Z);
            GL.Vertex3(vertices[3].X, vertices[3].Y, vertices[3].Z);
            GL.End();
        }*/

        public void Dibujar(){
           GL.Begin(PrimitiveType.Quads);
           GL.Color4(color);
           foreach (var vertice in vertices)
           {
            GL.Vertex3(vertice.X, vertice.Y, vertice.Z);
           }
           GL.End();
        }
    };


}