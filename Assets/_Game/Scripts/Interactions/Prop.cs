namespace LetsStartAKittyCult
{
    using UnityEngine;
    
    
    public class Prop : MonoBehaviour
    {
        [SerializeField]
        private GameObject targetObject;

        [SerializeField]
        private GameObject cameraTarget; 
        
        public void Teleport(GameObject interactor)
        {
            if (this.targetObject != null)
            {
                interactor.transform.position = this.targetObject.transform.position;
                Camera.main.transform.position = new Vector3(
                    this.cameraTarget.transform.position.x,
                    this.cameraTarget.transform.position.y,
                    Camera.main.transform.position.z);
            }
                
        }
    }
}