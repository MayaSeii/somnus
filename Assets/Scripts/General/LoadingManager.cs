using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    [field: SerializeField] public int SceneToLoad { get; set; }
    
    private void Start()
    {
        var cursorTexture = Resources.Load<Texture2D>("Sprites/Cursor");
        Cursor.SetCursor(cursorTexture, new Vector2(8, 0), CursorMode.Auto);
        Cursor.visible = false;
        
        StartCoroutine(LoadSceneAsync(SceneToLoad));
    }

    private static IEnumerator LoadSceneAsync(int id)
    {
        yield return new WaitForSeconds(2f);
        var operation = SceneManager.LoadSceneAsync(id, LoadSceneMode.Single);
        while (!operation.isDone) yield return null;
    }
}
