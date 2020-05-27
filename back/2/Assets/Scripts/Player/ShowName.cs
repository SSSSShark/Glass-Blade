using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowName : MonoBehaviour
{
    private TextMesh PlayerName;
    // Start is called before the first frame update
    void Start()
    {
        this.PlayerName = this.GetComponentInParent<TextMesh>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 cameraDirection = Camera.main.transform.forward;
        cameraDirection.y = 0f;
        this.PlayerName.transform.rotation = Quaternion.LookRotation(cameraDirection);
    }
}
