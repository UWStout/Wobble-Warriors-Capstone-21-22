using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeAlpha : MonoBehaviour
{

    [SerializeField] private float NumOpacityReduced = 40f;//sets the number of times that the object will change it's transparency

    [SerializeField] private float DeleteAfterXSec = 4.0f;//sets the time it takes for the object to self delete

    [SerializeField] private int FadeInScalar = 10;//percent that the object spends fading in vs fading out

    [SerializeField] private float TimeBeforeFadeOut = 0.5f;

    private bool startFading = false;//determines when the object should start the fading away

    [SerializeField] private GameObject puddle;//contains the prefab to of the puddle it's effecting

    [SerializeField] private float BaseOpacity = 0.8f;//max transparency of the object

    bool failedfade = true;

    bool startPlanB = true;

    private bool Fadeout = false;

    private float transparency;//determines base transparency of the object

    public float GetNOpacityReduced()
    {
        return NumOpacityReduced;
    }

    public float getDeletionTime()
    {
        return DeleteAfterXSec;
    }

    public float getFadeInScalar()
    {
        return FadeInScalar;
    }

    public float getTimeBeforeFade()
    {
        return TimeBeforeFadeOut;
    }

    public bool getStartFading()
    {
        return startFading;
    }

    public float getBaseOpacity()
    {
        return BaseOpacity;
    }

    public float getOpacity()
    {
        return gameObject.GetComponent<Material>().color.a;
    }


    // Start is called before the first frame update
    void Start()//initiates the 2 coroutines
    {
        failedfade = false;
        
        

        transparency = 0;
        StartCoroutine(FadeIn());//occurs first. the object moves quickly from 0.4f transparency to 0.8 transparency and then stops running
        //StartCoroutine(FadeOut());//will start running after Fadein coroutine stops and slowly fade out the object from 0.8 to 0 transparency
        Destroy(gameObject, DeleteAfterXSec*2);
    }

    // Update is called once per frame
    void Update()
    {
        if(failedfade && startPlanB)
        {
            startPlanB = false;
            transparency = 0;
            StartCoroutine(FadeIn());
        }
    }

    private IEnumerator FadeIn()// moves object from 0.4 transparency to 0.8f
    {
        float iterator = DeleteAfterXSec / (NumOpacityReduced * FadeInScalar);//this will fade in the object in the first 10% of their life

        float Titerator = BaseOpacity / (NumOpacityReduced);//determines the rate of change in seconds per transparency change

        while (startFading == false)
        {


            ChangeOpac(transparency);//function that changes the opacity of the object by the variable transparency

            transparency += Titerator;
            yield return new WaitForSeconds(iterator);
            if(transparency > BaseOpacity)
            {
                startFading = true;

            }
            if (transparency > 0.8f)
            {
                startFading = true;
            }
        }
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        Fadeout = true;
        float iterator = DeleteAfterXSec / NumOpacityReduced;
        float Titerator = BaseOpacity / (NumOpacityReduced);
        yield return new WaitForSeconds(TimeBeforeFadeOut);
        while (gameObject != null)
        {
            
            

                

            ChangeOpac(transparency);

            transparency -= Titerator;
            
            yield return new WaitForSeconds(iterator);
            //Debug.Log(transparency);
            if(transparency <= 0.04f)
            {
                Destroy(puddle);

            }
        }
    }

    public void ChangeOpac(float Alpha)
    {
        Color oldColor = gameObject.GetComponent<Renderer>().material.color;

        Color newColor = new Color(oldColor.r, oldColor.g, oldColor.b, transparency);



        gameObject.GetComponent<Renderer>().material.color = newColor;


    }
}
