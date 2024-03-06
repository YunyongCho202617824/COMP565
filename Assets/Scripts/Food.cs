using UnityEngine;

public class Food : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        gameObject.SetActive(false);
    }
}
