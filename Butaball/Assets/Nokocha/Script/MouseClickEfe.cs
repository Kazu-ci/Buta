using UnityEngine;

public class MouseClickEfe : MonoBehaviour
{
    public GameObject EffectPrefab;

    private Vector3 mousePos;
    void Start()
    {
        mousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Instantiate(EffectPrefab,mousePos,Quaternion.identity);
        }
    }
}
