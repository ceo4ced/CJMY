using UnityEngine;
using UnityEngine.Events;

public class EventManagerScript : MonoBehaviour
{
    public static EventManagerScript instance;
    public UnityEvent onSprayCanCollected;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            onSprayCanCollected = new UnityEvent();
            DontDestroyOnLoad(gameObject); // Optional: Only if you want it to persist across scenes
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
}
