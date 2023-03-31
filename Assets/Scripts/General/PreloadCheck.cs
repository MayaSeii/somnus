using UnityEngine;

public class PreloadCheck : MonoBehaviour
{
    private void Awake()
    {
        if (!GameObject.Find("__app")) UnityEngine.SceneManagement.SceneManager.LoadScene("_preload");
    }
}
