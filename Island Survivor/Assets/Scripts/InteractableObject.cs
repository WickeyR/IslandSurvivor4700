using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public string ItemName;

    //displays object name
    public string GetItemName(){
        return ItemName;
    }//end of GetItemName()
}//end of InteractableObject