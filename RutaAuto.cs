using OpenTK.Mathematics;
namespace OpenTKCubo3D{
    public static class RutaAuto
    {
        public static List<Vector3> ObtenerRuta()
        {
            return new List<Vector3>
            {
                new (-0.5f, 0.2f, 0f),
                new (-0.5f, 0.2f, 6f),
                new (-0.5f, 0.2f, 11f),
                new (-1f, 0.2f, 14f),
                new (-2f, 0.2f, 16f),
                new (-4f, 0.2f, 18f),
                new (-6f, 0.2f, 19f),
                new (-9f, 0.2f, 19f),
                new (-12f, 0.2f, 18f),
                new (-13.5f, 0.2f, 17f),
                new (-14.5f, 0.2f, 15.5f),
                new (-14.8f, 0.2f, 13.5f),
                new (-15f, 0.2f, 12f),
                new (-15.2f, 0.2f, 9f),
                new (-15.5f, 0.2f, 7f),
                new (-15.7f, 0.2f, 0f)
            };
        }
    }
}