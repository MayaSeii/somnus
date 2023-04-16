using System;
using System.Linq;
using Coffee.UIExtensions;
using Settings;
using UnityEngine;

namespace UI
{
    public class TitleUIManager : MonoBehaviour
    {
        public static TitleUIManager Instance { get; private set; }
        
        #region - VAR Settings -
        
        [field: Header("Settings")]
        [field: SerializeField] public TitlePageButton[] Buttons { get; private set; }
        [field: SerializeField] public GameObject[] Pages { get; private set; }
        
        #endregion

        #region - UNITY Awake -
    
        private void Awake()
        {
            if (Instance != null) Debug.LogError("Found more than one UI Manager in the scene.");
            Instance = this;
            
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    
        #endregion
        
        #region - Settings Menu -

        public void TogglePage(int index)
        {
            SettingsManager.Instance.CurrentPage = (SettingsPage) index;
            Pages.ToList().ForEach(p => p.SetActive(Array.IndexOf(Pages, p) == index));
            Buttons.ToList().ForEach(b => b.GetComponentInChildren<Unmask>().enabled = Array.IndexOf(Buttons, b) == index);
        }

        public TitlePageButton ActiveButton()
        {
            return Buttons[(int) SettingsManager.Instance.CurrentPage];
        }
        
        #endregion
    }
}
