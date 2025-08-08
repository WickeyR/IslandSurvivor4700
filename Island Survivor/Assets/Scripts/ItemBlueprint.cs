using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBlueprint : MonoBehaviour
{
    public string itemName; //what object the player will be crafting
    public string req1; //material needed
    public string req2; //material needed
    public int req1Amount; 
    public int req2Amount;
    public int numOfReqs;

    //constructor with user input
    public ItemBlueprint(string iN, string r1, string r2, int r1A, int r2A, int num){
        itemName = iN;
        req1 = r1;
        req2 = r2;
        req1Amount = r1A;
        req2Amount = r2A;
        numOfReqs = num;
    }//end of constructor
}//end of ItemBlueprint class
