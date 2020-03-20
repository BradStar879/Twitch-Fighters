using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    private GameObject fighterOne;
    private GameObject fighterTwo;
    private float startingCameraZ;
    private float startingPlayerDistance;
    private Camera camera;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float playerDistance = Mathf.Abs(fighterTwo.transform.position.x - fighterOne.transform.position.x);
        transform.position = new Vector3((fighterOne.transform.position.x + fighterTwo.transform.position.x) / 2,
                                            transform.position.y, -4.1f - (playerDistance / 3f));
    }

    public void Init(GameObject fighterOne, GameObject fighterTwo)
    {
        camera = GetComponent<Camera>();
        this.fighterOne = fighterOne;
        this.fighterTwo = fighterTwo;
        startingCameraZ = transform.position.z;
        startingPlayerDistance = Mathf.Abs(fighterTwo.transform.position.x - fighterOne.transform.position.x);
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