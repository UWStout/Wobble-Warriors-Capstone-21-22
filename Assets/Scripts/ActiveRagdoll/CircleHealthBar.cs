using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircleHealthBar : MonoBehaviour
{
    [SerializeField] Transform mask;
    [SerializeField] Vector2 rotateRange;

    [SerializeField] Image tailSprite;
    [SerializeField] Image bodySprite;
    [SerializeField] Color initialColor;
    [SerializeField] Color finalColor;

    public HealthbarManager healthManager;
    public PlayerCharacter character;
    public float maxValue;
    public int playerNumber;

    public GameObject renderObject;

    void Start()
    {
        ReadyUpPlayer.updateUIHealth.AddListener(changeUIColor);
        healthManager = GetComponentInParent<HealthbarManager>();
    }

    void Update()
    {
        if (character)
        {
            renderObject.SetActive(true);
            mask.rotation = Quaternion.Euler(Vector3.Lerp(new Vector3(0, 0, rotateRange.x), new Vector3(0, 0, rotateRange.y), 1 - (character.health / maxValue)));
            tailSprite.color = Color.Lerp(initialColor, finalColor, 1 - (character.health / maxValue));
            tailSprite.transform.rotation = Quaternion.identity;

        }
        else
        {
            renderObject.SetActive(false);
        }
    }

    public void setMaxValue(float val)
    {
        maxValue = val;
    }

    void changeUIColor(Color newColor, int playNum)
    {
        if(playerNumber == playNum)
        {
            Color opaqueColor = new Vector4(newColor.r, newColor.g, newColor.b, 1);
            bodySprite.color = opaqueColor;
        }
    }
}
