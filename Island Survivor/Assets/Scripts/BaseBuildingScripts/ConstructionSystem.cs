using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionSystem : MonoBehaviour
{
    public static ConstructionSystem Instance { get; set; } //singleton
    public GameObject itemToBeConstructed; //reference to the item to be constructed
    public bool inConstructionMode = false; //check if in construction mode
    public GameObject constructionHoldingSpot; //hold the item to construct things 
    public bool isValidPlacement;
    public bool selectingAGhost;
    public GameObject selectedGhost;
    //Materials we store as refereces for the ghosts
    //ghosts = semi transparent item that is a placeholder for an item not yet placed in the environment
    public Material ghostSelectedMat; //show green for item being selected, red = no build
    public Material ghostSemiTransparentMat; //for testing
    public Material ghostFullTransparentMat;
    //We keep a reference to all ghosts currently in our world,
    //so the manager can monitor them for various operations
    public List<GameObject> allGhostsInExistence = new List<GameObject>();
    public GameObject destroyedItem;
    public GameObject constructionUI; //displays instructions when in construction mode
    public GameObject player; //prevent player from colliding with ghosts by disabling the collider

    private void Awake(){
        if (Instance != null && Instance != this){
            Destroy(gameObject);
        }//end of if
        else{
            Instance = this;
        }//end of else
    }//end of Awake

    //right click on item
    public void ActivateConstructionPlacement(string itemToConstruct){
        GameObject item = Instantiate(Resources.Load<GameObject>(itemToConstruct)); //instantiate item to be constructed in 3D

        //replace the name of the gameobject so it will not be a clone
        item.name = itemToConstruct;
        //set new parent to the character's holding spot for the item
        item.transform.SetParent(constructionHoldingSpot.transform, false);

        //item.transform.localPosition = Vector3.zero; //spawn at holding spot
        //item.transform.localRotation = Quaternion.identity;

        itemToBeConstructed = item;
        itemToBeConstructed.gameObject.tag = "activeConstructable";

        //Disabling the non-trigger collider so our mouse can cast a ray
        itemToBeConstructed.GetComponent<Constructable>().solidCollider.enabled = false; //set true when item is placed

        //Activating Construction mode
        inConstructionMode = true;
    }//end of ActivateConstructionPlacement

    private void GetAllGhosts(GameObject itemToBeConstructed){
        List<GameObject> ghostlist = itemToBeConstructed.gameObject.GetComponent<Constructable>().ghostList;

        foreach (GameObject ghost in ghostlist){
            Debug.Log(ghost);
            allGhostsInExistence.Add(ghost);
        }//end of foreach
    }//end of GetAllGhosts

    private void PerformGhostDeletionScan(){
        foreach (GameObject ghost in allGhostsInExistence){
            if (ghost != null){
                if (ghost.GetComponent<GhostItem>().hasSamePosition == false){ //if we did not already add a flag
                    foreach (GameObject ghostX in allGhostsInExistence){
                        //First we check that it is not the same object
                        if (ghost.gameObject != ghostX.gameObject){
                            //If its not the same object but they have the same position
                            if (XPositionToAccurateFloat(ghost) == XPositionToAccurateFloat(ghostX) && ZPositionToAccurateFloat(ghost) == ZPositionToAccurateFloat(ghostX)){
                                if (ghost != null && ghostX != null){
                                    // setting the flag
                                    ghostX.GetComponent<GhostItem>().hasSamePosition = true;
                                    break;
                                }//end of if
                            }//end of if
                        }//end of if
                    }//end of foreach
                }//end of if
            }//end of if
        }//end of foreach
        foreach (GameObject ghost in allGhostsInExistence){
            if (ghost != null){
                if (ghost.GetComponent<GhostItem>().hasSamePosition){
                    Destroy(ghost);
                }//end of if
            }//end of if
        }//end of foreach
    }//end of PerformGhostDeletionScan

    private float XPositionToAccurateFloat(GameObject ghost){
        if (ghost != null){
            //Turning the position to a 2 decimal rounded float
            Vector3 targetPosition = ghost.gameObject.transform.position;
            float pos = targetPosition.x;
            float xFloat = Mathf.Round(pos * 100f) / 100f;
            return xFloat;
        }//end of if
        return 0;
    }//end of XPositionToAccurateFloat

    private float ZPositionToAccurateFloat(GameObject ghost){
        if (ghost != null){
            //Turning the position to a 2 decimal rounded float
            Vector3 targetPosition = ghost.gameObject.transform.position;
            float pos = targetPosition.z;
            float zFloat = Mathf.Round(pos * 100f) / 100f;
            return zFloat;
        }//end of if
        return 0;
    }//end of ZPositionToAccurateFloat

    private void Update(){
        //check to see if player has selected an item before allowing construction mode to happen
        if (inConstructionMode && (itemToBeConstructed == null || itemToBeConstructed.Equals(null))){
            Debug.LogWarning("itemToBeConstructed was destroyed or null. Exiting construction mode safely.");
            inConstructionMode = false;
            return;
        }//end of if

        //display instructions when in construction mode
        if (inConstructionMode){
            constructionUI.SetActive(true);
        }//end of if
        else{
            constructionUI.SetActive(false);
        }//end of else

        if (inConstructionMode){ //double check that itemToBeConstructed has things in it
            if (itemToBeConstructed == null || itemToBeConstructed.Equals(null)){
                Debug.LogWarning("itemToBeConstructed is not assigned or was destroyed.");
                constructionUI.SetActive(false); // Hide UI
                return; // Exit Update early
            }//end of if
            else{
                if (itemToBeConstructed.name == "FoundationModel"){ //ADD FOR CAMPFIRE LATER
                  //for foundation placement
                    if (CheckValidConstructionPosition()){
                        isValidPlacement = true;
                        itemToBeConstructed.GetComponent<Constructable>().SetValidColor();
                    }//end of if
                    else{
                        isValidPlacement = false;
                        itemToBeConstructed.GetComponent<Constructable>().SetInvalidColor();
                    }//end of else
                }//end of if
            }//end of else
        }//end of if
        else{
            constructionUI.SetActive(false);
            return;
        }//end of else
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        //ADD FOR CAMPFIRE LATER
        if (Physics.Raycast(ray, out hit)){
            var selectionTransform = hit.transform;
            if (selectionTransform.gameObject.CompareTag("ghost") && itemToBeConstructed.name == "FoundationModel"){ //for placcing on foundation ghosts
                itemToBeConstructed.SetActive(false);
                selectingAGhost = true;
                selectedGhost = selectionTransform.gameObject;
            }//end of if
            else if (selectionTransform.gameObject.CompareTag("wallGhost") && itemToBeConstructed.name == "WallModell"){ //for placing on wall ghosts
                itemToBeConstructed.SetActive(false);
                selectingAGhost = true;
                selectedGhost = selectionTransform.gameObject;
            }//end of else if
            else{ //not pointing to anything with raycast
                itemToBeConstructed.SetActive(true);
                selectedGhost = null; //when no ghost is selected
                selectingAGhost = false;
            }//end of else
        }//end of if

        // Left Mouse Click to Place item
        if (Input.GetMouseButtonDown(0) && inConstructionMode){
            //Debug.Log("Left click");
            //ADD FOR CAMPFIRE LATER
            if (isValidPlacement && selectedGhost == null && itemToBeConstructed.name == "FoundationModel"){ //We don't want the freestyle to be triggered when we select a ghost.
                PlaceItemFreeStyle(); //only for foundation or campfire, walls must be on foundation
                DestroyItem(destroyedItem);
            }//end of if
            if (selectingAGhost){
                PlaceItemInGhostPosition(selectedGhost);
                DestroyItem(destroyedItem);
            }//end of if
        }//end of if
        //Press 'X' to Cancel                      //TODO - don't destroy the ui item until you actually placed it.
        if (Input.GetKeyDown(KeyCode.X) && isValidPlacement){     //Left Mouse Button
            destroyedItem.SetActive(true);
            destroyedItem = null;
            DestroyItem(itemToBeConstructed);
            itemToBeConstructed = null;
            inConstructionMode = false; //exit construction mode
        }//end of if
    }//end of Update

    private void PlaceItemInGhostPosition(GameObject copyOfGhost){
        Vector3 ghostPosition = copyOfGhost.transform.position;
        Quaternion ghostRotation = copyOfGhost.transform.rotation;

        selectedGhost.gameObject.SetActive(false);

        //Setting the item to be active again (after we disabled it in the ray cast)
        itemToBeConstructed.gameObject.SetActive(true);
        //Setting the parent to be the root of our scene
        itemToBeConstructed.transform.SetParent(transform.parent.transform.parent, true);

        var randomOffset = UnityEngine.Random.Range(0.01f, 0.03f); //random range for positioning                                                          

        itemToBeConstructed.transform.position = new Vector3(ghostPosition.x, ghostPosition.y, ghostPosition.z + randomOffset); //so walls will never be in the same spot for rendering issues
        itemToBeConstructed.transform.rotation = ghostRotation;

        itemToBeConstructed.GetComponent<Constructable>().solidCollider.enabled = true; //Enabling back the solider collider that we disabled earlier
        itemToBeConstructed.GetComponent<Constructable>().SetDefaultColor(); //set the default color (material)

        if (itemToBeConstructed.name == "FoundationModel"){ //for Foundation only, wall has no ghosts (placed on foundation), campfire can be placed anywhere 
            //Making the Ghost Children to no longer be children of this item
            itemToBeConstructed.GetComponent<Constructable>().ExtractGhostMembers();
            itemToBeConstructed.tag = "placedFoundation";
            //Adding all the ghosts of this item into the manager's ghost bank
            GetAllGhosts(itemToBeConstructed);
            PerformGhostDeletionScan();
        }//end of if
        else{ //for any other object
            itemToBeConstructed.tag = "placedWall";
            DestroyItem(selectedGhost); //destroy the ghost because no more walls can be placed there
        }//end of else

        itemToBeConstructed = null;
        inConstructionMode = false;
    }//end of PlaceItemInGhostPosition


    private void PlaceItemFreeStyle(){
        //Setting the parent to be the root of our scene
        itemToBeConstructed.transform.SetParent(transform.parent.transform.parent, true);

        //Making the Ghost Children to no longer be children of this item
        itemToBeConstructed.GetComponent<Constructable>().ExtractGhostMembers();
        //Setting the default color/material
        itemToBeConstructed.GetComponent<Constructable>().SetDefaultColor();
        itemToBeConstructed.tag = "placedFoundation";
        itemToBeConstructed.GetComponent<Constructable>().enabled = false;
        //Enabling back the solider collider that we disabled earlier
        itemToBeConstructed.GetComponent<Constructable>().solidCollider.enabled = true;

        //Adding all the ghosts of this item into the manager's ghost bank
        GetAllGhosts(itemToBeConstructed);
        PerformGhostDeletionScan();

        itemToBeConstructed = null;

        inConstructionMode = false;
    }//end of PlaceItemFreeStyle

    private bool CheckValidConstructionPosition(){
        if (itemToBeConstructed != null){
            return itemToBeConstructed.GetComponent<Constructable>().isValidToBeBuilt;
        }//end of if
        return false;
    }//end of CheckValidConstructionPosition

    /*
    void DestroyItem(GameObject item){
        Destroy(item);
        InventorySystem.Instance.RecalculateList();
        CraftingSystem.Instance.RefreshReqs();
    }//end of DestroyItem
    */
    void DestroyItem(GameObject item){
        if (item != null){
            Destroy(item);
        }//end of if
        InventorySystem.Instance.RecalculateList();
        CraftingSystem.Instance.RefreshReqs();
        StartCoroutine(Clear(item));
    }//end of DestroyItem

    //delay destruction of item
    private IEnumerator Clear(GameObject item){
        yield return null; // wait one frame
        if (itemToBeConstructed == item){
            itemToBeConstructed = null;
        }//end of if
    }//end of IEnumerator

}//end of ConstructionSystem

