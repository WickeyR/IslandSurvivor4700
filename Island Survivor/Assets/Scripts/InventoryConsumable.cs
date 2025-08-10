using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryConsumable : MonoBehaviour, IPointerClickHandler
{
    public string itemName = "Banana";
    public bool amountIsPercent = true;
    public float hungerAmount = 0.30f; // 30% of max hunger
    public float healthAmount = 0f;    // set >0 if food should heal

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Right) return;

        var stats = FindObjectOfType<PlayerStats>();
        if (stats != null)
        {
            float addHunger = amountIsPercent ? stats.maxHunger * hungerAmount : hungerAmount;
            stats.Eat(addHunger, healthAmount);
        }

        Destroy(gameObject);                       // remove the UI item from the slot
        InventorySystem.Instance.RecalculateList(); // keep itemList in sync
    }
}