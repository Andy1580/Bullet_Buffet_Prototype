using UnityEngine;

public class DestructionObject : MonoBehaviour
{
    [SerializeField] private float timeToDestroy = 0.08f;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, timeToDestroy);
    }
}
