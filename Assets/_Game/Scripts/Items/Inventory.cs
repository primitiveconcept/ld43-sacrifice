namespace LetsStartAKittyCult
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Events;


    public class Inventory : MonoBehaviour
    {
        [SerializeField]
        private List<ItemEntry> items;

        [SerializeField]
        private int maxSlots = 1;

        [SerializeField]
        private Vector2 dropOffsetRange = new Vector2(1, 1);

        [SerializeField]
        private bool locked = false;

        [SerializeField]
        private ItemEntryEvent onItemPickup;

        [SerializeField]
        private ItemEntryEvent onItemRemoval;

        private HashSet<ItemData> discoveredItems = new HashSet<ItemData>();


        #region Properties
        public int Count
        {
            get { return this.items.Count; }
        }


        public HashSet<ItemData> DiscoveredItems
        {
            get { return this.discoveredItems; }
        }


        public List<ItemEntry> Items
        {
            get { return this.items; }
        }


        public bool Locked
        {
            get { return this.locked; }
            set { this.locked = value; }
        }


        public int MaxSlots
        {
            get { return this.maxSlots; }
            set { this.maxSlots = value; }
        }


        public ItemEntryEvent OnItemPickup
        {
            get { return this.onItemPickup; }
        }


        public ItemEntryEvent OnItemRemoval
        {
            get { return this.onItemRemoval; }
        }


        public ItemEntry this[ItemData itemData]
        {
            get
            {
                int index = GetIndexOf(itemData);
                if (index < 0)
                    return null;

                return this[index];
            }
        }


        public ItemEntry this[int index]
        {
            get { return this.items[index]; }
        }
        #endregion


        public bool AcquireItem(ItemEntry itemEntry)
        {
            return AcquireItem(itemEntry.ItemData, itemEntry.Count);
        }


        public bool AcquireItem(ItemData itemData, int amount)
        {
            if (this.locked)
                return false;

            ItemEntry existingEntry = this[itemData];

            int collected = 0;

            if (existingEntry != null)
            {
                collected = IncreaseItemCount(existingEntry, amount);
            }
            else
            {
                existingEntry = new ItemEntry() { ItemData = itemData, Count = amount };
                collected = AddNewItemEntry(existingEntry);
            }

            if (collected > 0)
            {
                if (!this.discoveredItems.Contains(itemData))
                    this.discoveredItems.Add(itemData);
                if (this.onItemPickup != null)
                    this.onItemPickup.Invoke(existingEntry);

                return true;
            }

            return false;
        }


        public bool CanCraft(ItemData itemData)
        {
            foreach (ItemEntry ingredient in itemData.Recipe)
                if (!Contains(ingredient.ItemData, ingredient.Count))
                    return false;

            return true;
        }


        public ItemEntry ConsumeItem(int index, int amount = 1)
        {
            ItemEntry item = this.items[index];
            item.Count -= amount;

            if (this.onItemRemoval != null)
                this.onItemRemoval.Invoke(item);

            if (item.Count < 1)
                this.items.RemoveAt(index);

            return item;
        }


        public ItemEntry ConsumeItem(ItemEntry itemEntry)
        {
            return ConsumeItem(itemEntry.ItemData, itemEntry.Count);
        }


        public ItemEntry ConsumeItem(ItemData itemData, int amount = 1)
        {
            int index = GetIndexOf(itemData);
            return ConsumeItem(index, amount);
        }


        public bool Contains(ItemEntry itemEntry)
        {
            return Contains(itemEntry.ItemData, itemEntry.Count);
        }


        public bool Contains(ItemData itemData, int count = 1)
        {
            ItemEntry foundItem = this[itemData];
            if (foundItem == null)
                return false;

            return foundItem.Count >= count;
        }


        public bool Craft(ItemData itemData)
        {
            if (!CanCraft(itemData))
                return false;

            foreach (ItemEntry ingredient in itemData.Recipe)
                ConsumeItem(ingredient);

            AcquireItem(itemData, 1);

            return true;
        }


        public void DropAll()
        {
            for (int i = 0; i < this.Count; i++)
                DropItem(i);
        }


        public void DropAll(bool removeFromInventory)
        {
            for (int i = 0; i < this.Count; i++)
                DropItem(i, removeFromInventory);
        }


        public void DropItem(int index, bool removeFromInventory = true)
        {
            ItemEntry itemEntry = this[index];

            if (!itemEntry.IsDroppable)
                return;

            ItemPickup pickup = ItemPickup.CreateFromItemEntry(itemEntry);
            Vector2 dropLocation = new Vector2(
                this.transform.position.x + UnityEngine.Random.Range(-this.dropOffsetRange.x, this.dropOffsetRange.x),
                this.transform.position.y + UnityEngine.Random.Range(-this.dropOffsetRange.y, this.dropOffsetRange.y)
            );

            pickup.transform.position = dropLocation;

            if (removeFromInventory)
            {
                if (this.onItemRemoval != null)
                    this.onItemRemoval.Invoke(itemEntry);

                this.items.RemoveAt(index);
            }
        }


        public int GetIndexOf(ItemData itemData)
        {
            for (int index = 0; index < this.Count; index++)
            {
                ItemEntry itemEntry = this[index];
                if (itemEntry.ItemData == itemData)
                    return index;
            }

            return -1;
        }


        private int AddNewItemEntry(ItemEntry itemEntry)
        {
            if (itemEntry.Count > itemEntry.ItemData.MaxCount)
                itemEntry.Count = itemEntry.ItemData.MaxCount;

            if (this.Count < this.maxSlots)
            {
                this.items.Add(itemEntry);
                return itemEntry.Count;
            }

            // Inventory slots already maxed out.
            return 0;
        }


        private int IncreaseItemCount(ItemEntry existingEntry, int amount)
        {
            if (existingEntry.Count < existingEntry.ItemData.MaxCount)
            {
                int previousAmount = existingEntry.Count;
                existingEntry.Count += amount;
                return existingEntry.Count - previousAmount;
            }

            // Item already maxed out.
            return 0;
        }


        [Serializable]
        public class ItemEntryEvent : UnityEvent<ItemEntry>
        {
        }
    }
}