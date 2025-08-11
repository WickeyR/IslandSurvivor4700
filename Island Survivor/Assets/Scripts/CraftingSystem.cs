using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftingSystem : MonoBehaviour
{
    public GameObject craftingScreenUI;
    public GameObject toolsScreenUI; //crafting tools
    public GameObject boatScreenUI; //crafting the escape boat
    public GameObject paddleScreenUI; //crafting paddle for boat
    public List<string> inventoryList = new List<string>();
    public bool isOpen; //check if screen is open
    //create reference for the crafting buttons
    Button toolsBTN, boatBTN, paddleBTN;
    Button craftHammerBTN, craftNailsBTN, craftBoatBTN, craftPaddleBTN;
    //create reference for requirements
    TextMeshProUGUI hammerReq1, hammerReq2, nailsReq1, boatReq1, boatReq2, boatReq3, paddleReq1, paddleReq2, paddleReq3;
    //item blueprints
    public ItemBlueprint Hammer;
    public ItemBlueprint Nails;
    public ItemBlueprint Boat;
    public ItemBlueprint Paddle;

    //singleton pattern
    public static CraftingSystem Instance { get; set; }
    private void Awake(){
        if(Instance!=null && Instance != this){
            Destroy(gameObject);
        }//end of if
        else{
            Instance = this;
        }//end of else
        //make blueprint for Hammer
        GameObject bpH = new GameObject("Hammer");
        Hammer = bpH.AddComponent<ItemBlueprint>();
        Hammer.itemName = "Hammer";
        Hammer.req1 = "Wood (Psst Right Click!)";
        Hammer.req2 = "Rock (Psst Right Click!)";
        Hammer.req1Amount = 1;
        Hammer.req2Amount = 1;
        Hammer.numOfReqs = 2;
        //make blueprint for Nails
        GameObject bpN = new GameObject("Nails");
        Nails = bpN.AddComponent<ItemBlueprint>();
        Nails.itemName = "Nails";
        Nails.req1 = "Rock (Psst Right Click!)";
        Nails.req1Amount = 2;
        Nails.numOfReqs = 1;
        //make blueprint for Boat
        GameObject bpB = new GameObject("Boat");
        Boat = bpB.AddComponent<ItemBlueprint>();
        Boat.itemName = "Boat";
        Boat.req1 = "Wood (Psst Right Click!)";
        Boat.req2 = "Nails";
        Boat.reqHammer = "Hammer";
        Boat.req1Amount = 10;
        Boat.req2Amount = 5;
        Boat.reqHammerAmount = 1;
        Boat.numOfReqs = 3;
        //make blueprint for Paddle
        GameObject bpP = new GameObject("Paddle");
        Paddle = bpP.AddComponent<ItemBlueprint>();
        Paddle.itemName = "Paddle";
        Paddle.req1 = "Wood (Psst Right Click!)";
        Paddle.req2 = "Nails";
        Paddle.reqHammer = "Hammer";
        Paddle.req1Amount = 2;
        Paddle.req2Amount = 3;
        Paddle.reqHammerAmount = 1;
        Paddle.numOfReqs = 3;
    }//end of Awake

    // Start is called before the first frame update
    void Start(){
        isOpen = false;

        //Temporarily activate tools screen to allow finding children
        //bool wasActive = toolsScreenUI.activeSelf;
        //toolsScreenUI.SetActive(true);
        //to open the tools screen
        toolsBTN = craftingScreenUI.transform.Find("toolsButton").GetComponent<Button>();
        toolsBTN.onClick.AddListener(delegate { OpenToolsCategory(); });
        //for hammer
        Transform hammerTransform = toolsScreenUI.transform.Find("Hammer");
        hammerReq1 = hammerTransform.Find("hammerReq1").GetComponent<TextMeshProUGUI>();
        hammerReq2 = hammerTransform.Find("hammerReq2").GetComponent<TextMeshProUGUI>();
        craftHammerBTN = hammerTransform.Find("craftHammerBTN").GetComponent<Button>();
        craftHammerBTN.onClick.AddListener(delegate { CraftItem(Hammer); });
        //for nails
        Transform nailsTransform = toolsScreenUI.transform.Find("Nails");
        nailsReq1 = nailsTransform.Find("nailsReq1").GetComponent<TextMeshProUGUI>();
        craftNailsBTN = nailsTransform.Find("craftNailsBTN").GetComponent<Button>();
        craftNailsBTN.onClick.AddListener(delegate { CraftItem(Nails); });
        //Return UI to its previous state (invisible again)
        //toolsScreenUI.SetActive(wasActive);

        //open boat crafting screen
        //bool boatActive = boatScreenUI.activeSelf;
        //boatScreenUI.SetActive(true);
        boatBTN = craftingScreenUI.transform.Find("boatButton").GetComponent<Button>();
        boatBTN.onClick.AddListener(delegate { OpenBoatCategory(); });
        Transform boatTransform = boatScreenUI.transform.Find("Boat");
        boatReq1 = boatTransform.Find("boatReq1").GetComponent<TextMeshProUGUI>();
        boatReq2 = boatTransform.Find("boatReq2").GetComponent<TextMeshProUGUI>();
        boatReq3 = boatTransform.Find("boatReq3").GetComponent<TextMeshProUGUI>(); //the hammer
        craftBoatBTN = boatTransform.Find("craftBoatBTN").GetComponent<Button>();
        craftBoatBTN.onClick.AddListener(delegate { CraftItem(Boat); });
        //boatScreenUI.SetActive(boatActive); //make invisible again

        //open paddle crafting screen
        //bool wasActive = toolsScreenUI.activeSelf;
        //toolsScreenUI.SetActive(true);
        paddleBTN = craftingScreenUI.transform.Find("paddleButton").GetComponent<Button>();
        paddleBTN.onClick.AddListener(delegate { OpenPaddleCategory(); });
        //Debug.Log("worked");
        Transform paddleTransform = paddleScreenUI.transform.Find("Paddle");
        paddleReq1 = paddleTransform.Find("paddleReq1").GetComponent<TextMeshProUGUI>();
        paddleReq2 = paddleTransform.Find("paddleReq2").GetComponent<TextMeshProUGUI>();
        paddleReq3 = paddleTransform.Find("paddleReq3").GetComponent<TextMeshProUGUI>();
        craftPaddleBTN = paddleTransform.Find("craftPaddleBTN").GetComponent<Button>();
        craftPaddleBTN.onClick.AddListener(delegate { CraftItem(Paddle); });
        //toolsScreenUI.SetActive(wasActive);

        RefreshReqs(); //get updated reqs
    }//end of Start

    //Update is called once per frame
    void Update(){
        if (Input.GetKeyDown(KeyCode.C) && !isOpen)
        { //check if C key is pressed and if the crafting system is opened
            craftingScreenUI.SetActive(true); //becomes visible
            isOpen = true;
            Cursor.lockState = CursorLockMode.None; //can use mouse
            Cursor.visible = true;
        }//end of if
        else if (Input.GetKeyDown(KeyCode.C) && isOpen){ //will close inventory
            craftingScreenUI.SetActive(false);
            toolsScreenUI.SetActive(false); //close just in case
            boatScreenUI.SetActive(false);
            paddleScreenUI.SetActive(false);
            if (!InventorySystem.Instance.isOpen){ //only lock if both screens are closed
                Cursor.lockState = CursorLockMode.Locked; //can not use mouse
                Cursor.visible = false;
            }//end of if

            isOpen = false;
        }//end of else if
        RefreshReqs();
    }//end of Update

    //open the tools crafting page
    private void OpenToolsCategory(){
        craftingScreenUI.SetActive(false); //close crafting screen
        toolsScreenUI.SetActive(true); //open tools crafting screen
        RefreshReqs(); //show most update requirements when tools screen is pulled up
    }//end of OpenToolsCategory

    //open the boat crafting page
    private void OpenBoatCategory(){
        craftingScreenUI.SetActive(false); //close crafting screen
        boatScreenUI.SetActive(true); //open boat crafting screen
        RefreshReqs(); //get newest requirements
    }//end of OpenBoatCategory

    //open the paddle boat crafting page
    private void OpenPaddleCategory(){
        craftingScreenUI.SetActive(false); //close crafting screen
        paddleScreenUI.SetActive(true);
        //Debug.Log("opened");
        RefreshReqs();
    }//end of OpenPaddleCategory

    private void CraftItem(ItemBlueprint craftable){
        //check that all items are there if crafting again
        if (!Checker(craftable)){
            Debug.Log("Requirements not met");
            return;
        }//end of if
        InventorySystem.Instance.AddToInventory(craftable.itemName); //add whatever you made into the inventory
        if(craftable.numOfReqs == 1){
            InventorySystem.Instance.RemoveItem(craftable.req1, craftable.req1Amount);//remove used materials from inventory
        }//end of if
        else if(craftable.numOfReqs==2 || craftable.numOfReqs==3){
            InventorySystem.Instance.RemoveItem(craftable.req1, craftable.req1Amount);//remove used materials from inventory
            InventorySystem.Instance.RemoveItem(craftable.req2, craftable.req2Amount);//remove used materials from inventory
            //do not remove hammer as it is a reusable item
        }//end of else if
        StartCoroutine(calculate()); //to refresh the list properly
        RefreshReqs(); 
    }//end of CraftItem

    public IEnumerator calculate(){
        yield return new WaitForSeconds(1f);
        InventorySystem.Instance.RecalculateList();
    }//end of calculate

    private void RefreshReqs(){
        //crafting materials
        int rockCount = 0; //amount of rocks needed
        int woodCount = 0; //amount of wood need
        int hammerCount = 0; //check if player has a hammer or no
        int nailCount = 0;
        inventoryList = InventorySystem.Instance.itemList;

        //list what the player has on the crafting screen
        foreach(string iN in inventoryList){ //iN = item name
            switch(iN){
                case "Rock (Psst Right Click!)":
                    rockCount++;
                    break;
                case "Wood (Psst Right Click!)":
                    woodCount++;
                    break;
                case "Hammer":
                    hammerCount++;
                    break;
                case "Nails":
                    nailCount++;
                    break;
            }//end of switch statement
        }//end  of foreach

        //Hammer display
        hammerReq1.text = "One piece of Wood [" + woodCount + "]";
        hammerReq2.text = "One Rock [" + rockCount + "]";
        //check if the player can craft
        if(woodCount>=Hammer.req1Amount && rockCount>=Hammer.req2Amount){
            craftHammerBTN.gameObject.SetActive(true);
        }//end of if
        else{
            craftHammerBTN.gameObject.SetActive(false);
        }//end of else

        //Nails display
        nailsReq1.text = "Two Rocks [" + rockCount + "]";
        //check if the player can craft
        if(rockCount>=Nails.req1Amount){
            craftNailsBTN.gameObject.SetActive(true);
        }//end of if
        else{
            craftNailsBTN.gameObject.SetActive(false);
        }//end of else

        //Boat display
        boatReq1.text = "Ten pieces of Wood [" + woodCount + "]";
        boatReq2.text = "Five nails [" + nailCount + "]";
        boatReq3.text = "One hammer [" + hammerCount + "]";
        if(woodCount>=Boat.req1Amount && nailCount>=Boat.req2Amount && hammerCount>=Boat.reqHammerAmount){
        craftBoatBTN.gameObject.SetActive(true);
        }//end of if
        else{
            craftBoatBTN.gameObject.SetActive(false);
        }//end of else

        //Paddle display
        paddleReq1.text = "Two pieces of Wood [" + woodCount + "]";
        paddleReq2.text = "Three nails [" + nailCount + "]";
        paddleReq3.text = "One hammer [" + hammerCount + "]";
        if(woodCount>=Paddle.req1Amount && nailCount>=Paddle.req2Amount && hammerCount >= Boat.reqHammerAmount){
            craftPaddleBTN.gameObject.SetActive(true);
        }//end of if
        else{
            craftPaddleBTN.gameObject.SetActive(false);
        }//end of else
    }//end of RefreshReqs

    //double checks player has all the items before allowing to craft after first craft
    private bool Checker(ItemBlueprint item){
        int req1Count = 0;
        int req2Count = 0;
        int reqHCount = 0; //for hammer

        foreach(string i in InventorySystem.Instance.itemList){ //i for item
            if(i == item.req1){ //count amount of items for req1
                req1Count++;
            }//end of if
            if(item.numOfReqs==2 && i==item.req2){ //count amount of items for req2 if needed
                req2Count++;
            }//end of if
            if (item.numOfReqs==3 && i==item.reqHammer){ //make sure there's a hammer
                reqHCount++;
            }//end of if
        }//end of foreach
        if(item.numOfReqs == 1){ //check if requirements are met (one requirement only)
            return req1Count >= item.req1Amount;
        }//end of if
        else if(item.numOfReqs == 2){ //check for two requirements
            return req1Count>=item.req1Amount && req2Count>=item.req2Amount;
        }//end of elseif
        else if(item.numOfReqs == 3){ //check for three requirements
            return req1Count>=item.req1Amount && req2Count>=item.req2Amount && reqHCount>=1;
        }//end of else if
        else{
            return false;
        }//end of else
    }//end of Checker
}//end of CraftingSystem
