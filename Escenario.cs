using System.Text.Json.Serialization;
using OpenTK.Mathematics;
namespace OpenTKCubo3D
{
    public class Escenario
    {
        public Dictionary<string, ObjetoU> Objetos { get; set; } = new();

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
     
        public Escenario(){}

        public void DibujarTodo(Matrix4 matrizPadre)
        {
            var matrizLocal = Transform.GetMatrix(centroDeMasa);
            Matrix4 matrizAcumulada = matrizLocal * matrizPadre;
            ModeloU.DibujarEjes();
            foreach (var obj in Objetos.Values){
                obj.Dibujar(matrizAcumulada); 
            }
        }

        public Vector3 CalcularCentro()
        {
            var puntos = Objetos.Values
                .SelectMany(obj => obj.Partes.Values
                    .SelectMany(fig => fig.Caras.Values
                        .SelectMany(c => c.Vertices.Values)));
            return Puntos.CalcularCentro(puntos);
        }

        public void RecalcularCentroDeMasa()
        {
            centroDeMasa = CalcularCentro();
            foreach (var obj in Objetos.Values)
                obj.RecalcularCentroDeMasa();
        }


        public void Rotar(float xDeg, float yDeg, float zDeg)
        {
            Transform.RotateAround(centroDeMasa, xDeg, yDeg, zDeg);
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

        public void AgregarObjeto(string id, ObjetoU obj) => Objetos[id] = obj;
        public ObjetoU GetObjeto(string id) => Objetos[id];

    }
}