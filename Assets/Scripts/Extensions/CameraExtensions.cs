using UnityEngine;

namespace Extensions
{
    internal static class CameraExtensions
    {
        // Extend the camera class, to include a useful method which
        // would otherwise have to live in some arbitrary helper class
        //
        // As this method relies on a camera to function, it is better to
        // instead add this method to the Camera class via extension methods
        public static bool FitsOnScreen(this Camera camera, Vector3 position, float halfWidth)
        {
            var left = new Vector2(position.x - halfWidth, position.y);
            var right = new Vector2(position.x + halfWidth, position.y);

            var leftPoint = camera.WorldToViewportPoint(left);
            var rightPoint = camera.WorldToViewportPoint(right);

            var onScreen = leftPoint.x <= 1
                           && leftPoint.x >= 0
                           && rightPoint.x <= 1
                           && rightPoint.x >= 0;

            return onScreen;
        }
    }
}
