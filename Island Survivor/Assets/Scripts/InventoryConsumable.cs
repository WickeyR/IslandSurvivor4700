using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryConsumable : MonoBehaviour, IPointerClickHandler
{
    public string itemName = "Banana";
    public bool amountIsPercent = true;
    public float hungerAmount = 0.30f; // 30% of max hunger
    public float healthAmount = 0f;    // set >0 if food should heal

    //for base building
    public bool isUseable;
    public GameObject toBeUsed;

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

        if (isUseable){
            toBeUsed = gameObject; //whatever item is being clicked on
            UseItem();
            DestroyImmediate(gameObject); //remove item image
            InventorySystem.Instance.RecalculateList(); //generate new list after items are used
            CraftingSystem.Instance.RefreshReqs(); //update requirements after items are use up
        }//end of if
    }

    public void UseItem(){
        //close all screens if open so you can place item
        InventorySystem.Instance.isOpen = false;
        InventorySystem.Instance.inventoryScreenUI.SetActive(false);
        CraftingSystem.Instance.isOpen = false;
        CraftingSystem.Instance.craftingScreenUI.SetActive(false);
        CraftingSystem.Instance.toolsScreenUI.SetActive(false);
        CraftingSystem.Instance.boatScreenUI.SetActive(false);
        CraftingSystem.Instance.baseScreenUI.SetActive(false);

        //go back to playing mode with locked cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //SelectionManager.Instance.EnableSelection();
        SelectionManager.Instance.enabled = true;

        //check which item was selected
        switch(gameObject.name){
            case "Foundation(Clone)":
                ConstructionSystem.Instance.ActivateConstructionPlacement("FoundationModel"); //instantiate foundation model
                break;
            case "Foundation": //for testing
                ConstructionSystem.Instance.ActivateConstructionPlacement("FoundationModel");
                break;
            //case "Wall":
               // ConstructionManager.Instance.ActivateConstructionPlacement("WallModel");
               // break;
            default:
                break; //don't do anything
        }//end of switch
    }//end of UseItem
}

