using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCameraControl : MonoBehaviour
{
    public GameObject player;
    public GameObject minimap;
    public Transform playerIcon;
    private Transform mapCamera;
    // Start is called before the first frame update
    void Start()
    {
        mapCamera = gameObject.transform;
        minimap.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (player && player.GetComponent<PlayerCharacter>().isAlive)
        {
            Vector3 newpos = mapCamera.position;
            newpos.x = player.transform.position.x;
            newpos.z = player.transform.position.z;
            mapCamera.position = newpos;

            Vector3 newangles = playerIcon.localEulerAngles;
            newangles.z = 360.0f - player.transform.localEulerAngles.y;
            playerIcon.localEulerAngles = newangles;
        }
    }

    public void ShowMap()
    {
        minimap.SetActive(!minimap.activeInHierarchy);
    }
}
