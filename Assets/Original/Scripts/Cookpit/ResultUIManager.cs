using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class ResultUIManager : MonoBehaviour
{
    public GameObject player;
    public Text resultText;
    public GameObject[] transition;
    public DescriptionUIManager descriptionField;

    [NonSerialized]
    public bool flag = false;               //このスクリプトを動かすフラグ

    private MyStatus myStatus;
    private ItemListUIManager itemListUIManager;
    private ItemInformation itemInfo;
    private ItemInformation upgradeItemInfo;
    private ChoiceUIManager choice;
    private float transitionTime = 0f;
    private float dTime = 0f;
    private int count = 0;

    // Start is called before the first frame update
    void Start()
    {
        myStatus = player.GetComponent<MyStatus>();
    }

    // Update is called once per frame
    void Update()
    {
        if (flag)
        {
            transitionTime += Time.deltaTime;
            count++;

            itemListUIManager = transition[1].GetComponent<ItemListUIManager>();
            itemInfo = itemListUIManager.items[itemListUIManager.number].GetComponent<ItemInformation>();
            if (itemInfo.Item.upgradeItem != null)
            {
                upgradeItemInfo = itemInfo.Item.upgradeItem.GetComponent<ItemInformation>();
            }
            choice = transition[0].GetComponent<ChoiceUIManager>();
            
            if (descriptionField.number == 0)
            {
                resultText.text = "変換が完了しました";
                this.transform.Find("Quantity").GetComponent<Text>().text =
                    itemInfo.Item.itemName + "× " + choice.quantity * itemInfo.Item.upgradeQuantity +
                    "\n↓\n" +
                    upgradeItemInfo.Item.itemName + "× " + choice.quantity;

                if (count == 1)
                {
                    itemInfo.ItemQuantity -= choice.quantity * itemInfo.Item.upgradeQuantity;
                    upgradeItemInfo.ItemQuantity += choice.quantity;
                }
            }
            else if (descriptionField.number == 1)
            {
                if (itemInfo.Item.itemType == ItemInformation.ItemType.EnergyTank)
                {
                    if (myStatus.EnergyAmount < myStatus.MaxEnergyAmount)
                    {
                        if (count == 1)
                        {
                            myStatus.EnergyAmount = myStatus.MaxEnergyAmount;
                            itemInfo.ItemQuantity--;
                        }
                        resultText.text = "アイテムを使いました";
                        this.transform.Find("Quantity").GetComponent<Text>().text = "エネルギーを補充しました";
                    }
                    else
                    {
                        if (count == 1)
                        {
                            resultText.text = "アイテムを使えません";
                            this.transform.Find("Quantity").GetComponent<Text>().text = "エネルギーは満タンです";
                        }
                    }
                }
                else if (itemInfo.Item.itemType == ItemInformation.ItemType.HighEnergyTank)
                {
                    resultText.text = "アイテムを使えません";
                    this.transform.Find("Quantity").GetComponent<Text>().text = "コントローラ操作にて実行してください。";
                    dTime += Time.deltaTime;
                }
                else if (itemInfo.Item.itemType == ItemInformation.ItemType.SmallRepairParts)
                {
                    if (myStatus.Hp < myStatus.MaxHp)
                    {
                        if (count == 1)
                        {
                            myStatus.Hp += (int)((float)myStatus.MaxHp * 0.2f);
                            if (myStatus.Hp > myStatus.MaxHp)
                            {
                                myStatus.Hp = myStatus.MaxHp;
                            }
                            itemInfo.ItemQuantity--;
                        }
                        resultText.text = "アイテムを使いました";
                        this.transform.Find("Quantity").GetComponent<Text>().text = "HPを20%回復しました";
                    }
                    else
                    {
                        if (count == 1)
                        {
                            resultText.text = "アイテムを使えません";
                            this.transform.Find("Quantity").GetComponent<Text>().text = "HPは満タンです";
                        }
                    }
                }
                else if (itemInfo.Item.itemType == ItemInformation.ItemType.MediumRepairParts)
                {
                    if (myStatus.Hp < myStatus.MaxHp)
                    {
                        if (count == 1)
                        {
                            myStatus.Hp += (int)((float)myStatus.MaxHp * 0.5f);
                            if (myStatus.Hp > myStatus.MaxHp)
                            {
                                myStatus.Hp = myStatus.MaxHp;
                            }
                            itemInfo.ItemQuantity--;
                        }
                        resultText.text = "アイテムを使いました";
                        this.transform.Find("Quantity").GetComponent<Text>().text = "HPを50%回復しました";
                    }
                    else
                    {
                        if (count == 1)
                        {
                            resultText.text = "アイテムを使えません";
                            this.transform.Find("Quantity").GetComponent<Text>().text = "HPは満タンです";
                        }
                    }
                }
                else if (itemInfo.Item.itemType == ItemInformation.ItemType.HighRepairParts)
                {
                    if (myStatus.Hp < myStatus.MaxHp)
                    {
                        if (count == 1)
                        {
                            myStatus.Hp = myStatus.MaxHp;
                            itemInfo.ItemQuantity--;
                        }
                        resultText.text = "アイテムを使いました";
                        this.transform.Find("Quantity").GetComponent<Text>().text = "HPを全回復しました";
                    }
                    else
                    {
                        if (count == 1)
                        {
                            resultText.text = "アイテムを使えません";
                            this.transform.Find("Quantity").GetComponent<Text>().text = "HPは満タンです";
                        }
                    }
                }
            }
            
            if (transitionTime > 0.2f)
            {
                if (OVRInput.GetDown(OVRInput.RawButton.RHandTrigger) || OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger) || dTime > 3f)
                {
                    transition[1].GetComponent<ItemListUIManager>().flag = true;
                    this.flag = false;
                    transitionTime = 0f;
                    dTime = 0f;
                    count = 0;
                    choice.quantity = 0;
                    transition[0].SetActive(false);
                    this.gameObject.SetActive(false);
                }
            }
        }
    }
}
