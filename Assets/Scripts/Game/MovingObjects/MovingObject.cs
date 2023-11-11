using UnityEngine;

public abstract class MovingObject : MonoBehaviour
{
    [HideInInspector]
    public new Transform transform;

    protected void Setup()
    {
        transform = GetComponent<Transform>();
    }
}
