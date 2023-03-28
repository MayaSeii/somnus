using UnityEngine;
using UnityEngine.UI;

public class MenuScrollbar : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Scrollbar>().value = 1;
    }
}
