using UnityEngine;

public abstract class Pickable : MonoBehaviour
{
    public virtual void Pickup()
    {
        Destroy(this.gameObject);
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Pickup();
        }
    }
}