
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL;

namespace OpenTKCubo3D
{
    public class Caras
    {
        private List<Puntos> Vertices;
        private Puntos origen;
        private Color4 color;

        public Caras(List<Puntos> vertices, Puntos origen, Color4 color)
        {
             if (vertices == null || vertices.Count < 2)
                throw new ArgumentException("Deben proporcionarse al menos 2 vértices.");

            Vertices = vertices;
            this.origen = origen;
            this.color = color;
        }

        public Puntos Origen
        {
            get { return origen;}
            set { origen = value;}
        }


        public void Dibujar(){

            PrimitiveType tipoPrimitiva = Vertices.Count switch
            {
              2 => PrimitiveType.Lines,
              3 => PrimitiveType.Triangles,
              4 => PrimitiveType.Quads,
              _ => PrimitiveType.Polygon
            };

           GL.Begin(tipoPrimitiva);
           GL.Color4(color);
           foreach (var vert in Vertices)
           {
            GL.Vertex3(vert.X, vert.Y, vert.Z);
           }
           GL.End();
        }


        public static void DibujarEjes()
        {
         new Caras(new List<Puntos>{new (-2.0f, 0, 0), new (2.0f, 0, 0)}, Puntos.Zero, Color4.Red).Dibujar();
         new Caras(new List<Puntos>{new (0, -2.0f, 0), new (0, 2.0f, 0)}, Puntos.Zero, Color4.Green).Dibujar();
         new Caras(new List<Puntos>{new (0, 0, -2.0f), new (0, 0, 2.0f)}, Puntos.Zero, Color4.Blue).Dibujar();
        
        }
    };


}