using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTagwithInput : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            gameObject.tag = "Fire";
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            gameObject.tag = "Water";
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            gameObject.tag = "Ice";
        }

        if (Input.GetKeyDown(KeyCode.M))
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
