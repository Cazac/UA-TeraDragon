using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsController : MonoBehaviour
{

    public Animator CreditsAnimator;


    private void Awake()
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
            CreditsAnimator.speed = 10f;
        }
        else
        {
            CreditsAnimator.speed = 1;
        }


        if (Input.GetKey("escape"))
        {
            //Load into main game
            SceneManager.LoadScene("Main Menu");
        }
    }

    private IEnumerator CreditsPlay()
    {
        yield return new WaitForSeconds(2.5f);


        //Credit Slidshow
        CreditsAnimator.Play("Play");
    }

}
