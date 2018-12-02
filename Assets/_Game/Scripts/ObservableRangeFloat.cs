namespace LetsStartAKittyCult
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;


    [Serializable]
    public abstract class ObservableRangeFloat : MonoBehaviour
    {
        [SerializeField]
        protected float current;

        [SerializeField]
        protected float min = 0;

        [SerializeField]
        protected float max = 100;

        [SerializeField]
        protected bool setToMaxOnStart;

        [SerializeField]
        protected ChangedEvent onChanged;


        #region Properties
        public float Current
        {
            get { return this.current; }
        }


        public float Max
        {
            get { return this.max; }
        }


        public float Min
        {
            get { return this.min; }
        }


        public ChangedEvent OnChanged
        {
            get { return this.onChanged; }
        }


        public bool SetToMaxOnStart
        {
            get { return this.setToMaxOnStart; }
            set { this.setToMaxOnStart = value; }
        }
        #endregion


        public float GetPercent()
        {
            float totalRange = this.Max - this.Min;
            float percent = this.Current / totalRange;
            return percent;
        }


        public virtual void Increase(float amount, bool forceEvent = false)
        {
            float newValue = this.current + amount;
            if (newValue > this.max)
                newValue = this.max;

            if (newValue == this.current)
            {
                if (forceEvent)
                    InvokeChanged();
                return;
            }

            this.current = newValue;
            InvokeChanged();
        }


        public void InvokeChanged()
        {
            if (this.onChanged != null)
                this.onChanged.Invoke(this);
        }


        public virtual void Reduce(float amount, bool forceEvent = false)
        {
            float newValue = this.current - amount;
            if (newValue < 0)
                newValue = 0;

            if (newValue == this.current)
            {
                if (forceEvent)
                    InvokeChanged();
                return;
            }

            this.current = newValue;
            InvokeChanged();
        }


        public virtual void SetCurrent(float value, bool forceEvent = false)
        {
            if (value == this.current)
            {
                if (forceEvent)
                    InvokeChanged();
                return;
            }

            this.current = value;
            InvokeChanged();
        }


        public virtual void SetMax(float value, bool forceEvent = false)
        {
            if (value < this.min)
                value = this.min;

            if (value == this.max)
            {
                if (forceEvent)
                    InvokeChanged();
                return;
            }

            this.max = value;
            InvokeChanged();
        }


        public virtual void SetMin(float value, bool forceEvent = false)
        {
            if (value > this.max)
                value = this.max;

            if (value == this.min)
            {
                if (forceEvent)
                    InvokeChanged();
                return;
            }

            this.min = value;
            InvokeChanged();
        }


        public virtual void Start()
        {
            if (this.setToMaxOnStart)
                this.current = this.max;

            InvokeChanged();
        }


        [Serializable]
        public class ChangedEvent : UnityEvent<ObservableRangeFloat>
        {
        }
    }
}