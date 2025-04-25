using ImGuiNET;
using System.Collections.Generic;
using System.Linq;

namespace OpenTKCubo3D.UI
{
    public static class UIEditor
    {
        private static int _nivelSeleccionado = 0;
        private static int _indexObjeto = 0;
        private static int _indexParte = 0;
        private static int _indexCara = 0;

        private static string[] _objetos = new string[0];
        private static string[] _partes = new string[0];
        private static string[] _caras = new string[0];

        public static void Render(PanelTransformaciones panel)
        {
            ImGui.SetNextWindowSize(new System.Numerics.Vector2(300, 400), ImGuiCond.FirstUseEver);
            ImGui.Begin("Panel de Transformaciones", ImGuiWindowFlags.None);

            string[] niveles = { "Escenario", "Objeto", "Figura", "Cara" };
            if (ImGui.Combo("Nivel", ref _nivelSeleccionado, niveles, niveles.Length))
            {
                _indexObjeto = 0;
                _indexParte = 0;
                _indexCara = 0;
                ActualizarListas();
            }

            if (_nivelSeleccionado >= 1 && _objetos.Length > 0)
            {
                if (ImGui.Combo("Objeto", ref _indexObjeto, _objetos, _objetos.Length))
                {
                    _indexParte = 0;
                    _indexCara = 0;
                    ActualizarListas();
                }
            }
            if (_nivelSeleccionado >= 2 && _partes.Length > 0)
            {
                if (ImGui.Combo("Parte", ref _indexParte, _partes, _partes.Length))
                {
                    _indexCara = 0;
                    ActualizarListas();
                }
            }
            if (_nivelSeleccionado == 3 && _caras.Length > 0)
            {
                ImGui.Combo("Cara", ref _indexCara, _caras, _caras.Length);
            }

            if (ImGui.Button("Aplicar Selección"))
            {
                var nivel = (PanelTransformaciones.Nivel)_nivelSeleccionado;
                string? idObj = _objetos.ElementAtOrDefault(_indexObjeto);
                string? idParte = _partes.ElementAtOrDefault(_indexParte);
                string? idCara = _caras.ElementAtOrDefault(_indexCara);

                panel.Seleccionar(nivel, idObj, idParte, idCara);
            }

            if (ImGui.CollapsingHeader("Acciones Rápidas"))
            {
                if (ImGui.Button("Rotar +X")) panel.Rotar(5f, 0f, 0f);
                if (ImGui.Button("Rotar +Y")) panel.Rotar(0f, 5f, 0f);
                if (ImGui.Button("Rotar +Z")) panel.Rotar(0f, 0f, 5f);

                ImGui.Separator();

                if (ImGui.Button("Escalar x1.1")) panel.Escalar(1.1f);
                if (ImGui.Button("Reducir x0.9")) panel.Escalar(0.9f);

                ImGui.Separator();

                if (ImGui.Button("Mover +X")) panel.Transladar(0.5f, 0f, 0f);
                if (ImGui.Button("Mover -X")) panel.Transladar(-0.5f, 0f, 0f);
                if (ImGui.Button("Mover +Y")) panel.Transladar(0f, 0.5f, 0f);
                if (ImGui.Button("Mover -Y")) panel.Transladar(0f, -0.5f, 0f);
                if (ImGui.Button("Mover +Z")) panel.Transladar(0f, 0f, 0.5f);
                if (ImGui.Button("Mover -Z")) panel.Transladar(0f, 0f, -0.5f);
            }

            ImGui.End();
        }

        private static void ActualizarListas()
        {
            var escenario = GestorEscenarios.EscenarioActual;
            _objetos = escenario.Objetos.Keys.ToArray();

            if (_nivelSeleccionado >= 2 && _indexObjeto < _objetos.Length)
            {
                var obj = escenario.GetObjeto(_objetos[_indexObjeto]);
                _partes = obj.Partes.Keys.ToArray();
            }
            else
            {
                _partes = new string[0];
            }

            if (_nivelSeleccionado == 3 && _indexObjeto < _objetos.Length && _indexParte < _partes.Length)
            {
                var obj = escenario.GetObjeto(_objetos[_indexObjeto]);
                var fig = obj.Partes[_partes[_indexParte]];
                _caras = fig.Caras.Keys.ToArray();
            }
            else
            {
                _caras = new string[0];
            }
        }
    }
}
