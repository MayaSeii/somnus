using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace General
{
    public class DebugManager : MonoBehaviour
    {
        public static DebugManager Instance;
    
        #region - VAR Debug Menu -
    
        [field: SerializeField] public GameObject DebugInfo { get; set; }
        [field: SerializeField] public TMP_Text CurrentRoomText { get; set; }
        [field: SerializeField] public TMP_Text CurrentInteractableText { get; set; }
        [field: SerializeField] public TMP_Text LastHaunt { get; set; }
    
        #endregion
    
        #region - UNITY Awake -
    
        private void Awake()
        {
            if (Instance != null) Debug.LogError("Found more than one Debug Manager in the scene.");
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

        public void UpdateLastHaunt(string text)
        {
            LastHaunt.text = $"Last Haunt: {text}";
        }
    
        #endregion
    }
}
