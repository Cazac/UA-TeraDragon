    using UnityEngine;
    using UnityEditor.Animations;
    public class TowerAnimationController : MonoBehaviour
    {

        private const string EARTH = "EarthTower";
        private const string FIRE = "FireTower";
        private const string ICE = "IceTower";
        private const string LIGHT = "LightTower";


        private Animator animator;
        private void Awake() {
            animator = GetComponent<Animator>();
        }

       private void Start() 
        {
            string tagToSet = "";
            if(this.gameObject.tag == ICE)
                tagToSet = "Ice";
            else if(this.gameObject.tag == FIRE)
                tagToSet = "Fire";
            else if(this.gameObject.tag == EARTH)
                tagToSet = "Earth";
            else if(this.gameObject.tag == LIGHT)
                tagToSet = "Light";

            animator.SetTrigger(tagToSet);
        }
    }