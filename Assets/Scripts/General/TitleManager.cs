using System.Collections;
using Audio;
using FMODUnity;
using General;
using Inputs;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class TitleManager : MonoBehaviour
{
    public static TitleManager Instance { get; private set; }
    
    [field: SerializeField] public Image Fader { get; private set; }
    [field: SerializeField] public GameObject TitleScreen { get; private set; }
    [field: SerializeField] public GameObject SettingsMenu { get; private set; }
    
    private void Awake()
    {
        Instance = this;
        InitialiseCursor();
    }

    private void Start()
    {
        GameManager.Instance.InitialiseInMenu();
        AudioManager.Instance.InitialiseInMenu();
        ControlsManager.Instance.InitialiseInMenu();
    }

    private static void InitialiseCursor()
    {
        var cursorTexture = Resources.Load<Texture2D>("Sprites/Cursor");
        Cursor.SetCursor(cursorTexture, new Vector2(8, 0), CursorMode.Auto);
    }

    public void StartGame()
    {
        RuntimeManager.PlayOneShot(FMODEvents.Instance.KeySmash);
        RuntimeManager.PlayOneShot(FMODEvents.Instance.SettingsButton);
        
        AudioManager.Instance.EventInstances["MenuAmbience"].stop(STOP_MODE.ALLOWFADEOUT);
        AudioManager.Instance.EventInstances["DeepHum"].stop(STOP_MODE.ALLOWFADEOUT);
        
        StartCoroutine(FadeOut());
    }

    public void QuitGame()
    {
        RuntimeManager.PlayOneShot(FMODEvents.Instance.SettingsButton);
        Application.Quit();
    }

    private IEnumerator FadeOut()
    {
        Cursor.visible = false;
        Fader.enabled = true;
        
        while (Fader.color.a < 1f)
        {
            var colour = Fader.color;
            colour.a += Time.deltaTime * 2f;
            Fader.color = colour;
            yield return null;
        }

        SceneManager.LoadScene("Loading");
        yield return null;
    }
    
    public void ToggleSettings()
    {
        RuntimeManager.PlayOneShot(FMODEvents.Instance.SettingsButton);
        SettingsMenu.SetActive(!SettingsMenu.activeInHierarchy);
        TitleScreen.SetActive(!TitleScreen.activeInHierarchy);
    }

    public void ToggleSettings(InputAction.CallbackContext context)
    {
        RuntimeManager.PlayOneShot(FMODEvents.Instance.SettingsButton);
        SettingsMenu.SetActive(!SettingsMenu.activeInHierarchy);
        TitleScreen.SetActive(!TitleScreen.activeInHierarchy);
    }
}
