using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Alien : MonoBehaviour
{
    public string alienName = "tmp";

    public Sprite image;

    public Sprite AlienFace;

    public AudioSource SoundEffect;

    public bool exist = false;

    public string information = "null";

    public string whatAnimation;

    public Sprite[] PartnerEffectImage = new Sprite[5];
}
