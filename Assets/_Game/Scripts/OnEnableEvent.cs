namespace LetsStartAKittyCult
{
    using UnityEngine;
    using UnityEngine.Events;


    public class OnEnableEvent : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent onEnableAction;


        private void OnEnable()
        {
            this.onEnableAction.Invoke();
        }       
    }
}