using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlots : MonoBehaviour, IDropHandler
{
    public GameObject Item{
        get{
            //ensures you can't add more than one item at once, check if slot it full
            if (transform.childCount > 0){ 
                return transform.GetChild(0).gameObject; 
            }//end of if
            return null;
        }//end of get
    }//end of Item

    public void OnDrop(PointerEventData eventData){
        Debug.Log("OnDrop");
        //can put in item if there is nothing in there
        if (!Item){
            ItemDragNDrop.item.transform.SetParent(transform);
            //drops in center of the slot
            ItemDragNDrop.item.transform.localPosition = new Vector2(0, 0);
        }//end of if
    }//end of OnDrop
}//end of ItemSlots
