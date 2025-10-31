using UnityEngine;
using UnityEngine.SceneManagement;

public class MainGameEnd : MonoBehaviour
{
    GameManager gameManager;
    private float gameTime = 0f;
    private float gameMaxTime = 120f;
    private bool stopper;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        //gameManager = GameObject.Find("GameManager").gameObject.GetComponent<GameManager>();
        stopper = true;
    }
    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    public void MainGameTimeCount()
    {
        if (gameManager != null && gameManager.state == GameManager.State.Ingame)
        {
            gameTime += Time.deltaTime;
            //Debug.Log("経過時間："+gameTime);
        }
    }

    public void MainGameEndCheck()
    {
        if (stopper)
        {
            if (gameTime > gameMaxTime)
            {
                if(ScoreManager.Player1Score != ScoreManager.Player2Score)
                {
                    SceneLoaderWithFade.Instance.LoadSceneWithFade("Result");
                    stopper = false;
                }
                else
                {
                    SceneLoaderWithFade.Instance.LoadSceneWithFade("hikiwake");
                    stopper = false;
                }
                
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (gameManager != null)
        {
            Debug.Log("GameManagerの状態: " + gameManager.state);
        }


        MainGameTimeCount();
        MainGameEndCheck();

        Debug.Log("TimeScale: " + Time.timeScale);

    }
    public float GetRemainingTime()
    {
        return gameMaxTime - gameTime;
    }

    public void ResetGameTime()
    {
        gameTime = 0f;
        stopper = true;
        Debug.Log("MainGameEnd: 時間リセット");
    }
}
