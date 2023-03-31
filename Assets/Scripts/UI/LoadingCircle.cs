using UnityEngine;

public class LoadingCircle : MonoBehaviour
{
    private void Update()
    {
        transform.Rotate(new Vector3(0f, 0f, -270f * Time.deltaTime));
    }
}
