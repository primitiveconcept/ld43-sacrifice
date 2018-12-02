namespace LetsStartAKittyCult
{
    using System;
    using UnityEngine;


    public class Cat : MonoBehaviour
    {
        [Header("Blessing")]
        [SerializeField]
        private float blessAmount = 0.35f;

        [SerializeField]
        private float blessingCooldown = 1f;

        [SerializeField]
        private int equippedItemIndex;

        [SerializeField]
        private int equippedWeaponIndex;

        [SerializeField]
        private Transform attackOrigin;

        [SerializeField]
        private Transform useItemOrigin;

        [SerializeField]
        private Inventory.ItemEntryEvent onSelectedItemChanged;

        private Inventory inventory;
        private Weapon equippedWeapon;
        private UsableItem equippedItem;
        private SpriteSorter spriteSorter;

        private float blessingCooldownTimer;
        private bool isBlessing;

        private float weaponCooldown;
        private float itemCooldown;


        #region Properties
        public UsableItem EquippedItem
        {
            get { return this.equippedItem; }
        }


        public Weapon EquippedWeapon
        {
            get { return this.equippedWeapon; }
        }


        public Inventory Inventory
        {
            get { return this.inventory; }
        }


        public bool IsBlessing
        {
            get { return this.isBlessing; }
        }


        public SpriteSorter SpriteSorter
        {
            get { return this.spriteSorter; }
        }
        #endregion


        public void ActivateBlessing(Human human)
        {
            if (this.isBlessing)
                return;

            human.AddBlessing(this.blessAmount);
            this.isBlessing = true;
            this.blessingCooldownTimer = this.blessingCooldown;
        }


        public void Awake()
        {
            this.spriteSorter = GetComponent<SpriteSorter>();
            this.inventory = GetComponent<Inventory>();

            //EquipWeapon(this.equippedWeaponIndex);
            //EquipItem(this.equippedItemIndex);
        }


        public bool EquipItem(int inventoryIndex)
        {
            if (this.inventory.Count < 1)
            {
                this.equippedItemIndex = -1;
                this.equippedItem = null;
                return false;
            }

            if (inventoryIndex >= this.inventory.Count)
                return false;

            ItemEntry item = this.inventory[inventoryIndex];

            if (item == null
                || !item.ItemData.IsUsable)
                return false;

            this.equippedItemIndex = inventoryIndex;

            SwapItemPrefab(item);
            this.equippedItemIndex = inventoryIndex;

            if (this.onSelectedItemChanged != null)
                this.onSelectedItemChanged.Invoke(item);

            return true;
        }


        public void EquipNextItem()
        {
            for (int i = 0; i <= this.inventory.Count; i++)
            {
                this.equippedItemIndex++;
                if (this.equippedItemIndex > this.inventory.Count)
                    this.equippedItemIndex = 0;

                if (EquipItem(this.equippedItemIndex))
                    return;
            }

            this.equippedItemIndex = -1;
            this.equippedItem = null;
        }


        public void EquipNextWeapon()
        {
            for (int i = 0; i < this.inventory.Count; i++)
            {
                this.equippedWeaponIndex++;
                if (this.equippedWeaponIndex == this.inventory.Count)
                    this.equippedWeaponIndex = 0;

                if (EquipWeapon(this.equippedWeaponIndex))
                    return;
            }

            this.equippedWeaponIndex = -1;
            this.equippedWeapon = null;
        }


        public void EquipPreviousItem()
        {
            for (int i = 0; i < this.inventory.Count; i++)
            {
                this.equippedItemIndex--;
                if (this.equippedItemIndex < 0)
                    this.equippedItemIndex = this.inventory.Count - 1;

                if (EquipItem(this.equippedItemIndex))
                    return;
            }

            this.equippedItemIndex = -1;
            this.equippedItem = null;
        }


        public void EquipPreviousWeapon()
        {
            for (int i = 0; i < this.inventory.Count; i++)
            {
                this.equippedWeaponIndex--;
                if (this.equippedWeaponIndex < 0)
                    this.equippedWeaponIndex = this.inventory.Count - 1;

                if (EquipWeapon(this.equippedWeaponIndex))
                    return;
            }

            this.equippedWeaponIndex = -1;
            this.equippedWeapon = null;
        }


        // Getting tired, what even
        public bool EquipWeapon(int inventoryIndex)
        {
            if (this.inventory.Count < 1)
            {
                this.equippedWeaponIndex = -1;
                this.equippedWeapon = null;
                return false;
            }

            if (inventoryIndex >= this.inventory.Count)
                return false;

            ItemEntry item = this.inventory[inventoryIndex];

            if (item == null
                || !item.ItemData.IsWeapon)
                return false;

            this.equippedWeaponIndex = inventoryIndex;

            SwapWeaponPrefab(item);
            this.equippedWeaponIndex = inventoryIndex;

            return true;
        }


        public void Update()
        {
            if (this.blessingCooldownTimer > 0)
            {
                this.blessingCooldownTimer -= GameTime.DeltaTime;
                if (this.blessingCooldownTimer <= 0)
                    this.isBlessing = false;
            }

            if (this.weaponCooldown > 0)
                this.weaponCooldown -= GameTime.DeltaTime;

            if (this.itemCooldown > 0)
                this.itemCooldown -= GameTime.DeltaTime;
        }


        public void UseEquippedItem()
        {
            if (this.equippedItem == null
                || this.itemCooldown > 0)
                return;

            ItemEntry itemEntry = this.inventory[this.equippedItemIndex];

            Vector2 direction = this.spriteSorter.SpriteRenderer.flipX
                                    ? Vector2.left
                                    : Vector2.right;

            this.equippedItem.Use(this, direction);
            this.itemCooldown = itemEntry.ItemData.ActivationCooldown;

            if (itemEntry.ItemData.IsConsumed)
            {
                this.inventory.ConsumeItem(this.equippedItemIndex, 1);
                if (itemEntry.Count == 0)
                    EquipPreviousItem();
                else if (this.onSelectedItemChanged != null)
                    this.onSelectedItemChanged.Invoke(itemEntry);
            }
        }


        public void UseEquippedWeapon()
        {
            if (this.equippedWeapon == null
                || this.weaponCooldown > 0)
                return;

            ItemEntry itemEntry = this.inventory[this.equippedWeaponIndex];

            Vector2 direction = this.spriteSorter.SpriteRenderer.flipX
                                    ? Vector2.left
                                    : Vector2.right;

            this.equippedWeapon.Attack(direction);
            this.weaponCooldown = itemEntry.ItemData.ActivationCooldown;

            if (itemEntry.ItemData.IsConsumed)
            {
                this.inventory.ConsumeItem(this.equippedWeaponIndex, 1);
                if (itemEntry.Count == 0)
                    EquipPreviousWeapon();
            }
        }


        private void SwapItemPrefab(ItemEntry item)
        {
            if (this.equippedItem != null)
                PoolManager.Despawn(this.equippedItem.gameObject);

            GameObject itemObject = PoolManager.Spawn(item.ItemData.EquipPrefab);
            Pool itemPool = PoolManager.GetPool(itemObject);
            itemPool.transform.SetParent(this.useItemOrigin);
            itemPool.transform.localPosition = Vector3.zero;
            itemObject.transform.localPosition = Vector3.zero;

            UsableItem usableItem = itemObject.GetComponent<UsableItem>();
            this.equippedItem = usableItem;
        }


        private void SwapWeaponPrefab(ItemEntry item)
        {
            if (this.equippedWeapon != null)
                PoolManager.Despawn(this.equippedWeapon.gameObject);

            GameObject weaponObject = PoolManager.Spawn(item.ItemData.EquipPrefab);
            Pool weaponPool = PoolManager.GetPool(weaponObject);
            weaponPool.transform.SetParent(this.attackOrigin);
            weaponPool.transform.localPosition = Vector3.zero;
            weaponObject.transform.localPosition = Vector3.zero;

            Weapon weapon = weaponObject.GetComponent<Weapon>();
            this.equippedWeapon = weapon;
        }
    }
}