using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Managers
{
    public class DebugManager : MonoBehaviour
    {
        public static DebugManager Instance;
    
        #region - VAR Debug Menu -
    
        [field: SerializeField] public GameObject DebugInfo { get; set; }
        [field: SerializeField] public TMP_Text CurrentRoomText { get; set; }
        [field: SerializeField] public TMP_Text CurrentInteractableText { get; set; }
    
        #endregion
    
        #region - UNITY Awake -
    
        private void Awake()
        {
            Instance = this;
        }
    
        #endregion
    
        #region - Debug Menu -

        public void ToggleDebugInfo(InputAction.CallbackContext context)
        {
            DebugInfo.SetActive(!DebugInfo.activeInHierarchy);
        }
    
        public void UpdateCurrentRoom(string text)
        {
            CurrentRoomText.text = text;
        }
        
        public void UpdateCurrentInteractable(string text)
        {
            CurrentInteractableText.text = text;
        }
    
        #endregion
    }
}
