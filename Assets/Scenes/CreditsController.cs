using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsController : MonoBehaviour
{

    public Animator CreditsAnimator;


    void Awake()
    {
        FindObjectOfType<SoundManager>().PlaySpecificSound("Peace");
    }

    private void Start()
    {

        StartCoroutine(CreditsPlay());
    }

    private void Update()
    {
        if (Input.GetKey("space"))
        {
            CreditsAnimator.speed = 3f;
        }
        else
        {
            CreditsAnimator.speed = 1;
        }
    }

    private IEnumerator CreditsPlay()
    {
        yield return new WaitForSeconds(2.5f);





        //Credit Slidshow
        CreditsAnimator.Play("Play");
    }

}
