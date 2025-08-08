using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem Instance { get; set; } //singleton
    public GameObject inventoryScreenUI; //reference to UI
    public bool isOpen; //check if inventory screen is open
    //public bool isFull; //check if inventory is full
    public List<GameObject> slots = new List<GameObject>(); //slots in the inventory (not the object)
    public List<string> itemList = new List<string>(); //names of items in slots
    private GameObject addedItem; //item you want to add into the inventory
    private GameObject equippedSlot; //slot you want to put the item in

    private void Awake(){
        if (Instance != null && Instance != this){
            Destroy(gameObject);
        }//end of if
        else{
            Instance = this;
        }//end of else
    }//end of Awake

    private void Start(){
        isOpen = false;
        populateSlotList();
    }//end of Start

    //attach game objects to the slots in the list
    private void populateSlotList(){
        //searching for transforms in inventoryScreenUI's transform
        foreach(Transform child in inventoryScreenUI.transform){
            if (child.CompareTag("Slots")){
                slots.Add(child.gameObject); //add if child of inventoryUI is a slot
            }//end of if
        }//end of foreach
    }//end of PopulateSlotList

    void Update(){
        if (Input.GetKeyDown(KeyCode.E) && !isOpen){ //check if E key is pressed and if the inventory system is opened
            Debug.Log("Inventory Opened");
            inventoryScreenUI.SetActive(true); //becomes visible
            isOpen = true;
            Cursor.lockState = CursorLockMode.None; //can use mouse
            Cursor.visible = true;
        }//end of if
        else if (Input.GetKeyDown(KeyCode.E) && isOpen){ //will close inventory
            inventoryScreenUI.SetActive(false);

            if (!CraftingSystem.Instance.isOpen){ //only lock if both screens are closed
                Cursor.lockState = CursorLockMode.Locked; //can not use mouse
                Cursor.visible = false;
            }//end of if

            isOpen = false;
        }//end of else if
    }//end of Update

    //revised with the help of ChatGPT
    public void AddToInventory(string itemName){
        equippedSlot = NextEmptySlot(); //find empty slot
        //exit if not empty slots
        if(equippedSlot == null){
            Debug.Log("Inventory is full");
            return;
        }//end of if

        GameObject itemPrefab = Resources.Load<GameObject>(itemName); //load prefab image from Resources folder
        //leave if no prefab
        if(itemPrefab == null){
            Debug.Log("Item not found");
            return;
        }//end of if
        addedItem = Instantiate(itemPrefab, equippedSlot.transform); //instantiate the UI prefab as a child of the empty slot
        addedItem.transform.localPosition = Vector3.zero; //reset position in slot
        addedItem.transform.localScale = Vector3.one; //good scale so it won't go out of the slot (.9)
        addedItem.transform.SetParent(equippedSlot.transform, false); //reset parent and set false (don't keep world position)
        itemList.Add(itemName); //add item to list in inspector
      
        /*
        equippedSlot = NextEmptySlot();
        addedItem = Instantiate(Resources.Load<GameObject>(itemName), equippedSlot.transform.position, equippedSlot.transform.rotation);
        addedItem.transform.SetParent(equippedSlot.transform);
        itemList.Add(itemName);
        */
    }//end of AddToInventory

    public void RemoveItem(string n, int amount){
        int counter = amount;

        for(int i=slots.Count; i>=0; i--){ //find item and remove
            if(slots[i].transform.childCount > 0){ //start from last slot for good UI
                if(slots[i].transform.GetChild(0).name == n+" (Clone)" && counter!=0){ //if match material needed, then remove it
                    Destroy(slots[i].transform.GetChild(0).gameObject);
                    counter--;
                }//end of if
            }//end of if
        }//end of for
    }//end of RemoveItem

    //checks if the inventory is full or not
    public bool CheckFull(){
        int counter = 0;
        foreach(GameObject slot in slots){
            if(slot.transform.childCount != 0){
                counter++;
            }//end of if
        }//end of foreach
        if(counter == 24){ //max amount of slots
            return true; //no more room
        }//end of if
        else{
            return false; //still has room
        }//end of else
    }//end of checkFull

     private GameObject NextEmptySlot(){
        foreach(GameObject slot in slots){
            if(slot.transform.childCount == 0){ //no children = empty
                return slot;
            }//end of if
        }//end of foreach
        return new GameObject();
    }//end of nextEmptySlot

    //recalculates inventory list after the player crafts something
    public void RecalculateList(){
        itemList.Clear(); //redo list so it must be emptied
        foreach(GameObject slot in slots){
            if(slot.transform.childCount > 0){
                string tempName = slot.transform.GetChild(0).name; //ex: will get Stone (Clone)
                string fullName = tempName;
                string clone = " (Clone)";
                string newName = name.Replace(clone, "");
                itemList.Add(newName);
            }//end of if
        }//end of foreach
    }//end of RecalculationList
}//end of InventorySystem class
