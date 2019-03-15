using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public sealed class MainMenu : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup start;
        [SerializeField]
        private CanvasGroup controls;
        [SerializeField]
        private CanvasGroup settings;

        public void StartGame()
        {
            SceneManager.LoadScene("Level 1");
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        public void OpenControls()
        {
            ToggleMenu(false, true, false);
        }

        public void OpenSettings()
        {
            ToggleMenu(false, false, true);
        }

        public void ReturnToStartScreen()
        {
            ToggleMenu(true, false, false);
        }

        private void ToggleMenu(bool displayStart, bool displayControls, bool displaySettings)
        {
            UserInterfaceUtils.ToggleCanvasGroup(start, displayStart);
            UserInterfaceUtils.ToggleCanvasGroup(controls, displayControls);
            UserInterfaceUtils.ToggleCanvasGroup(settings, displaySettings);
        }
    }
}
