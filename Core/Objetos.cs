using System.Text.Json.Serialization;
using OpenTK.Mathematics;
namespace OpenTKCubo3D
{
    public class ObjetoU
    {
        public Dictionary<string, Figura> Partes { get; set; } = new Dictionary<string, Figura>();
        public Puntos Centro { get; set; } = new();
        public Color4 Color { get; set; } = Color4.White;

        public Transformaciones Transform { get; set; } = new Transformaciones();
        [JsonIgnore]
        public Vector3 centroDeMasa { get; set; }
        [JsonPropertyName("centroDeMasa")]
        public Puntos CentroDeMasaSerializable
        {
            get => new Puntos(centroDeMasa.X, centroDeMasa.Y, centroDeMasa.Z);
            set => centroDeMasa = value.ToVector3();
        }
        public ObjetoU(){}
        public ObjetoU(Puntos centro, float ancho, float alto, float profundidad, Color4 color)
        {
            Centro = centro;
            Color = color;
            Partes = ModeloU.GenerarPartesU(centro,ancho,alto,profundidad,color);
            centroDeMasa = CalcularCentro();
        }
        
        public void Dibujar(Matrix4 matrizPadre)
        {
            Matrix4 matrizLocal = Transform.GetMatrix(centroDeMasa);
            Matrix4 matrizAcumulada = matrizLocal * matrizPadre;
            foreach (var figura in Partes.Values){
                figura.Dibujar(matrizAcumulada);
            }
        }

        public Vector3 CalcularCentro()
        {
            var puntos = Partes.Values
            .SelectMany(fig => fig.Caras.Values
                .SelectMany(c => c.Vertices.Values));
                    return Puntos.CalcularCentro(puntos);
        }


        public void RecalcularCentroDeMasa()
        {
            centroDeMasa = CalcularCentro();
            foreach (var parte in Partes.Values)
                parte.RecalcularCentroDeMasa();
        }


        public void Rotar(float xDeg, float yDeg, float zDeg)
        {
            Transform.RotateA(centroDeMasa, xDeg, yDeg, zDeg);
        }

        public void Escalar(float f)
        {
            Transform.Position -= centroDeMasa;
            Transform.Escalate(f);
            Transform.Position += centroDeMasa;
        }

        public void Transladar(float dx, float dy, float dz)
        {
            Transform.Transladate(dx, dy, dz);
        }

    }
}