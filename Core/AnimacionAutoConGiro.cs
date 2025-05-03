using OpenTK.Mathematics;

namespace OpenTKCubo3D
{
    public class AnimacionAutoConGiro
    {
        private ObjetoU _auto;
        private List<Vector3> _puntosRutaSuavizados;
        private int _indiceSpline = 0;
        private float _t = 0f;
        private float _velocidad;
        private float _radioRueda = 0.5f;

        private List<Figura> _ruedasDelanteras;
        private List<Figura> _ruedasTraseras;

        public bool Activa { get; set; } = true;

        public AnimacionAutoConGiro(ObjetoU auto, List<Vector3> puntosBase, float velocidad = 4f)
        {
            _auto = auto;
            _velocidad = velocidad;
            _puntosRutaSuavizados = SplineGenerator.GenerarRutaSuavizada(puntosBase, 12);

            _ruedasDelanteras = auto.Partes
                .Where(kv => kv.Key.ToLower().Contains("ruedadd") || kv.Key.ToLower().Contains("ruedadi"))
                .Select(kv => kv.Value)
                .ToList();

            _ruedasTraseras = auto.Partes
                .Where(kv => kv.Key.ToLower().Contains("ruedatd") || kv.Key.ToLower().Contains("ruedati"))
                .Select(kv => kv.Value)
                .ToList();
        }

        public void Actualizar(float deltaTime)
        {
            if (!Activa || _puntosRutaSuavizados.Count < 4 || _indiceSpline + 3 >= _puntosRutaSuavizados.Count)
            {
                Activa = false;
                return;
            }

            // Obtener puntos spline actuales
            Vector3 p0 = _puntosRutaSuavizados[_indiceSpline];
            Vector3 p1 = _puntosRutaSuavizados[_indiceSpline + 1];
            Vector3 p2 = _puntosRutaSuavizados[_indiceSpline + 2];
            Vector3 p3 = _puntosRutaSuavizados[_indiceSpline + 3];

            Vector3 posicionActual = SplineGenerator.CatmullRom(p0, p1, p2, p3, _t);

            // Avanzar proporcionalmente según la distancia entre p1 y p2
            float distanciaTramo = Vector3.Distance(p1, p2);
            float avance = _velocidad * deltaTime;
            _t += avance / distanciaTramo;

            while (_t >= 1f)
            {
                _t -= 1f;
                _indiceSpline++;
                if (_indiceSpline + 3 >= _puntosRutaSuavizados.Count)
                {
                    Activa = false;
                    return;
                }

                // Actualizar puntos al cambiar de tramo
                p0 = _puntosRutaSuavizados[_indiceSpline];
                p1 = _puntosRutaSuavizados[_indiceSpline + 1];
                p2 = _puntosRutaSuavizados[_indiceSpline + 2];
                p3 = _puntosRutaSuavizados[_indiceSpline + 3];
                distanciaTramo = Vector3.Distance(p1, p2);
            }

            // Calcular nueva posición y dirección
            Vector3 nuevaPos = SplineGenerator.CatmullRom(p0, p1, p2, p3, _t);
            Vector3 direccion = nuevaPos - posicionActual;
            float distanciaReal = direccion.Length;
            // Detener avance si el desplazamiento es demasiado pequeño
            if (distanciaReal < 1e-4f)
                return;
            
            direccion = direccion.Normalized();

            // Mover auto
            _auto.Transform.Position = nuevaPos;

            // Orientación del chasis
            if (direccion != Vector3.Zero)
            {
                float anguloObjetivo = MathF.Atan2(direccion.X, direccion.Z);
                float anguloActual = MathHelper.DegreesToRadians(_auto.Transform.Rotation.Y);
                float anguloSuavizado = MathHelper.Lerp(anguloActual, anguloObjetivo, deltaTime * 5f);
                _auto.Transform.Rotation = new Vector3(0, MathHelper.RadiansToDegrees(anguloSuavizado), 0);

                float giroY = MathHelper.Clamp(direccion.X * 25f, -25f, 25f);
                foreach (var rueda in _ruedasDelanteras)
                {
                    float rotX = rueda.Transform.Rotation.X;
                    rueda.Transform.Rotation = new Vector3(rotX, giroY, 0);
                }
            }

            // Rotar ruedas en X según distancia recorrida
            float rotacionRueda = (distanciaReal / (_radioRueda * MathF.Tau)) * 360f;
            foreach (var rueda in _ruedasDelanteras.Concat(_ruedasTraseras))
            {
                rueda.Transform.Rotate(rotacionRueda, 0, 0);
            }
        }
    }
}
