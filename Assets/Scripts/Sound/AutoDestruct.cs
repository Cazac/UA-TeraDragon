using UnityEngine;
public class AutoDestruct : MonoBehaviour
{
    private void Update()
    {
        DestroySelf();
    }
    public void DestroySelf()
    {
        if (!GameObject.FindObjectOfType<SoundManager>().isActiveAndEnabled)
        {
            DestroyImmediate(this.gameObject);
            //this.GetComponent<AudioSource>().clip = null;
        }
    }
}
