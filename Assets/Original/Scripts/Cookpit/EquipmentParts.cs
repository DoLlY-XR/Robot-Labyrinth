using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentParts : MonoBehaviour
{
    public enum PartsType
    {
        AttackParts,
        ShieldParts
    };

    [Serializable]
    public struct PartsInfo
    {
        public GameObject parts;
        public PartsType partsType;
        public int power;
    }

    [SerializeField]
    private PartsInfo weaponParts;
    [SerializeField]
    private PartsInfo shieldParts;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        weaponParts.power = weaponParts.parts.GetComponent<PartsInformation>().Parts.power;
        weaponParts.parts.transform.Find("Name").GetComponent<Text>().text = "火力パーツ(+" + weaponParts.power + ")";
        shieldParts.power = shieldParts.parts.GetComponent<PartsInformation>().Parts.power;
        shieldParts.parts.transform.Find("Name").GetComponent<Text>().text = "頑強パーツ(+" + shieldParts.power + ")";
    }

    public PartsInfo WeaponInfo
    {
        get { return weaponParts; }
    }

    public PartsInfo ShieldInfo
    {
        get { return shieldParts; }
    }

    public int WeaponPower
    {
        get { return weaponParts.power; }
        set { weaponParts.power = value; }
    }

    public int ShieldPower
    {
        get { return shieldParts.power; }
        set { shieldParts.power = value; }
    }
}
