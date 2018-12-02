namespace LetsStartAKittyCult
{
    using UnityEngine;


    public class Human : MonoBehaviour
    {
        private Interactable interactable;
        private bool incapacitated;
        
        
        public void ReceiveBlessing(GameObject other)
        {
            Cat cat = other.GetComponent<Cat>();
            if (cat == null)
                return;
            
            cat.ActivateBlessing(this);
        }
    }
}