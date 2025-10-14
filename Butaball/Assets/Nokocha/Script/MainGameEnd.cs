using UnityEngine;
using UnityEngine.SceneManagement;

public class MainGameEnd : MonoBehaviour
{
    GameManager gameManager;
    private float gameTime = 0f;
    private float gameMaxTime = 120f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        gameManager = GameObject.Find("SceneManagerObj").gameObject.GetComponent<GameManager>();
    }

    public void MainGameTimeCount()
    {
        if (gameManager != null && gameManager.state == GameManager.State.Ingame)
        {
            gameTime += Time.deltaTime;
            Debug.Log("Œo‰ßŽžŠÔF"+gameTime);
        }
    }

    public void MainGameEndCheck()
    {
        if(gameTime > gameMaxTime)
        {
            SceneManager.LoadScene("Result");
        }
    }

    // Update is called once per frame
    void Update()
    {
        MainGameTimeCount();
        MainGameEndCheck();
    }
}
