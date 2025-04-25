using OpenTK.Mathematics;

namespace OpenTKCubo3D
{
    public class Transformaciones(){
        public Vector3 Position { get; set; } = Vector3.Zero;
        public Vector3 Rotation { get; set; } = Vector3.Zero; 
        public Vector3 Scale { get; set; } = Vector3.One;


        public Matrix4 GetMatrix(Vector3 centro) => Matrix4.CreateTranslation(-centro) *
        Matrix4.CreateRotationX(MathHelper.DegreesToRadians(Rotation.X)) *
        Matrix4.CreateRotationY(MathHelper.DegreesToRadians(Rotation.Y)) *
        Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(Rotation.Z)) *
        Matrix4.CreateScale(Scale) *
        Matrix4.CreateTranslation(centro + Position);
        public void Transladate(float x, float y, float z) {
            Position += new Vector3(x, y, z);
        }

        public void Rotate(float xDeg, float yDeg, float zDeg) {
            Rotation += new Vector3(xDeg, yDeg, zDeg);
        }

        public void RotateAround(Vector3 centro, float xDeg, float yDeg, float zDeg)
        {
            Rotate(xDeg, yDeg, zDeg);
        }

        public void Escalate(float n) {
            Scale *= new Vector3(n);
        }
    }

}