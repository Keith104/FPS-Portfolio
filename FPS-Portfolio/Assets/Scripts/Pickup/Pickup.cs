using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField] PickupItems item;

    private void OnTriggerEnter(Collider other)
    {
        IPickup pickupable = other.GetComponent<IPickup>();

        if (pickupable != null)
        {
            pickupable.GetPickupItem(item);
            Destroy(gameObject);
        }
    }
}
