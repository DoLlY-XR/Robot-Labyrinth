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
    public SoundManager console;
    public ActivityLogUIManager activityLogPanel;
    public GameObject logPrefab;

    [NonSerialized]
    public bool flag = false;               //このスクリプトを動かすフラグ

    private CanvasGroup resultCanvas;
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
        resultCanvas = GetComponent<CanvasGroup>();
        myStatus = player.GetComponent<MyStatus>();
    }

    // Update is called once per frame
    void Update()
    {
        if (flag)
        {
            transitionTime += Time.deltaTime;
            count++;
            resultCanvas.alpha = 1f;

            if (count == 1)
            {
                itemListUIManager = transition[1].GetComponent<ItemListUIManager>();
                itemInfo = itemListUIManager.items[itemListUIManager.number].GetComponent<ItemInformation>();
            }
            
            if (itemInfo.Item.upgradeItem != null)
            {
                upgradeItemInfo = itemInfo.Item.upgradeItem.GetComponent<ItemInformation>();
            }
            choice = transition[0].GetComponent<ChoiceUIManager>();
            
            if (descriptionField.number == 0)
            {
                if (count == 1)
                {
                    foreach (var item in itemListUIManager.items)
                    {
                        if (item.GetComponent<ItemInformation>().Item.itemType == upgradeItemInfo.Item.itemType)
                        {
                            if (item.GetComponent<ItemInformation>().Item.maxQuantity > item.GetComponent<ItemInformation>().Item.quantity)
                            {
                                console.Decision();

                                resultText.text = "変換が完了しました";
                                this.transform.Find("Quantity").GetComponent<Text>().text =
                                    itemInfo.Item.itemName + "× " + choice.quantity * itemInfo.Item.upgradeQuantity +
                                    "\n↓\n" +
                                    upgradeItemInfo.Item.itemName + "× " + choice.quantity;
                                itemInfo.ItemQuantity -= choice.quantity * itemInfo.Item.upgradeQuantity;

                                itemListUIManager.AddItem(item, choice.quantity, true);
                                logPrefab.transform.GetChild(0).GetComponent<Text>().text = itemInfo.Item.itemName + "×" + choice.quantity * itemInfo.Item.upgradeQuantity +
                                    "を" + upgradeItemInfo.Item.itemName + "× " + choice.quantity + "に変換しました。";
                                activityLogPanel.AddLog(logPrefab);
                            }
                            else
                            {
                                resultText.text = "変換に失敗しました";
                                this.transform.Find("Quantity").GetComponent<Text>().text =
                                    itemInfo.Item.itemName + "× " + choice.quantity * itemInfo.Item.upgradeQuantity +
                                    "\n↓\n" +
                                    upgradeItemInfo.Item.itemName + "× " + choice.quantity;

                                logPrefab.transform.GetChild(0).GetComponent<Text>().text = "手持ちがいっぱいで変換できませんでした。";
                                activityLogPanel.AddLog(logPrefab);
                            }

                            return;
                        }
                    }

                    foreach (var item in itemListUIManager.itemPrefabList)
                    {
                        if (item.GetComponent<ItemInformation>().Item.itemType == upgradeItemInfo.Item.itemType)
                        {
                            console.Decision();

                            resultText.text = "変換が完了しました";
                            this.transform.Find("Quantity").GetComponent<Text>().text =
                                itemInfo.Item.itemName + "× " + choice.quantity * itemInfo.Item.upgradeQuantity +
                                "\n↓\n" +
                                upgradeItemInfo.Item.itemName + "× " + choice.quantity;
                            itemInfo.ItemQuantity -= choice.quantity * itemInfo.Item.upgradeQuantity;


                            itemListUIManager.AddItem(item, choice.quantity, false);
                            logPrefab.transform.GetChild(0).GetComponent<Text>().text = itemInfo.Item.itemName + "×" + choice.quantity * itemInfo.Item.upgradeQuantity +
                                "を" + upgradeItemInfo.Item.itemName + "× " + choice.quantity + "に変換しました。";
                            activityLogPanel.AddLog(logPrefab);
                        }
                    }
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
                            console.Recovering();

                            myStatus.EnergyAmount = myStatus.MaxEnergyAmount;
                            itemInfo.ItemQuantity--;

                            logPrefab.transform.GetChild(0).GetComponent<Text>().text = "エネルギーを補充しました。";
                            activityLogPanel.AddLog(logPrefab);
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
                            console.Recovering();

                            myStatus.Hp += (int)((float)myStatus.MaxHp * 0.2f);
                            if (myStatus.Hp > myStatus.MaxHp)
                            {
                                myStatus.Hp = myStatus.MaxHp;
                            }
                            itemInfo.ItemQuantity--;

                            logPrefab.transform.GetChild(0).GetComponent<Text>().text = "HPを20%回復しました。";
                            activityLogPanel.AddLog(logPrefab);
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
                            console.Recovering();

                            myStatus.Hp += (int)((float)myStatus.MaxHp * 0.5f);
                            if (myStatus.Hp > myStatus.MaxHp)
                            {
                                myStatus.Hp = myStatus.MaxHp;
                            }
                            itemInfo.ItemQuantity--;

                            logPrefab.transform.GetChild(0).GetComponent<Text>().text = "HPを50%回復しました。";
                            activityLogPanel.AddLog(logPrefab);
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
                            console.Recovering();

                            myStatus.Hp = myStatus.MaxHp;
                            itemInfo.ItemQuantity--;

                            logPrefab.transform.GetChild(0).GetComponent<Text>().text = "HPを全回復しました。";
                            activityLogPanel.AddLog(logPrefab);
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
                    this.flag = false;
                    transition[0].GetComponent<ChoiceUIManager>().flag = false;
                    transition[1].GetComponent<ItemListUIManager>().flag = true;
                    transitionTime = 0f;
                    dTime = 0f;
                    count = 0;
                    choice.quantity = 0;
                }
            }
        }
        else
        {
            resultCanvas.alpha = 0f;
        }
    }
}
