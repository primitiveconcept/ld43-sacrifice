namespace LetsStartAKittyCult
{
    using System;
    using UnityEngine;


    [Serializable]
    public class ItemEntry
    {
        [SerializeField]
        private ItemData itemData;

        [SerializeField]
        private int count;

        [SerializeField]
        private bool isDroppable = true;

        public event Action<ItemEntry> Changed;


        #region Properties
        public int Count
        {
            get { return this.count; }
            set
            {
                if (value == this.count
                    || this.count == this.itemData.MaxCount)
                    return;

                if (value < 0)
                    this.count = 0;
                else if (value > this.itemData.MaxCount)
                    this.count = this.itemData.MaxCount;
                else
                    this.count = value;

                Action<ItemEntry> changed = Changed;
                if (changed != null)
                    changed(this);
            }
        }


        public bool IsDroppable
        {
            get { return this.isDroppable; }
            set { this.isDroppable = value; }
        }


        public ItemData ItemData
        {
            get { return this.itemData; }
            set { this.itemData = value; }
        }
        #endregion
    }
}