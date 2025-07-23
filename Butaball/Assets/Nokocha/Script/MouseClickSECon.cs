using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;

public class MouseClickSECon : MonoBehaviour
{
    private AudioSource Click_;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Click_ = GetComponent<AudioSource>();
    }

    public void OnButtonClick()
    {
        Click_.Play();
        Debug.Log("Sound");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
