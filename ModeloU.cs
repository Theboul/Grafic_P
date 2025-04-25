
using OpenTK.Mathematics;

namespace OpenTKCubo3D
{
    public static class ModeloU
    {
        public static Dictionary<string, Caras> GenerarCubo(Puntos origen, float ancho, float alto, float profundidad, Color4 color)
        {
            var caras = new Dictionary<string, Caras>();

            float x = origen.X;
            float y = origen.Y;
            float z = origen.Z;

            float x2 = x + ancho;
            float y2 = y + alto;
            float z2 = z + profundidad;

            caras.Add("frontal", new Caras(new Dictionary<string, Puntos>
            {
                ["CaraF1"] = new Puntos(x,  y,  z),
                ["CaraF2"] = new Puntos(x2, y,  z),
                ["CaraF3"] = new Puntos(x2, y2, z),
                ["CaraF4"] = new Puntos(x,  y2, z),
            }, origen, color));

            caras.Add("trasera", new Caras(new Dictionary<string, Puntos>
            {
                ["CaraT1"] = new Puntos(x2, y2, z2),
                ["CaraT2"] = new Puntos(x2, y,  z2),
                ["CaraT3"] = new Puntos(x,  y,  z2),
                ["CaraT4"] = new Puntos(x,  y2, z2),
            }, origen, color));

            caras.Add("inferior", new Caras(new Dictionary<string, Puntos>
            {
                ["CaraIn1"] = new Puntos(x,  y,  z),
                ["CaraIn2"] = new Puntos(x2, y,  z),
                ["CaraIn3"] = new Puntos(x2, y,  z2),
                ["CaraIn4"] = new Puntos(x,  y,  z2),
            }, origen, color));

            caras.Add("superior", new Caras(new Dictionary<string, Puntos>
            {
                ["CaraS1"] = new Puntos(x,  y2, z),
                ["CaraS2"] = new Puntos(x2, y2, z),
                ["CaraS3"] = new Puntos(x2, y2, z2),
                ["CaraS4"] = new Puntos(x,  y2, z2),
            }, origen, color));

            caras.Add("derecha", new Caras(new Dictionary<string, Puntos>
            {
                ["CaraD1"] = new Puntos(x2, y,  z),
                ["CaraD2"] = new Puntos(x2, y2, z),
                ["CaraD3"] = new Puntos(x2, y2, z2),
                ["CaraD4"] = new Puntos(x2, y,  z2),
            }, origen, color));

            caras.Add("izquierda", new Caras(new Dictionary<string, Puntos>
            {
                ["CaraIz1"] = new Puntos(x, y,  z),
                ["CaraIz2"] = new Puntos(x, y2, z),
                ["CaraIz3"] = new Puntos(x, y2, z2),
                ["CaraIz4"] = new Puntos(x, y,  z2),
            }, origen, color));

            foreach (var cara in caras.Values)
                cara.RecalcularCentroDeMasa();
            return caras;
        }

        public static Dictionary<string, Figura> GenerarPartesU(Puntos centro, float ancho, float alto, float profundidad, Color4 color)
        {
            var partes = new Dictionary<string, Figura>();

            partes.Add("lateral_izq", new Figura(
                new Puntos(centro.X - ancho / 2, centro.Y - alto / 2, centro.Z),
                ancho / 4, alto, profundidad, color
            ));

            partes.Add("lateral_der", new Figura(
                new Puntos(centro.X + ancho / 2 - ancho / 4, centro.Y - alto / 2, centro.Z),
                ancho / 4, alto, profundidad, color
            ));

            partes.Add("base_inf", new Figura(
                new Puntos(centro.X - ancho / 2, centro.Y - alto / 2, centro.Z),
                ancho, alto / 4, profundidad, color
            ));

            return partes;
        }

        public static void DibujarEjes()
        {
            Matrix4 identidad = Matrix4.Identity;
            //Rojo
            new Caras(
                new Dictionary<string, Puntos>
                {
                    ["inicioX"] = new Puntos(-2.0f, 0, 0),
                    ["finX"] = new Puntos(2.0f, 0, 0)
                },
                Puntos.Zero,
                Color4.Red
            ).Dibujar(identidad);

            // Verde
            new Caras(
                new Dictionary<string, Puntos>
                {
                    ["inicioY"] = new Puntos(0, -2.0f, 0),
                    ["finY"] = new Puntos(0, 2.0f, 0)
                },
                Puntos.Zero,
                Color4.Green
            ).Dibujar(identidad);

            // Azul
            new Caras(
                new Dictionary<string, Puntos>
                {
                    ["inicioZ"] = new Puntos(0, 0, -2.0f),
                    ["finZ"] = new Puntos(0, 0, 2.0f)
                },
                Puntos.Zero,
                Color4.Blue
            ).Dibujar(identidad);
        }
        
    }
}

