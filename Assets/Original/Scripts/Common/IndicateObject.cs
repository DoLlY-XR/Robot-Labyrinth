using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicateObject : MonoBehaviour
{
    [SerializeField]
    private Transform lightObj;
    [SerializeField]
    private Transform door;
    [SerializeField]
    private Transform player;
    [SerializeField]
    private float distance;
    [SerializeField]
    private List<GameObject> enemy;
    private bool flag = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < enemy.Count; i++)
        {
            if (Vector3.Distance(enemy[i].transform.position, player.position) > distance)
            {
                enemy[i].gameObject.SetActive(false);
            }
            else
            {
                enemy[i].gameObject.SetActive(true);
            }
        }

        if (Vector3.Distance(lightObj.position, player.position) > distance)
        {
            lightObj.gameObject.SetActive(false);
        }
        else
        {
            lightObj.gameObject.SetActive(true);
        }
    }

    public void Increase(GameObject obj)
    {
        enemy.Add(obj);
    }

    public void Decrease(GameObject obj)
    {
        enemy.Remove(obj);
    }
}
