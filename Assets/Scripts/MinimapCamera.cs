using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour
{

    public GameObject player;
    public Camera MainCamera;
    public bool rotateWithPlayer = false;

    private void Awake()
    {
        if (player == null)
        {
            player = GameObject.Find("Player");
        }

        if (MainCamera == null)
        {
            MainCamera = Camera.main;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

        disableFog();

        setPosition();
        setRotation();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (player != null)
        {
            setPosition();

            if (rotateWithPlayer && MainCamera)
            {
                setRotation();
            }
        }
    }

    private void setPosition()
    {
        Vector3 newPos = player.transform.position;
        newPos.y = transform.position.y;
        transform.position = newPos;

    }

    private void setRotation()
    {
        transform.rotation = Quaternion.Euler(90.0f, MainCamera.transform.eulerAngles.y, 0.0f);
    }

    private void disableFog(){
        RenderSettings.fog = false;
    }
}
