using UnityEngine;
using UnityEngine.SceneManagement;

public class MainGameEnd : MonoBehaviour
{
    GameManager gameManager;
    private float gameTime = 0f;
<<<<<<< Updated upstream
<<<<<<< Updated upstream
    private float gameMaxTime = 120f;
=======
=======
>>>>>>> Stashed changes
    private float gameMaxTime = 1000f;
    private bool stopper;
>>>>>>> Stashed changes
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
