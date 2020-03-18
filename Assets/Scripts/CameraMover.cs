using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    private GameObject playerOne;
    private GameObject playerTwo;
    private float startingCameraZ;
    private float startingPlayerDistance;
    private Camera camera;
    // Start is called before the first frame update
    void Start()
    {
        playerOne = GameObject.FindGameObjectWithTag("Fighter1");
        playerTwo = GameObject.FindGameObjectWithTag("Fighter2");
        startingCameraZ = transform.position.z;
        startingPlayerDistance = Mathf.Abs(playerTwo.transform.position.x - playerOne.transform.position.x);
    }

    // Update is called once per frame
    void Update()
    {
        float playerDistance = Mathf.Abs(playerTwo.transform.position.x - playerOne.transform.position.x);
        transform.position = new Vector3((playerOne.transform.position.x + playerTwo.transform.position.x) / 2,
                                            transform.position.y, -4.1f - (playerDistance / 3f));
    }

    public void Init()
    {
        camera = GetComponent<Camera>();
    }

    public void EnableCamera()
    {
        camera.enabled = true;
    }

    public void DisableCamera()
    {
        camera.enabled = false;
    }
}