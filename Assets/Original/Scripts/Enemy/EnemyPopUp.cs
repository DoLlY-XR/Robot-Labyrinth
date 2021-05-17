using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPopUp : MonoBehaviour
{
    public IndicateObject indicateObject;

    [SerializeField]
    private StageLevel stage;
    [SerializeField]
    private GameObject enemyPrefab;
    [SerializeField]
    private int popUpLimit = 2;
    [SerializeField]
    private float popUpWaitTime = 20f;
    [SerializeField]
    private int popUpCount = 0;
    [SerializeField]
    private float waitTime = 0f;

    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        Random.InitState(System.DateTime.Now.Millisecond);
        var randDestination = UnityEngine.Random.insideUnitCircle * 8;
        var destination = transform.position + new Vector3(randDestination.x, transform.position.y, randDestination.y);
        transform.eulerAngles = new Vector3(destination.x, transform.position.y, destination.z) - transform.position;

        var enemyInstance = Instantiate<GameObject>
                        (enemyPrefab, destination, transform.rotation);
        enemyInstance.GetComponent<EnemyController>().enemyPop = this;
        enemyInstance.GetComponent<EnemyStatus>().Lv = stage.lv + Random.Range(0, 3);
        indicateObject.Increase(enemyInstance);

        popUpCount++;

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.gameStatus == GameManager.GameStatus.Over || gameManager.gameStatus == GameManager.GameStatus.Clear)
        {
            Destroy(gameObject);
        }

        if (popUpCount < popUpLimit)
        {
            waitTime += Time.deltaTime;

            if (waitTime > popUpWaitTime)
            {
                Random.InitState(System.DateTime.Now.Millisecond);
                var randDestination = UnityEngine.Random.insideUnitCircle * 8;
                var destination = transform.position + new Vector3(randDestination.x, transform.position.y, randDestination.y);
                transform.eulerAngles = new Vector3(destination.x, transform.position.y, destination.z) - transform.position;

                var enemyInstance = Instantiate<GameObject>
                                (enemyPrefab, destination, transform.rotation);
                enemyInstance.GetComponent<EnemyController>().enemyPop = this;
                enemyInstance.GetComponent<EnemyStatus>().Lv = stage.lv + Random.Range(0, 3);
                indicateObject.Increase(enemyInstance);

                waitTime = 0f;
                popUpCount++;
            }
        }
    }

    public int PopUpCount
    {
        get { return popUpCount; }
        set { popUpCount = value; }
    }
}
