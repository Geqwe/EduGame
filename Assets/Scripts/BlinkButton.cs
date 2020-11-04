using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkButton : MonoBehaviour
{
    Image image;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        StartCoroutine("Blink");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Blink()
     {
         while (true)
         {
             switch(image.color.a.ToString())
             {
                 case "0":
                     image.color = new Color(image.color.r, image.color.g, image.color.b, 1);
                     //Play sound
                     yield return new WaitForSeconds(0.3f);
                     break;
                 case "1":
                     image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
                     //Play sound
                     yield return new WaitForSeconds(0.3f);
                     break;
             }
         }
     }
}
