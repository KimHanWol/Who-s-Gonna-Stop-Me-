using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    public char planetTheme;

    public string planetName;

    private double planetHealth;

    private double planetMaxHealth;

    public double getPlanetHealth()
    {
        return planetHealth;
    }

    public void setPlanetHealth(double newPlanetHealth)
    {
        planetHealth = newPlanetHealth;
    }

    public void setPlanetMaxHealth(double newPlanetMaxHealth)
    {
        planetMaxHealth = newPlanetMaxHealth;
    }

    public Sprite planetImage;

    public Sprite getPlanetImage()
    {
        return planetImage;
    }

    public Sprite[] beingDistoryed = new Sprite[2];

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
