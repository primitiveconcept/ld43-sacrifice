namespace LetsStartAKittyCult
{
    using UnityEngine;


    [CreateAssetMenu]
    public class ItemData : ScriptableObject
    {
        [SerializeField]
        private string displayName;

        [SerializeField]
        private Sprite icon;

        [SerializeField]
        private AudioClip pickupClip;

        [SerializeField]
        private GameObject equipPrefab;

        [SerializeField]
        private int maxCount = 255;

        [SerializeField]
        private bool isUsable = true;

        [SerializeField]
        private bool isWeapon = false;

        [SerializeField]
        private bool isConsumed = true;

        [SerializeField]
        private float activationCooldown;

        [SerializeField]
        private ItemEntry[] recipe;


        #region Properties
        public float ActivationCooldown
        {
            get { return this.activationCooldown; }
        }


        public string DisplayName
        {
            get { return this.displayName; }
        }


        public GameObject EquipPrefab
        {
            get { return this.equipPrefab; }
        }


        public Sprite Icon
        {
            get { return this.icon; }
        }


        public bool IsConsumed
        {
            get { return this.isConsumed; }
        }


        public bool IsUsable
        {
            get { return this.isUsable; }
        }


        public bool IsWeapon
        {
            get { return this.isWeapon; }
        }


        public int MaxCount
        {
            get { return this.maxCount; }
        }


        public AudioClip PickupClip
        {
            get { return this.pickupClip; }
        }


        public ItemEntry[] Recipe
        {
            get { return this.recipe; }
        }
        #endregion
    }
}