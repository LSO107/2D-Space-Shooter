using UnityEngine;

namespace Extensions
{
    internal static class CameraExtensions
    {
        // Extend the camera class to include FitsOnScreen method so it doesn't
        // have to live in an arbitrary class. 
        //
        // The method relies on a camera to function, so it makes sense to
        // add it to the Camera class via extension methods
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
