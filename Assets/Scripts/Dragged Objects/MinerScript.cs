using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinerScript : MonoBehaviour
{
    public int level;
    public CrystalTile crystalTile;
    public float timer;

    private PlayerStats playerStats;

    // Start is called before the first frame update
    private void Start()
    {
        playerStats = FindObjectOfType<PlayerStats>();
    }

    // Update is called once per frame
    private void Update()
    {
        CrystalTimer();
    }


    private void CrystalTimer()
    {
        timer += level * Time.fixedDeltaTime;

        if(timer > 15)
        {
            //Remove Cost
            timer -= 15;

            //Get color
            CrystalColor gemColor = GetCrytals();

            //Add to player
            playerStats.AddCrystal(gemColor);

            //UI Popup
            PopupText(1, gemColor);
        }
    }

    public CrystalColor GetCrytals()
    {
        return crystalTile.ProduceRandomCrystal();
    }



    public void PopupText(float damage, CrystalColor gemColor)
    {
        //Normalize float vs int???
        int normalDamage = (int)damage;

        //Get From PLayer Stasts
        GameObject textPrefab = playerStats.popupGemText_Prefab;

        //Create
        GameObject popupText = Instantiate(textPrefab, transform.position, Quaternion.identity);

        Color color = Color.white;


        switch (gemColor)
        {
            case CrystalColor.RED:
                color = Color.red;
                break;
            case CrystalColor.YELLOW:
                color = Color.yellow;
                break;
            case CrystalColor.BLUE:
                color = Color.blue;
                break;
            case CrystalColor.GREEN:
                color = Color.green;
                break;
        }

        //Setup
        GemPopup gemPopup = popupText.GetComponent<GemPopup>();
        gemPopup.Setup(normalDamage, color);
    }
}
