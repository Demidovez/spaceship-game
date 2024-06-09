using UnityEngine;
using UnityEngine.UI;

namespace PopupSpace
{
    public class PopupSimple: MonoBehaviour
    {
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _exitButton;

        private void Start()
        {
            _restartButton.onClick.AddListener(PopupsManagement.InvokeOnPopupNewGameEvent);
            _exitButton.onClick.AddListener(QuitGame);
        }

        private void QuitGame()
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
            Application.Quit();
        }
    }
}