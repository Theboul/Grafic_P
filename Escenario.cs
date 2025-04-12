using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using System.Text.Json;
using System.Drawing;

namespace OpenTKCubo3D
{
    public class Escenario
    {
        private List<ObjetoU> objetos;
    
        public Escenario()
        {
            objetos = new List<ObjetoU>();
        }

        public void IniEscenario(int cantidadCopias, Color4 color)
        {
            float areaDistribucion = 2.0f;
            var random = new Random();
            for (int i = 0; i < cantidadCopias; i++)
            {
                float x = (float)(random.NextDouble() * areaDistribucion - areaDistribucion / 2);
                float y = (float)(random.NextDouble() * areaDistribucion - areaDistribucion / 2);
                float z = (float)(random.NextDouble() * areaDistribucion - areaDistribucion / 2);

                objetos.Add(new ObjetoU(new Puntos(x, y, z), 1.5f, 1.5f, 0.3f, color));
            }
       }

        public void DibujarTodo()
        {

            Caras.DibujarEjes();

            foreach (var obj in objetos)
            {
                obj.Dibujar(); 
            }
        }

        public void Guardar(string rutaArchivo)
        {
            var datos = new
            {
                Objetos = objetos.Select(objetos => new
                {
                    Centro = new { objetos.centro.X, objetos.centro.Y, objetos.centro.Z },
                    objetos.anchoTotal,
                    objetos.altoTotal,
                    objetos.profundidad,
                    Color = new[] { objetos.color.R, objetos.color.G, objetos.color.B, objetos.color.A }
                }).ToList(),
            };

            File.WriteAllText(rutaArchivo, 
                JsonSerializer.Serialize(datos, new JsonSerializerOptions { WriteIndented = true }));
       }

        public void Cargar(string rutaArchivo)
        {
            if (!File.Exists(rutaArchivo))
            {
                throw new FileNotFoundException("El archivo no existe.", rutaArchivo);
            }

            var json = File.ReadAllText(rutaArchivo);
            var datos = JsonSerializer.Deserialize<JsonElement>(json);

            objetos.Clear();
            foreach (var obj in datos.GetProperty("Objetos").EnumerateArray())
            {
                var colorArray = obj.GetProperty("Color").EnumerateArray().ToArray();
                objetos.Add(new ObjetoU(
                new Puntos(
                obj.GetProperty("Centro").GetProperty("X").GetSingle(),
                obj.GetProperty("Centro").GetProperty("Y").GetSingle(),
                obj.GetProperty("Centro").GetProperty("Z").GetSingle()
                ),
                obj.GetProperty("anchoTotal").GetSingle(),  // ¡Ahora en minúsculas!
                obj.GetProperty("altoTotal").GetSingle(),
                obj.GetProperty("profundidad").GetSingle(),
                new Color4(
                colorArray[0].GetSingle(),  // R
                colorArray[1].GetSingle(),  // G
                colorArray[2].GetSingle(),  // B
                colorArray[3].GetSingle()   // A
                )
                ));
            }
        }


    }
}