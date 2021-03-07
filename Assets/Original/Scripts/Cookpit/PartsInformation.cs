using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartsInformation : MonoBehaviour
{
    public enum PartsType
    {
        Null,
        AttackParts,
        ShieldParts
    };

    [Serializable]
    public struct PartsInfo
    {
        public PartsType partsType;
        public int power;

        public PartsInfo(PartsType partsType, int power)
        {
            this.partsType = partsType;
            this.power = power;
        }
    }

    [SerializeField]
    private PartsInfo partsInfo;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (partsInfo.partsType == PartsType.AttackParts)
        {
            this.transform.Find("Name").GetComponent<Text>().text = "火力パーツ(+" + partsInfo.power + ")";
        }
        else if (partsInfo.partsType == PartsType.ShieldParts)
        {
            this.transform.Find("Name").GetComponent<Text>().text = "頑強パーツ(+" + partsInfo.power + ")";
        }
    }

    public PartsInfo Parts
    {
        get { return partsInfo; }
        set { partsInfo = value; }
    }
}
