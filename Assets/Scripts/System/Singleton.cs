using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour
{
    public static T Instance { get; private set; }

    protected void Setup(T instance, bool currentSceneOnly = false)
    {
        if (Instance != null)
        {
            Destroy(gameObject);

            return;
        }

        Instance = instance;

        if (!currentSceneOnly)
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
