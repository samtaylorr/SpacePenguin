using UnityEngine;
using UnityEngine.Events;

public class RunEventOnStart : MonoBehaviour
{
    public UnityEvent start;

    // Start is called before the first frame update
    void Start()
    {
        start.Invoke();
    }
}
