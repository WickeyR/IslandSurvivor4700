using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildBase : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool isUseable;
    //public GameObject toBeUsed;

    // Triggered when the mouse is clicked over the item that has this script.
    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (isUseable)
            {
                //toBeUsed = gameObject; //whatever item is being clicked on
                ConstructionSystem.Instance.destroyedItem = gameObject;
                gameObject.SetActive(false);
                UseItem();
                //DestroyImmediate(gameObject); //remove item image
                //InventorySystem.Instance.RecalculateList(); //generate new list after items are used
                //CraftingSystem.Instance.RefreshReqs(); //update requirements after items are use up
            }//end of if
        }//end of if
    }//end of OnPointerDown

    // Triggered when the mouse button is released over the item that has this script.
    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (isUseable)
            {
                DestroyImmediate(gameObject);
                InventorySystem.Instance.RecalculateList();
                CraftingSystem.Instance.RefreshReqs();
            }//end of if
        }//end of if
    }//end of OnPointerUp

    public void UseItem()
    {
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

        SelectionManager.Instance.EnableSelection();
        SelectionManager.Instance.enabled = true;

        //check which item was selected
        switch (gameObject.name)
        {
            case "Foundation(Clone)":
                ConstructionSystem.Instance.ActivateConstructionPlacement("FoundationModel"); //instantiate foundation model
                break;
            case "Foundation": //for testing
                ConstructionSystem.Instance.ActivateConstructionPlacement("FoundationModel");
                break;
            case "Wall(Clone)":
                ConstructionSystem.Instance.ActivateConstructionPlacement("WallModel"); //instantiate foundation model
                break;
            case "Wall": //for testing
                ConstructionSystem.Instance.ActivateConstructionPlacement("WallModel");
                break;
            case "Fire(Clone)":
                ConstructionSystem.Instance.ActivateConstructionPlacement("FireModel"); //instantiate foundation model
                break;
            case "Fire": //for testing
                ConstructionSystem.Instance.ActivateConstructionPlacement("FireModel");
                break;
            default:
                break; //don't do anything
        }//end of switch
    }//end of UseItem
}//end of BuildBase 

