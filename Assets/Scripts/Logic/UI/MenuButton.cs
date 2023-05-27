using UnityEngine;
using UnityEngine.UI;

namespace Logic.UI
{
    public class MenuButton : MonoBehaviour
    {
        [SerializeField] private Button menuButton;
        [SerializeField] private RestartScreen restartScreen;
        
        public void Awake()
        {
            menuButton.onClick.AddListener(restartScreen.Show);;
        }
    }
}