#if UNITY_EDITOR
namespace GSpawn_Pro
{
    public abstract class Shape3D
    {
        public abstract void drawFilled();
        public abstract void drawWire();
    }
}
#endif