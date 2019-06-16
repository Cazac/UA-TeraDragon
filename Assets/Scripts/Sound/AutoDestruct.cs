using UnityEngine;
public class AutoDestruct : MonoBehaviour
{
    private void Update()
    {
        DestroySelf();
    }
    public void DestroySelf()
    {
        if (!this.GetComponent<AudioSource>().isPlaying)
        {
            DestroyImmediate(this.gameObject);
            //this.GetComponent<AudioSource>().clip = null;
        }
    }
}
