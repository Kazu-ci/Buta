using Unity.VisualScripting;
using UnityEngine;

public class MapGimmick : MonoBehaviour
{
    public GameObject minimeteor;

    GameManager gameManager;
    //InGameManager.GameState gameState;
    
    private float gameTime = 0;

    void Awake()
    {
        gameManager = GameObject.Find("SceneManagerObj").gameObject.GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        meteor();
    }

    public void meteor ()
    {
        if (gameManager != null&& gameManager.state ==GameManager.State.Ingame)
        {
            gameTime+=Time.deltaTime ;
            Debug.Log("time"+gameTime);
        }
        else
        {
            Debug.Log("GameManager has an error");
        }
            
    }



}
