using UnityEngine;

namespace Extensions
{
    internal static class Collider2DExtensions
    {
        public static float GetHalfWidth(this Collider2D collider2D) => collider2D.bounds.size.x / 2;
    }
}
