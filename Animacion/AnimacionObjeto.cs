namespace OpenTKCubo3D
{
    public class AnimacionObjeto
    {
        public ObjetoU Objeto;
        public LibretoAnimacion Libreto;
        public float TiempoActual = 0f;
        public bool Loop;

        public AnimacionObjeto(ObjetoU objeto, LibretoAnimacion libreto, bool loop = false)
        {
            Objeto = objeto;
            Libreto = libreto;
            Loop = loop;
        }

        public void Actualizar(float deltaTime)
        {
            TiempoActual += deltaTime;

            float duracion = Libreto.Duracion;
            if (TiempoActual > duracion)
            {
                if (Loop)
                    TiempoActual %= duracion;
                else
                    TiempoActual = duracion;
            }

            var frame = Libreto.Interpolar(TiempoActual);

            Objeto.Transform.Position = frame.Posicion;
            Objeto.Transform.Rotation = frame.Rotacion;
            Objeto.Transform.Scale = frame.Escala;
        }
    }
}
