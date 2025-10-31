using System.Drawing;
using UnityEngine;

public class result : MonoBehaviour
{
    public Renderer p1;
    public Renderer p2;
   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        EndGame();
        if(WinCon.WinPlayer)
        {
            ChangeColor(p2, UnityEngine.Color.blue);
            ChangeColor(p1, UnityEngine.Color.red);
            Debug.Log("色変更");
        }
        else
        {
            ChangeColor(p2, UnityEngine.Color.red);
            ChangeColor(p1, UnityEngine.Color.blue);
            Debug.Log("色変更");
        }

    }
    void ChangeColor(Renderer renderer, UnityEngine.Color color)
    {
        if (renderer != null)
        {
            renderer.material.color = color;
        }
    }

    private void EndGame()
    {
        //Escが押された時
        if (Input.GetKey(KeyCode.Escape))
        {

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
    Application.Quit();//ゲームプレイ終了
#endif
        }

    }
}
