namespace LetsStartAKittyCult
{
    using System;
    using System.Collections.Generic;
    using UnityEngine.Events;


    [Serializable]
    public class KittyCult
    {
        public string WorshippedGod;
        public List<Human> Members = new List<Human>();
        public List<Human> HumansSacrificed = new List<Human>();
        public float EndGameTimer = 10 * 60 * 1000;
        
        private int currentDay = 10;
        
        public UnityEvent OnTimeUp;
        public IntegerEvent OnDayEnded;
        public IntegerEvent OnCultMemberAdded;
        public IntegerEvent OnHumanSacrificed;
        

        #region Properties
        public int CatDaysLeft
        {
            get { return (int)(this.EndGameTimer / (60 * 1000) + 1); }
        }


        public int Glory
        {
            get { return this.HumansSacrificed.Count; }
        }
        #endregion


        public void DecreaseTime()
        {
            this.EndGameTimer -= GameTime.DeltaTime;

            if (CatDaysLeft < this.currentDay)
            {
                this.currentDay--;
                this.OnDayEnded.Invoke(this.currentDay);
            }
            
            if (this.EndGameTimer <= 0)
                this.OnTimeUp.Invoke();
        }


        public void AddCultMember(Human human)
        {
            this.Members.Add(human);
            this.OnCultMemberAdded.Invoke(this.Members.Count);
        }


        public void SacrificeHuman(Human human)
        {
            this.HumansSacrificed.Add(human);
            this.OnHumanSacrificed.Invoke(this.HumansSacrificed.Count);
        }
    }


    [Serializable]
    public class IntegerEvent : UnityEvent<int>
    {
    }
}