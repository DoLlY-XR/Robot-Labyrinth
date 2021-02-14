using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddForceBeam : MonoBehaviour
{
    //　ビームを飛ばす力
    [SerializeField]
    private float bulletPower = 500.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.GetComponent<Rigidbody>().AddForce(this.transform.forward * bulletPower);
    }
}
