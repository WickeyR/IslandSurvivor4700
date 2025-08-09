using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBlueprint : MonoBehaviour
{
    public string itemName; //what object the player will be crafting
    public string req1; //material needed
    public string req2; //material needed
    public string reqHammer; //some items require a hammer to craft, will not be removed
    public int req1Amount; 
    public int req2Amount;
    public int reqHammerAmount; 
    public int numOfReqs;

    //default constructor
    public ItemBlueprint(){
        itemName = "";
        req1 = "";
        req2 = "";
        reqHammer = "";
        req1Amount = 0;
        req2Amount = 0;
        reqHammerAmount = 0;
        numOfReqs = 3;
    }//end of default constructor

    //constructor for 2 requirement blue prints, possible 3rd requiremenet is preset
    public ItemBlueprint(string iN, string r1, string r2, int r1A, int r2A, int num){
        itemName = iN;
        req1 = r1;
        req2 = r2;
        reqHammer = "Hammer";
        req1Amount = r1A;
        req2Amount = r2A;
        reqHammerAmount = 1;
        numOfReqs = num;
    }//end of constructor
}//end of ItemBlueprint class
