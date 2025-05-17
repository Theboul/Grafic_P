using System.Globalization;
using OpenTK.Mathematics;

namespace OpenTKCubo3D
{
    public static class LectorModeloObj
    {
        public static Dictionary<string, ObjetoU> CargarObjetoDesdeObj(string rutaObj, Color4? colorDefault = null)
        {
            var objetos = new Dictionary<string, ObjetoU>();
            var materiales = CargarMateriales(Path.ChangeExtension(rutaObj, ".mtl"));

            var vertices = new List<Vector3>();
            string objetoActual = "default";
            string grupoActual = "Figura_0";
            string materialActual = "default";

            var carasPorObjeto = new Dictionary<string, Dictionary<string, List<CaraTemp>>>();

            foreach (var linea in File.ReadLines(rutaObj))
            {
                var partes = linea.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (partes.Length == 0 || partes[0].StartsWith("#")) continue;

                switch (partes[0])
                {
                    case "v":
                        vertices.Add(ParseVector3(partes));
                        break;
                    case "o":
                        objetoActual = partes.Length > 1 ? partes[1] : $"objeto_{objetos.Count}";
                        carasPorObjeto.TryAdd(objetoActual, new());
                        grupoActual = "Figura_0";
                        break;
                    case "g":
                        grupoActual = partes.Length > 1 ? partes[1] : $"Figura_{Guid.NewGuid()}";
                        break;
                    case "usemtl":
                        materialActual = partes.Length > 1 ? partes[1] : materialActual;
                        break;
                    case "f":
                        carasPorObjeto.TryAdd(objetoActual, new());
                        carasPorObjeto[objetoActual].TryAdd(grupoActual, new());
                        carasPorObjeto[objetoActual][grupoActual].Add(new CaraTemp
                        {
                            IndicesVertices = ParseCaraIndices(partes),
                            MaterialUsado = materialActual
                        });
                        break;
                }
            }

            // Construir estructura final
            foreach (var (nombreObjeto, figuras) in carasPorObjeto)
            {
                var objeto = new ObjetoU();

                foreach (var (nombreFigura, carasTemp) in figuras)
                {
                    var figura = new Figura();
                    var caras = ProcesarCaras(carasTemp, vertices, materiales, colorDefault ?? Color4.White);

                    for (int i = 0; i < caras.Count; i++)
                        figura.Caras[$"cara_{i}"] = caras[i];

                    figura.RecalcularCentroDeMasa();
                    objeto.Partes[nombreFigura] = figura;
                }

                objeto.RecalcularCentroDeMasa();
                objetos[nombreObjeto] = objeto;
            }

            return objetos;
        }

        private static List<Caras> ProcesarCaras(List<CaraTemp> carasTemp, List<Vector3> vertices, Dictionary<string, Material> materiales, Color4 colorDefault)
        {
            var caras = new List<Caras>();

            foreach (var caraTemp in carasTemp)
            {
                if (caraTemp.IndicesVertices.Length >= 3)
                {
                    var v0 = vertices[caraTemp.IndicesVertices[0]];
                    var v1 = vertices[caraTemp.IndicesVertices[1]];
                    var v2 = vertices[caraTemp.IndicesVertices[2]];

                    var normal = Vector3.Cross(v1 - v0, v2 - v0).Normalized();
                    var centroide = (v0 + v1 + v2) / 3f;

                    if (Vector3.Dot(normal, centroide) > 0f)
                        Array.Reverse(caraTemp.IndicesVertices);
                }

                var cara = new Caras();
                for (int i = 0; i < caraTemp.IndicesVertices.Length; i++)
                {
                    var v = vertices[caraTemp.IndicesVertices[i]];
                    cara.Vertices[$"v{i}"] = new Puntos(v.X, v.Y, v.Z);
                }

                cara.Color = materiales.TryGetValue(caraTemp.MaterialUsado, out var mat)
                    ? new Color4(mat.ColorDifuso.X, mat.ColorDifuso.Y, mat.ColorDifuso.Z, 1.0f)
                    : colorDefault;

                cara.RecalcularCentroDeMasa();
                caras.Add(cara);
            }

            return caras;
        }

        private static int[] ParseCaraIndices(string[] partes) =>
            partes.Skip(1).Select(p => int.Parse(p.Split('/')[0]) - 1).ToArray();

        private static Dictionary<string, Material> CargarMateriales(string rutaMtl)
        {
            var materiales = new Dictionary<string, Material>();
            Material? materialActual = null;

            if (!File.Exists(rutaMtl)) return materiales;

            foreach (var linea in File.ReadLines(rutaMtl))
            {
                var partes = linea.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (partes.Length == 0) continue;

                switch (partes[0])
                {
                    case "newmtl":
                        if (partes.Length > 1)
                        {
                            materialActual = new Material { Nombre = partes[1] };
                            materiales[materialActual.Nombre] = materialActual;
                        }
                        break;
                    case "Kd":
                        if (materialActual != null && partes.Length >= 4)
                        {
                            var c = ParseVector3(partes);
                            materialActual.ColorDifuso = new Vector3(
                                Math.Clamp(c.X, 0f, 1f),
                                Math.Clamp(c.Y, 0f, 1f),
                                Math.Clamp(c.Z, 0f, 1f));
                        }
                        break;
                }
            }

            return materiales;
        }

        private static Vector3 ParseVector3(string[] partes)
        {
            return new Vector3(
                float.Parse(partes[1], CultureInfo.InvariantCulture),
                float.Parse(partes[2], CultureInfo.InvariantCulture),
                float.Parse(partes[3], CultureInfo.InvariantCulture));
        }

        private sealed class CaraTemp
        {
            public required int[] IndicesVertices { get; set; }
            public required string MaterialUsado { get; set; }
        }

        private sealed class Material
        {
            public required string Nombre { get; set; }
            public Vector3 ColorDifuso { get; set; } = Vector3.One;
        }
    }
}
