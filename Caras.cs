
using System.Text.Json.Serialization;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL;

namespace OpenTKCubo3D
{
    public class Caras
    {
        public Dictionary<string, Puntos> Vertices { get; set; } = new Dictionary<string, Puntos>();
        public Puntos? Origen { get; set; } = new();
        public Color4 Color { get; set; } = Color4.White;
        
        [JsonIgnore]
        public Transformaciones Transform { get; } = new Transformaciones();
        [JsonIgnore]
        public Vector3 centroDeMasa { get; set; }
        [JsonPropertyName("centroDeMasa")]
        public Puntos CentroDeMasaSerializable
        {
            get => new Puntos(centroDeMasa.X, centroDeMasa.Y, centroDeMasa.Z);
            set => centroDeMasa = value.ToVector3();
        }

        public Caras(){}
        public Caras(Dictionary<string, Puntos> vertices, Puntos origen, Color4 color)
        {
            Vertices = vertices;
            Origen = origen;
            Color = color;
            centroDeMasa = CalcularCentro();
        }
  
        
        public void Dibujar(Matrix4 matrizAcumulada){

            Matrix4 matrizLocal = Transform.GetMatrix(centroDeMasa);
            Matrix4 matrizFinal = matrizLocal * matrizAcumulada;


            PrimitiveType tipoPrimitiva = Vertices.Count switch
            {
              2 => PrimitiveType.Lines,
              3 => PrimitiveType.Triangles,
              4 => PrimitiveType.Quads,
              _ => PrimitiveType.Polygon
            };

           GL.Begin(tipoPrimitiva);
           GL.Color4(Color);
           foreach (var vert in Vertices)
           {
            Vector3 transformado = Vector3.TransformPosition(vert.Value.ToVector3(), matrizFinal);
            GL.Vertex3(transformado);
           }
           GL.End();

        }

        public Vector3 CalcularCentro() => Puntos.CalcularCentro(Vertices.Values);


        public void Rotar(float xDeg, float yDeg, float zDeg)
        {

            Transform.RotateAround(centroDeMasa, xDeg, yDeg, zDeg);
        }

        public void Escalar(float factor)
        {
            Transform.Position -= centroDeMasa;
            Transform.Escalate(factor);
            Transform.Position += centroDeMasa;
        }

        public void Transladar(float dx, float dy, float dz)
        {
            Transform.Transladate(dx, dy, dz);
        }

        
        public void RecalcularCentroDeMasa()
        {
            centroDeMasa = CalcularCentro();
        }
    };
}