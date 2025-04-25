using System.Text.Json;
using System.Text.Json.Serialization;
using OpenTK.Mathematics;

namespace OpenTKCubo3D
{
    public class Utilidades(){

        private static readonly JsonSerializerOptions _opcionesJson = new()
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Converters = { new Color4Converter() }
        };

        public static void Guardar<T>(T objeto, string subdirectorio, string nombreArchivo)
        {
            try
            {
                string directorio = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, subdirectorio);
                Directory.CreateDirectory(directorio);
                
                string rutaCompleta = Path.Combine(directorio, $"{nombreArchivo}.json");
                string json = JsonSerializer.Serialize(objeto, _opcionesJson);
                File.WriteAllText(rutaCompleta, json);
                
                Console.WriteLine($"Guardado exitoso en: {rutaCompleta}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar: {ex.Message}");
                throw;
            }
        }

        public static T Cargar<T>(string subdirectorio, string nombreArchivo) where T : new()
        {
            try
            {
                string directorio = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, subdirectorio);
                string ruta = Path.Combine(directorio, $"{nombreArchivo}.json");

                if (!File.Exists(ruta))
                {
                    Console.WriteLine("Archivo no encontrado. Creando nueva instancia por defecto.");
                    return new T();
                }

                string json = File.ReadAllText(ruta);
                return JsonSerializer.Deserialize<T>(json, _opcionesJson) ?? new T();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error durante la carga: {ex.Message}");
                return new T();
            }
        }     
}

    public class Color4Converter : JsonConverter<Color4>
    {
        public override Color4 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
            {
                var array = doc.RootElement.EnumerateArray().ToArray();
                return new Color4(
                    array[0].GetSingle(),
                    array[1].GetSingle(),
                    array[2].GetSingle(),
                    array[3].GetSingle()
                );
            }
        }

        public override void Write(Utf8JsonWriter writer, Color4 value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            writer.WriteNumberValue(value.R);
            writer.WriteNumberValue(value.G);
            writer.WriteNumberValue(value.B);
            writer.WriteNumberValue(value.A);
            writer.WriteEndArray();
        }
        
    }



}