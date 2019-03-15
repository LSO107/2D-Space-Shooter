using UnityEngine;

namespace UI
{
    internal sealed class UserInterfaceUtils
    {
        /// <summary>
        /// enable or disable canvasGroup 
        /// </summary>
        public static void ToggleCanvasGroup(CanvasGroup group, bool show)
        {
            group.alpha = show ? 1 : 0;
            group.blocksRaycasts = show;
            group.interactable = show;
        }
    }
}
