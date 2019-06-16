using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ParticleController : MonoBehaviour
{

    public Button thisButton;
    public ParticleSystem[] boundsParticles;

    private bool internalSwitch = false;
    // Start is called before the first frame update
    void Start()
    {
        thisButton = GetComponent<Button>();
        thisButton.onClick.AddListener(MenuButtonClickListener);
        boundsParticles = new ParticleSystem[4];

        for (int i = 0; i < boundsParticles.Length; i++)
        {
            boundsParticles[i] = transform.GetChild(i).GetComponent<ParticleSystem>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (internalSwitch)
        {
            for (int i = 0; i < boundsParticles.Length; i++)
            {
                if (!boundsParticles[i].isPlaying)
                {
                    boundsParticles[i].Play();
                }
                //Debug.Log(i.ToString() + " " + boundsParticles[i].isEmitting);
            }
        }

        else
        {
            for (int i = 0; i < boundsParticles.Length; i++)
            {
                if (boundsParticles[i].isPlaying)
                {
                    boundsParticles[i].Stop();
                }
                //Debug.Log(i.ToString() + " " + boundsParticles[i].isEmitting);
            }
        }
    }

    void MenuButtonClickListener()
    {
            internalSwitch = true;
    }
}
