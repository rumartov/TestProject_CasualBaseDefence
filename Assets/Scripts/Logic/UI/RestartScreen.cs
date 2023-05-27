using Logic.Hero;
using Services.ResetLevel;
using UnityEngine;
using UnityEngine.UI;

namespace Logic.UI
{
    public class RestartScreen : MonoBehaviour
    {
        [SerializeField] private Button restartButton;
        [SerializeField] private Button closeButton;
        
        private IResetLevelService _resetLevelService;
        
        public void Construct(HeroDeath heroDeath, IResetLevelService resetLevelService)
        {
            Hide();
            
            heroDeath.HeroDied += Show;
            _resetLevelService = resetLevelService;
            
            restartButton.onClick.AddListener(ResetLevel);
            closeButton.onClick.AddListener(Hide);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }

        private void ResetLevel()
        {
            _resetLevelService.ResetLevel(Constants.MainLevel);
        }
    }
}