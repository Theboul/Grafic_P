using ImGuiNET;

namespace OpenTKCubo3D.UI
{
    public static class UIEditor
    {
        private static int _indexEscenario;
        private static int _nivelSeleccionado;
        private static int _indexObjeto;
        private static int _indexParte;
        private static int _indexCara;

        private static string[] _escenarios = System.Array.Empty<string>();
        private static string[] _objetos = System.Array.Empty<string>();
        private static string[] _partes = System.Array.Empty<string>();
        private static string[] _caras = System.Array.Empty<string>();

        public static void Render(PanelTransformaciones panel)
        {
            ImGui.SetNextWindowSize(new System.Numerics.Vector2(300, 500), ImGuiCond.FirstUseEver);
            ImGui.Begin("Panel de Transformaciones", ImGuiWindowFlags.None);

            RenderSeleccionEscenario();
            RenderMostrarEjes();
            RenderCombosSeleccion(panel);
            RenderAccionesRapidas(panel);

            ImGui.End();
        }

        private static void RenderSeleccionEscenario()
        {
            var todosEscenarios = GestorEscenarios.Todos;
            _escenarios = todosEscenarios.Keys.ToArray();

            if (_escenarios.Length > 1)
            {
                if (ImGui.Combo("Escenario", ref _indexEscenario, _escenarios, _escenarios.Length))
                {
                    _indexObjeto = 0;
                    _indexParte = 0;
                    _indexCara = 0;
                    ActualizarListas();
                }
            }
        }

        private static void RenderMostrarEjes()
        {
            if (_escenarios.Length >= 2) return;
            
            var nombreEscenario = _escenarios.ElementAtOrDefault(_indexEscenario);
            if (nombreEscenario != null && GestorEscenarios.Todos.TryGetValue(nombreEscenario, out var escenario))
            {
                bool mostrarEjes = escenario.MostrarEjes;
                if (ImGui.Checkbox("Mostrar Ejes", ref mostrarEjes))
                    escenario.MostrarEjes = mostrarEjes;
            }
        }

        private static void RenderCombosSeleccion(PanelTransformaciones panel)
        {
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

                var nombreEscenario = _escenarios.ElementAtOrDefault(_indexEscenario);
                if (nombreEscenario != null && GestorEscenarios.Todos.TryGetValue(nombreEscenario, out var escenarioSeleccionado))
                {
                    panel.CambiarEscenario(escenarioSeleccionado);
                    panel.Seleccionar(nivel, idObj, idParte, idCara);
                }
            }
        }

        private static void RenderAccionesRapidas(PanelTransformaciones panel)
        {
            if (!ImGui.CollapsingHeader("Acciones Rápidas"))
                return;

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

        private static void ActualizarListas()
        {
            var nombreEscenario = _escenarios.ElementAtOrDefault(_indexEscenario);
            if (nombreEscenario == null) return;

            if (!GestorEscenarios.Todos.TryGetValue(nombreEscenario, out var escenario))
                return;

            _objetos = escenario.Objetos.Keys.ToArray();

            if (_nivelSeleccionado >= 2 && _indexObjeto < _objetos.Length)
            {
                var obj = escenario.GetObjeto(_objetos[_indexObjeto]);
                _partes = obj.Partes.Keys.ToArray();

                if (_nivelSeleccionado == 3 && _indexParte < _partes.Length)
                {
                    var fig = obj.Partes[_partes[_indexParte]];
                    _caras = fig.Caras.Keys.ToArray();
                }
                else
                {
                    _caras = System.Array.Empty<string>();
                }
            }
            else
            {
                _partes = System.Array.Empty<string>();
                _caras = System.Array.Empty<string>();
            }
        }
    }
}
