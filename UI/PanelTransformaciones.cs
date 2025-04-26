namespace OpenTKCubo3D.UI
{
    public class PanelTransformaciones
    {
        public enum Nivel { Escenario, Objeto, Figura, Cara }

        private Escenario _escenario;
        private Nivel _nivelActual = Nivel.Escenario;
        private string? _idObjeto, _idParte, _idCara;

        public PanelTransformaciones(Escenario escenario)
        {
            _escenario = escenario;
        }

        public void Seleccionar(Nivel nivel, string? idObjeto = null, string? idParte = null, string? idCara = null)
        {
            _nivelActual = nivel;
            _idObjeto = idObjeto;
            _idParte = idParte;
            _idCara = idCara;
        }

        public void Rotar(float xDeg, float yDeg, float zDeg) => AplicarAccion(n => n.Rotar(xDeg, yDeg, zDeg));

        public void Escalar(float factor) => AplicarAccion(n => n.Escalar(factor));

        public void Transladar(float dx, float dy, float dz) => AplicarAccion(n => n.Transladar(dx, dy, dz));

        private void AplicarAccion(Action<dynamic> accion)
        {
            dynamic target = _nivelActual switch
            {
                Nivel.Escenario => _escenario,
                Nivel.Objeto => _escenario.GetObjeto(_idObjeto!),
                Nivel.Figura => _escenario.GetObjeto(_idObjeto!).Partes[_idParte!],
                Nivel.Cara => _escenario.GetObjeto(_idObjeto!).Partes[_idParte!].Caras[_idCara!],
                _ => _escenario
            };
            accion(target);
        }

        public void CambiarEscenario(Escenario nuevoEscenario)
        {
            _escenario = nuevoEscenario;
        }
    }
}
