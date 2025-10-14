using Unity.VisualScripting;
using UnityEngine;

public class MapGimmick : MonoBehaviour
{
    public GameObject minimeteor;
    GameManager gameManager;    
    private float gameTime = 0;

    void Awake()
    {
        gameManager = GameObject.Find("SceneManagerObj").gameObject.GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        gimmick();
    }

    public void gimmick()
    {
        if (gameManager != null&& gameManager.state ==GameManager.State.Ingame)
        {
            gameTime+=Time.deltaTime;

            //Debug.Log("time"+gameTime);

            if(gameTime > 10)
            {
                Instantiate(minimeteor);
                gameTime = 0;
            }
            

        }
    }

    
}
