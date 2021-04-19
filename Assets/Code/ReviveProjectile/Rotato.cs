using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotato : MonoBehaviour
{
    // Start is called before the first frame update
    void Update()
    {
        transform.eulerAngles = new Vector3((transform.eulerAngles.x + 5) % 360, 0, 0);
    }
}
