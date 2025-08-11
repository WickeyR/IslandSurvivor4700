using System.Collections;
using UnityEngine;

public class GameBootstrap : MonoBehaviour
{
    IEnumerator Start()
    {
        // wait one frame so InventorySystem has populated its slots
        yield return null;
        InventorySystem.Instance.AddToInventory("Fruit (Psst Right Click!)");
        InventorySystem.Instance.AddToInventory("Fruit (Psst Right Click!)EW");
        InventorySystem.Instance.RecalculateList();
    }
}