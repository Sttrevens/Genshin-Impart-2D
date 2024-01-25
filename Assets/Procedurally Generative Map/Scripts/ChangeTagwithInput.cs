using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTagwithInput : MonoBehaviour
{
    public float yDeviation = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            gameObject.tag = "Fire";
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            gameObject.tag = "Water";
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            gameObject.tag = "Ice";
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            gameObject.tag = "Grass";
        }

        /*        if (Input.GetKeyDown(KeyCode.F))
                {
                    Interact();
                }*/

        /*        if (elementText != null)
                {
                    elementText.text = gameObject.tag;
                }*/
    }
}
