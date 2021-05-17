using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInformation : MonoBehaviour
{
    public enum ItemType
    {
        EnergyTank,
        HighEnergyTank,
        SmallRepairParts,
        MediumRepairParts,
        HighRepairParts,
        key
    };

    [Serializable]
    public struct ItemInfo
    {
        public ItemType itemType;
        public string itemName;
        public int maxQuantity;
        public int quantity;
        public int upgradeQuantity;
        public GameObject upgradeItem;
        [TextArea(1, 6)]
        public string text;
    }

    [SerializeField]
    private ItemInfo itemInfo;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Find("Name").GetComponent<Text>().text = itemInfo.itemName + "× " + itemInfo.quantity + " (最大数" + itemInfo.maxQuantity + ")";
    }

    public ItemInfo Item
    {
        get { return itemInfo; }
    }

    public int ItemQuantity
    {
        get { return itemInfo.quantity; }
        set { itemInfo.quantity = value; }
    }
}
