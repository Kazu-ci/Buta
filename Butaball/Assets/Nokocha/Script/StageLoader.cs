using UnityEngine;
using UnityEngine.SceneManagement;

public class StageLoader : MonoBehaviour
{
    public void LoadStage(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    void Start()
    {
               
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
