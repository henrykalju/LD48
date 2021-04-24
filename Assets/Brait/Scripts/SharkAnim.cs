using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkAnim : MonoBehaviour
{

    private void FixedUpdate()
    {
        float rotation = transform.rotation.eulerAngles.z;
        int times = Mathf.FloorToInt(rotation / 360);
        if (rotation > 180)
        {
            rotation -= 360*(times+1);
        }
        else if (rotation < -180)
        {
            rotation += 360 * (times + 1);
        }

        if (rotation <= 90 && rotation >= -90)
        {
            transform.localScale = new Vector3(1,1,1);
        }
        else
        {
            transform.localScale = new Vector3(1, -1, 1);
        }
    }
}
