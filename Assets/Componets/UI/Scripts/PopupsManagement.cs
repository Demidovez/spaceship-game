using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace PopupSpace
{
    public delegate void OnPopupNewGame();
    
    public class PopupsManagement: MonoBehaviour
    {
        [SerializeField] private Image _background;
        [SerializeField] private GameObject _popupGameOverPrefab;
        [SerializeField] private GameObject _popupGameWinPrefab;
        
        private GameObject _popupGameOver;
        private GameObject _popupGameWin;
        
        public static event OnPopupNewGame OnPopupNewGameEvent;
        
        public static PopupsManagement Instance { get; private set; }
        
        private void Awake()
        {
            Instance = this;
        }

        public static void InvokeOnPopupNewGameEvent()
        {
            OnPopupNewGameEvent?.Invoke();
        }

        public void ShowGameOverPopup()
        {
            Time.timeScale = 0f;

            _background.gameObject.SetActive(true);
            _background.DOFade(0.8f, 0.1f).SetUpdate(true);
            
            _popupGameOver = Instantiate(_popupGameOverPrefab, transform.position - new Vector3(0,5,0), transform.rotation, transform);

            DOTween.Sequence()
                .Append(_popupGameOver.transform.DOMove(transform.position + new Vector3(0, 0.5f, 0), 0.1f))
                .Append(_popupGameOver.transform.DOMove(transform.position - new Vector3(0, 0.5f, 0), 0.1f))
                .Append(_popupGameOver.transform.DOMove(transform.position, 0.1f))
                .SetUpdate(true);
        }
         
        public void ShowGameWinPopup()
        {
            Time.timeScale = 0f;

            _background.gameObject.SetActive(true);
            _background.DOFade(0.8f, 0.1f).SetUpdate(true);
            
            _popupGameWin = Instantiate(_popupGameWinPrefab, transform);
                
            DOTween.Sequence()
                .Append(_popupGameWin.transform.DOScale(0f, 0f))
                .Append(_popupGameWin.transform.DOScale(1.1f, 0.1f))
                .Append(_popupGameWin.transform.DOScale(0.9f, 0.1f))
                .Append(_popupGameWin.transform.DOScale(1f, 0.1f))
                .SetUpdate(true);
        }
        
        public void HidePopups()
        {
            Time.timeScale = 1f;
            
            _background.gameObject.SetActive(false);
            Destroy(_popupGameOver);
            Destroy(_popupGameWin);
        }
    }
}