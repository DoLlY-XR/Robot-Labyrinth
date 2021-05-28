using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum GameStatus
    {
        StandBy,
        Start,
        Progress,
        Over,
        Clear
    }

    public GameStatus gameStatus;
    public GameObject systcemConsole;
    public Text[] systemConsoleText;
    public GameObject button;
    public RawImage[] robotCameraWindows = new RawImage[3];
    public float fadeSpeed = 0.01f;
    public ActivityLogUIManager activityLogPanel;
    public GameObject logPrefab;
    public AudioSource audioSource;

    private SoundManager soundManager;
    private float deltaTime = 0f;
    private bool soundFlag = false;
    private float colorValue;

    // Start is called before the first frame update
    void Start()
    {
        soundManager = GetComponent<SoundManager>();
        gameStatus = GameStatus.StandBy;
        systcemConsole.SetActive(false);
        deltaTime = 0f;
        colorValue = 0f;

        foreach (var window in robotCameraWindows)
        {
            window.color = new Color(colorValue, colorValue, colorValue);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameStatus == GameStatus.StandBy)
        {
            deltaTime += Time.deltaTime;

            if (deltaTime > 1f)
            {
                systemConsoleText[0].text = "ロボット&ラビリンス";
                systemConsoleText[0].color = Color.white;
                systemConsoleText[1].text = "システム起動";
                systcemConsole.SetActive(true);

                if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger))
                {
                    deltaTime = 0f;
                    gameStatus = GameStatus.Start;
                    soundManager.GameStart();
                    button.GetComponent<Image>().color = Color.gray;
                    systemConsoleText[1].color = Color.white;
                }
            }
        }
        else if (gameStatus == GameStatus.Start)
        {
            deltaTime += Time.deltaTime;

            colorValue += fadeSpeed;
            if (colorValue >= 1f)
            {
                colorValue = 1f;
            }

            foreach (var window in robotCameraWindows)
            {
                window.color = new Color(colorValue, colorValue, colorValue);
            }

            if (deltaTime > 3f)
            {
                deltaTime = 0f;
                gameStatus = GameStatus.Progress;
                systcemConsole.SetActive(false);
                button.GetComponent<Image>().color = Color.white;
                systemConsoleText[1].color = Color.black;
                logPrefab.transform.GetChild(0).GetComponent<Text>().text = "システムを起動しました。";
                activityLogPanel.AddLog(logPrefab);
                audioSource.Play();
            }

        }
        else if (gameStatus == GameStatus.Over)
        {
            if (soundFlag == false)
            {
                soundManager.GameOver();
                soundFlag = true;
            }

            deltaTime += Time.deltaTime;
            systemConsoleText[0].text = "ゲームオーバー...";
            systemConsoleText[0].color = Color.red;
            systemConsoleText[1].text = "リトライ";
            systcemConsole.SetActive(true);

            if (deltaTime > 3f)
            {
                button.SetActive(true);

                if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger))
                {
                    GameOver();
                }
            }
            else
            {
                button.SetActive(false);
            }
        }
        else if (gameStatus == GameStatus.Clear)
        {
            if (soundFlag == false)
            {
                soundManager.GameClear();
                soundFlag = true;
            }

            deltaTime += Time.deltaTime;
            systemConsoleText[0].text = "ゲームクリア！";
            systemConsoleText[0].color = Color.white;
            systemConsoleText[1].text = "リスタート";
            systcemConsole.SetActive(true);

            if (deltaTime > 3f)
            {
                button.SetActive(true);

                if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger))
                {
                    GameClear();
                }
            }
            else
            {
                button.SetActive(false);
            }
        }
    }

    void GameOver()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void GameClear()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
