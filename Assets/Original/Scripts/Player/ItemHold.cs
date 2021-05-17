using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemHold : MonoBehaviour
{
    public ActivityLogUIManager activityLogPanel;
    public EquipmentParts equipmentParts;
    public PartsListUIManager partsContent;
    public ItemListUIManager itemContent;
    public List<GameObject> parts;
    public List<GameObject> items;
    public GameObject logPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        parts = partsContent.parts;
        items = itemContent.items;
    }

    public void AddParts(GameObject newParts)
    {
        bool partsGet = true;
        string partsName = null;

        if (newParts.GetComponent<PartsInformation>().Parts.partsType == PartsInformation.PartsType.AttackParts)
        {
            partsName = "火力パーツ(+" + newParts.GetComponent<PartsInformation>().Parts.power + ")";
        }
        else if (newParts.GetComponent<PartsInformation>().Parts.partsType == PartsInformation.PartsType.ShieldParts)
        {
            partsName = "頑強パーツ(+" + newParts.GetComponent<PartsInformation>().Parts.power + ")";
        }

        if (equipmentParts.WeaponInfo.partsType.ToString() == newParts.GetComponent<PartsInformation>().Parts.partsType.ToString() &&
            equipmentParts.WeaponInfo.power == newParts.GetComponent<PartsInformation>().Parts.power)
        {
            partsGet = false;
            logPrefab.transform.GetChild(0).GetComponent<Text>().text = partsName + "をドロップしましたが、既に装備しているため獲得できませんでした。";
            activityLogPanel.AddLog(logPrefab);
            Debug.Log(newParts.GetComponent<PartsInformation>().Parts.partsType + "(" + newParts.GetComponent<PartsInformation>().Parts.power + ")" + "をドロップしましたが、既に装備しているため獲得できませんでした。");
        }

        if (equipmentParts.ShieldInfo.partsType.ToString() == newParts.GetComponent<PartsInformation>().Parts.partsType.ToString() &&
            equipmentParts.ShieldInfo.power == newParts.GetComponent<PartsInformation>().Parts.power)
        {
            partsGet = false;
            logPrefab.transform.GetChild(0).GetComponent<Text>().text = partsName + "をドロップしましたが、既に装備しているため獲得できませんでした。";
            activityLogPanel.AddLog(logPrefab);
            Debug.Log(newParts.GetComponent<PartsInformation>().Parts.partsType + "(" + newParts.GetComponent<PartsInformation>().Parts.power + ")" + "をドロップしましたが、既に装備しているため獲得できませんでした。");
        }

        foreach (GameObject childParts in parts)
        {
            if (childParts.GetComponent<PartsInformation>().Parts.partsType == newParts.GetComponent<PartsInformation>().Parts.partsType &&
                childParts.GetComponent<PartsInformation>().Parts.power == newParts.GetComponent<PartsInformation>().Parts.power)
            {
                partsGet = false;
                logPrefab.transform.GetChild(0).GetComponent<Text>().text = partsName + "をドロップしましたが、既に持っているため獲得できませんでした。";
                activityLogPanel.AddLog(logPrefab);
                Debug.Log(newParts.GetComponent<PartsInformation>().Parts.partsType + "(" + newParts.GetComponent<PartsInformation>().Parts.power + ")" + "をドロップしましたが、既に持っているため獲得できませんでした。");

                break;
            }
        }

        if (partsGet)
        {
            partsContent.AddParts(newParts);
            logPrefab.transform.GetChild(0).GetComponent<Text>().text = partsName + "を獲得しました。";
            activityLogPanel.AddLog(logPrefab);
            Debug.Log(newParts.GetComponent<PartsInformation>().Parts.partsType + "(" + newParts.GetComponent<PartsInformation>().Parts.power + ")" + "を獲得しました。");
        }
    }

    public void AddItem(ItemInformation.ItemType itemType)
    {
        string itemName = null;

        if (itemType == ItemInformation.ItemType.EnergyTank)
        {
            itemName = "エネルギータンク";
        }
        else if (itemType == ItemInformation.ItemType.HighEnergyTank)
        {
            itemName = "高密度エネルギータンク";
        }
        else if (itemType == ItemInformation.ItemType.SmallRepairParts)
        {
            itemName = "修復パーツ(小)";
        }
        else if (itemType == ItemInformation.ItemType.MediumRepairParts)
        {
            itemName = "修復パーツ(中)";
        }
        else if (itemType == ItemInformation.ItemType.HighRepairParts)
        {
            itemName = "修復パーツ(大)";
        }
        else if (itemType == ItemInformation.ItemType.key)
        {
            itemName = "鍵";
        }

        foreach (GameObject item in items)
        {
            if (item.GetComponent<ItemInformation>().Item.itemType == itemType)
            {
                if (item.GetComponent<ItemInformation>().Item.maxQuantity > item.GetComponent<ItemInformation>().Item.quantity)
                {
                    itemContent.AddItem(itemType, 1);
                    logPrefab.transform.GetChild(0).GetComponent<Text>().text = itemName + "を獲得しました。";
                    activityLogPanel.AddLog(logPrefab);
                    Debug.Log(itemType + "を獲得しました。");
                }
                else
                {
                    logPrefab.transform.GetChild(0).GetComponent<Text>().text = itemName + "をドロップしましたが、手持ちがいっぱいで獲得できませんでした。";
                    activityLogPanel.AddLog(logPrefab);
                    Debug.Log(itemType + "をドロップしましたが、手持ちがいっぱいで獲得できませんでした。");
                }

                return;
            }
        }

        itemContent.AddItem(itemType, 1);

        Debug.Log(itemType + "を獲得");
    }
}
