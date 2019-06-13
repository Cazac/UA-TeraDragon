using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class ParticleController : MonoBehaviour
{

    public Toggle thisToggle;
    public ParticleSystem[] boundsParticles;
    // Start is called before the first frame update
    void Start()
    {
        thisToggle = GetComponent<Toggle>();
        boundsParticles = new ParticleSystem[4];

        for (int i = 0; i < boundsParticles.Length; i++)
        {
            boundsParticles[i] = transform.GetChild(i).GetComponent<ParticleSystem>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (thisToggle.isOn)
        {
            for (int i = 0; i < boundsParticles.Length; i++)
            {
                if (!boundsParticles[i].isPlaying)
                {
                    boundsParticles[i].Play();
                }
                //Debug.Log(i.ToString() + " " + boundsParticles[i].isEmitting);
            }
        } else
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
}
