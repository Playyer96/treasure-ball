using UnityEngine;

public abstract class Pickupable : MonoBehaviour
{
    public abstract void Pickup();

    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Pickup();
        }
    }
}