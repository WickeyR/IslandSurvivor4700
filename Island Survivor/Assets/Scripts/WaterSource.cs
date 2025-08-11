using UnityEngine;

public class WaterSource : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetMouseButtonDown(1))
        {
            PlayerStats stats = other.GetComponent<PlayerStats>();
            if (stats != null)
            {
                stats.DrinkFull();
                Debug.Log("Drank water and restored thirst!");
            }
        }
    }
}