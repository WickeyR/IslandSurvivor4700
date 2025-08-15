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

    private void Awake(){
        if (Instance != null && Instance != this){
            Destroy(gameObject);
        }//end of if
        else{
            Instance = this;
        }//end of else
    }//end of Awake

    public void ActivateConstructionPlacement(string itemToConstruct){
        GameObject item = Instantiate(Resources.Load<GameObject>(itemToConstruct));

        //change the name of the gameobject so it will not be (clone)
        item.name = itemToConstruct;

        item.transform.SetParent(constructionHoldingSpot.transform, false);
        itemToBeConstructed = item;
        itemToBeConstructed.gameObject.tag = "activeConstructable";

        //Disabling the non-trigger collider so our mouse can cast a ray
        itemToBeConstructed.GetComponent<Constructable>().solidCollider.enabled = false;

        //Actiavting Construction mode
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
                if (ghost.GetComponent<GhostItem>().hasSamePosition == false) //if we did not already add a flag{
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
            }//end of foreach
        foreach (GameObject ghost in allGhostsInExistence){
            if (ghost != null){
                if (ghost.GetComponent<GhostItem>().hasSamePosition){
                    DestroyImmediate(ghost);
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
        if (itemToBeConstructed != null && inConstructionMode){
            if (CheckValidConstructionPosition()){
                isValidPlacement = true;
                itemToBeConstructed.GetComponent<Constructable>().SetValidColor();
            }//end of if
            else{
                isValidPlacement = false;
                itemToBeConstructed.GetComponent<Constructable>().SetInvalidColor();
            }//end of else
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)){
                var selectionTransform = hit.transform;
                if (selectionTransform.gameObject.CompareTag("ghost")){
                    itemToBeConstructed.SetActive(false);
                    selectingAGhost = true;
                    selectedGhost = selectionTransform.gameObject;
                }//end of if
                else{
                    itemToBeConstructed.SetActive(true);
                    selectingAGhost = false;
                }//end of else
            }//end of if
        }//end of if

        // Left Mouse Click to Place item
        if (Input.GetMouseButtonDown(0) && inConstructionMode){
            if (isValidPlacement && selectedGhost == false){ //We don't want the freestyle to be triggered when we select a ghost.
                PlaceItemFreeStyle();
            }//end of if
            if (selectingAGhost){
                PlaceItemInGhostPosition(selectedGhost);
            }//end of if
        }//end of if
        // Right Mouse Click to Cancel                      //TODO - don't destroy the ui item until you actually placed it.
        if (Input.GetMouseButtonDown(0) && isValidPlacement){     //Left Mouse Button

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

        itemToBeConstructed.transform.position = ghostPosition;
        itemToBeConstructed.transform.rotation = ghostRotation;

        //Making the Ghost Children to no longer be children of this item
        itemToBeConstructed.GetComponent<Constructable>().ExtractGhostMembers();
        //Setting the default color/material
        itemToBeConstructed.GetComponent<Constructable>().SetDefaultColor();
        itemToBeConstructed.tag = "placedFoundation";

        //Enabling back the solider collider that we disabled earlier
        itemToBeConstructed.GetComponent<Constructable>().solidCollider.enabled = true;

        //Adding all the ghosts of this item into the manager's ghost bank
        GetAllGhosts(itemToBeConstructed);
        PerformGhostDeletionScan();

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
}//end of ConstructionSystem
