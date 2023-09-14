using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    // Start is called before the first frame update

    public float TestHP;

    // Update is called once per frame
    void Update()
    {

        if (TestHP <= 0)
        {
            print("die!");
            Destroy(this.gameObject);
        }
    }
}
