using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * @Author Anna Thiele
 * This script fades UI images from a set color to a target transparency
 **/
public class FadeInOut : MonoBehaviour
{
    //fade variables
    [SerializeField] private float fadeRate;
    [SerializeField] private float targetAlpha;
    [SerializeField] private Color curAlpha;

    //timer variables
    [SerializeField] private bool wait = false;
    [SerializeField] private float waitSec = 0.0f;
    private bool timer = false;
    private float time = 0.0f;


    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Image>().color = curAlpha;
    }
    private void Update()
    {
        time += Time.deltaTime;

        if (wait)
        {
            waitSec -= Time.deltaTime;
            if (waitSec <= 0.0f)
            {
                wait = false;
            }
        }
        if (!wait && !timer)
        {
            //Debug.Log("Start Fade");
            StartCoroutine(FadeIn());
            timer = true;
        }
    }

    IEnumerator FadeIn()
    {
        time = 0.0f;
        targetAlpha = 1.0f;
        Color curColor = gameObject.GetComponent<Image>().color;
        while (Mathf.Abs(curColor.a - targetAlpha) > 0.0001f)
        {
            //Debug.Log("enum time:" + time);
            //Debug.Log(curColor.a + "will change to " + targetAlpha);
            curColor.a = Mathf.Lerp(curColor.a, targetAlpha, fadeRate * Time.deltaTime);
            gameObject.GetComponent<Image>().color = curColor;
            if (time >= 3.0f)
            {
                //Debug.Log("break");
                time = 0.0f;
                break;
            }
            yield return null;
        }
    }
}
