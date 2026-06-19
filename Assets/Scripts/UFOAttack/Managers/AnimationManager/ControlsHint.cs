using UnityEngine;

public class ControlsHint : MonoBehaviour
{
    [SerializeField] private float displayDuration = 15f;

    private void Start()
    {
        Destroy(gameObject, displayDuration);
    }
}