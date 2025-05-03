using System.Text.Json.Serialization;
using OpenTK.Mathematics;

namespace OpenTKCubo3D
{
    public class Figura{
        public Dictionary<string, Caras> Caras { get; set; } = new Dictionary<string, Caras>();
        public Puntos Origen { get; set; } = new();
        public Color4 Color { get; set; } = Color4.White;

        public Transformaciones Transform { get; } = new Transformaciones();
        [JsonIgnore]
        public Vector3 centroDeMasa { get; set; }
        [JsonPropertyName("centroDeMasa")]
        public Puntos CentroDeMasaSerializable
        {
            get => new Puntos(centroDeMasa.X, centroDeMasa.Y, centroDeMasa.Z);
            set => centroDeMasa = value.ToVector3();
        }
        
        public Figura(){}
        public Figura(Puntos origen, float ancho, float alto, float profundidad, Color4 color)
        {
            Origen = origen;
            Color = color;
            Caras = ModeloU.GenerarCubo(origen, ancho, alto, profundidad, color);
            centroDeMasa = CalcularCentro();
        }

        public void Dibujar(Matrix4 matrizPadre){ 
            Matrix4 matrizLocal = Transform.GetMatrix(centroDeMasa);
            Matrix4 matrizAcumulada = matrizLocal * matrizPadre;
            foreach (Caras caras in Caras.Values){
                caras.Dibujar(matrizAcumulada);
            } 
        }
        
        public Vector3 CalcularCentro()
        {
            var puntos = Caras.Values.SelectMany(c => c.Vertices.Values);
            return Puntos.CalcularCentro(puntos);
        }

         public void RecalcularCentroDeMasa()
        {
            centroDeMasa = CalcularCentro();
            foreach (var cara in Caras.Values)
                cara.RecalcularCentroDeMasa();
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