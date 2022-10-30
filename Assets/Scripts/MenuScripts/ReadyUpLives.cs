using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadyUpLives : MonoBehaviour
{

    [SerializeField]
    private Director director;

    //lives
    [SerializeField]
    private Image numberOfLivesImage;
    [SerializeField]
    public Sprite[] lives = new Sprite[10]; //stores life images, element 1 = 1 total lives
    public int totalLives = 1; //default life image that is showing and total lives given to player


    // Start is called before the first frame update
    void Start()
    {
        numberOfLivesImage.sprite = lives[1];
        totalLives = 1;
        director = GameObject.Find("Director").GetComponent<Director>();
        director.SetExtraLives(totalLives);
    }


    public void IncreaseLives()
    {
        if (totalLives < lives.Length-1)
        {
            totalLives++;
            numberOfLivesImage.sprite = lives[totalLives];
            director.SetExtraLives(totalLives);
            director.setLivesCountImage(numberOfLivesImage);
        }
    }

    public void DecreaseLives()
    {
        if (totalLives > 1)
        {
            totalLives--;
            numberOfLivesImage.sprite = lives[totalLives];
            director.SetExtraLives(totalLives);
            director.setLivesCountImage(numberOfLivesImage);
        }
    }

}
