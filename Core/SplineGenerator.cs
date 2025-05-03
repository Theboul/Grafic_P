using OpenTK.Mathematics;

namespace OpenTKCubo3D
{
    public static class SplineGenerator
    {
        public static List<Vector3> GenerarRutaSuavizada(List<Vector3> puntosBase, int pasosPorTramo = 15)
        {
            var rutaSuavizada = new List<Vector3>();
            if (puntosBase.Count < 2) return puntosBase;
            if (puntosBase.Count < 4) return InterpolarLineal(puntosBase, pasosPorTramo);

            // Primer segmento
            Vector3 p0 = puntosBase[0];
            Vector3 p1 = puntosBase[0];
            Vector3 p2 = puntosBase[1];
            Vector3 p3 = puntosBase[2];
            AgregarSegmento(p0, p1, p2, p3, pasosPorTramo, rutaSuavizada);

            // Segmentos intermedios
            for (int i = 0; i < puntosBase.Count - 3; i++)
            {
                p0 = puntosBase[i];
                p1 = puntosBase[i + 1];
                p2 = puntosBase[i + 2];
                p3 = puntosBase[i + 3];
                AgregarSegmento(p0, p1, p2, p3, pasosPorTramo, rutaSuavizada);
            }

            // Último segmento
            p0 = puntosBase[^3];
            p1 = puntosBase[^2];
            p2 = puntosBase[^1];
            p3 = puntosBase[^1];
            AgregarSegmento(p0, p1, p2, p3, pasosPorTramo, rutaSuavizada);

            return FiltrarPuntosCercanos(rutaSuavizada, 0.02f); // ajusta tolerancia si es necesario
        }

        private static void AgregarSegmento(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, int pasos, List<Vector3> ruta)
        {
            for (int i = 0; i <= pasos; i++)
            {
                float t = i / (float)pasos;
                ruta.Add(CatmullRom(p0, p1, p2, p3, t));
            }
        }

        private static List<Vector3> InterpolarLineal(List<Vector3> puntos, int pasosPorSegmento)
        {
            var resultado = new List<Vector3>();
            for (int i = 0; i < puntos.Count - 1; i++)
            {
                for (int j = 0; j <= pasosPorSegmento; j++)
                {
                    float t = j / (float)pasosPorSegmento;
                    resultado.Add(Vector3.Lerp(puntos[i], puntos[i + 1], t));
                }
            }
            return resultado;
        }

        public static Vector3 CatmullRom(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
        {
            float t2 = t * t;
            float t3 = t2 * t;

            return 0.5f * (
                (2 * p1) +
                (-p0 + p2) * t +
                (2 * p0 - 5 * p1 + 4 * p2 - p3) * t2 +
                (-p0 + 3 * p1 - 3 * p2 + p3) * t3
            );
        }


        private static List<Vector3> FiltrarPuntosCercanos(List<Vector3> puntos, float tolerancia)
        {
            var resultado = new List<Vector3>();
            if (puntos.Count == 0) return resultado;

            resultado.Add(puntos[0]);
            for (int i = 1; i < puntos.Count; i++)
            {
                if (Vector3.Distance(resultado[^1], puntos[i]) >= tolerancia)
                {
                    resultado.Add(puntos[i]);
                }
            }
            return resultado;
        }

    }
}