using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] CharacterMovement character;
    [SerializeField] Slider healthBar;
    [SerializeField] Image fill;

    [SerializeField] Transform attachedJoint;

    private float beginingHealth;
    private Vector3 offset;

    private void Start()
    {
        //get health bounds and set slider accordingly
        beginingHealth = character.health;
        healthBar.maxValue = beginingHealth;
        healthBar.minValue = 0;
        healthBar.value = beginingHealth;
        offset = attachedJoint.position - transform.position;
        if (character.GetComponent<TestBossAI>())
        {
            character.GetComponent<TestBossAI>().healthbar = this;
        }
        if (character.GetComponent<AICharacter>())
        {
            character.GetComponent<AICharacter>().healthbar = this;
        }

    }

    public void setMaxValue(int maxVal)
    {
        healthBar.maxValue = maxVal;
        beginingHealth = maxVal;
    }

    void Update()
    {
        //face the camera and move to character
        transform.rotation = Camera.main.transform.rotation;
        transform.position = attachedJoint.position - offset;

        //find color based on current health
        float colorValue = (beginingHealth - healthBar.value) / beginingHealth;
        fill.color = new Color(colorValue, 1 - colorValue, 0, 1);

        //set value of slider
        healthBar.value = Mathf.Lerp(healthBar.value,character.health,12* Time.deltaTime);
    }
}
