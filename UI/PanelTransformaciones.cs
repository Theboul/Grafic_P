
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

        public void Rotar(float xDeg, float yDeg, float zDeg)
        {
            switch (_nivelActual)
            {
                case Nivel.Escenario:
                    _escenario.Rotar(xDeg, yDeg, zDeg);
                    break;
                case Nivel.Objeto:
                    _escenario.GetObjeto(_idObjeto!).Rotar(xDeg, yDeg, zDeg);
                    break;
                case Nivel.Figura:
                    _escenario.GetObjeto(_idObjeto!).Partes[_idParte!].Rotar(xDeg, yDeg, zDeg);
                    break;
                case Nivel.Cara:
                    _escenario.GetObjeto(_idObjeto!).Partes[_idParte!].Caras[_idCara!].Rotar(xDeg, yDeg, zDeg);
                    break;
            }
        }

        public void Escalar(float factor)
        {
            switch (_nivelActual)
            {
                case Nivel.Escenario:
                    _escenario.Escalar(factor);
                    break;
                case Nivel.Objeto:
                    _escenario.GetObjeto(_idObjeto!).Escalar(factor);
                    break;
                case Nivel.Figura:
                    _escenario.GetObjeto(_idObjeto!).Partes[_idParte!].Escalar(factor);
                    break;
                case Nivel.Cara:
                    _escenario.GetObjeto(_idObjeto!).Partes[_idParte!].Caras[_idCara!].Escalar(factor);
                    break;
            }
        }

        public void Transladar(float dx, float dy, float dz)
        {
            switch (_nivelActual)
            {
                case Nivel.Escenario:
                    _escenario.Transladar(dx, dy, dz);
                    break;
                case Nivel.Objeto:
                    _escenario.GetObjeto(_idObjeto!).Transladar(dx, dy, dz);
                    break;
                case Nivel.Figura:
                    _escenario.GetObjeto(_idObjeto!).Partes[_idParte!].Transladar(dx, dy, dz);
                    break;
                case Nivel.Cara:
                    _escenario.GetObjeto(_idObjeto!).Partes[_idParte!].Caras[_idCara!].Transladar(dx, dy, dz);
                    break;
            }
        }
    }
}
