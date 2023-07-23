using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class FadeInFromBlack : MonoBehaviour
{
    [SerializeField] float delay = 2;
    [SerializeField] float duration = 2.5f;
    float  t;
    Vignette vign;
    // Start is called before the first frame update
    void Start()
    {
        Volume vol = this.gameObject.GetComponent<Volume>();
        if(!vol.profile.TryGet<Vignette>(out vign))
        {
            Debug.Log("Could Not Find Vignette");
        }
        t = delay + duration;
    }

    // Update is called once per frame
    void Update()
    {
        vign.intensity.value = Mathf.Clamp(t / duration, 0, 1);
        t -= Time.deltaTime;
        if (t < 0)
        {
            Sunset();
        }
    }
    void Sunset()
    {
        vign.intensity.overrideState=false;
        GetComponent<FadeInFromBlack>().enabled = false;
    }
}
