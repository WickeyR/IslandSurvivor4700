using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{
    public GameObject InteractionInfoUI; //allows us to turn UI element on and off
    Text interaction_text; //changing the text value directly in the Inspector

    private void Start(){
        interaction_text = InteractionInfoUI.GetComponent<Text>(); //assigned to internal text component
    }//end of start

    void Update(){
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //casts a ray from the center of the screen
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)){ //checks if the ray hit anything and stores information in hit if something was hit
            var selectionTransform = hit.transform; //grabs hit object

            if (selectionTransform.GetComponent<InteractableObject>()){
                //grabs the item name to display
                interaction_text.text = selectionTransform.GetComponent<InteractableObject>().GetItemName();
                InteractionInfoUI.SetActive(true);
            }//end of if
            else{
                InteractionInfoUI.SetActive(false); //shows nothing if object is nnot interactable
            }//end of else
        }//end of if
    }//end of Update
}//end of SelectionManager