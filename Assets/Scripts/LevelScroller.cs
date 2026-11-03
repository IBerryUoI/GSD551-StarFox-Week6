using UnityEngine;

public class LevelScroller : MonoBehaviour
{
    [SerializeField]
    private float scrollSpeed = 10f;

    // Update is called once per frame
    void Update()
    {
        Scroll();
    }

    private void Scroll()
    {
        Vector3 scrollMovement = Vector3.back * scrollSpeed * Time.deltaTime;
        transform.position += scrollMovement;
    }
}
