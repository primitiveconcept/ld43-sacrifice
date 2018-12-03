namespace LetsStartAKittyCult
{
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.Events;


    public class Player : MonoBehaviour
    {
        private static List<Player> players = new List<Player>();

        [SerializeField]
        private ItemData extraLifeItemData;

        [SerializeField]
        private ItemData scoreItemData;

        [SerializeField]
        private UnityEvent onLivesDepleted;

        [SerializeField]
        private KittyCult cult;

        private Interactable currentInteractable;
        private UIPlayerHud playerHud;
        private Inventory inventory;


        #region Properties
        public KittyCult Cult
        {
            get { return this.cult; }
        }
        #endregion


        public static int Add(Player player)
        {
            if (players.Contains(player))
                return players.IndexOf(player);

            players.Add(player);
            return players.Count - 1;
        }


        public static Player Get(int playerIndex = 0)
        {
            if (players.Count > playerIndex)
                return players[playerIndex];

            return null;
        }


        public static Player GetClosest(Transform transform)
        {
            if (players.Count < 1)
                return null;

            if (players.Count == 1)
                return players[0];

            Player nearestPlayer = players[0];
            float nearestPlayerDistance = Vector2.Distance(transform.position, nearestPlayer.transform.position);
            for (int i = 1; i < players.Count; i++)
            {
                Player player = players[i];
                float distance = Vector2.Distance(transform.position, player.transform.position);
                if (distance >= nearestPlayerDistance)
                    continue;

                nearestPlayer = player;
                nearestPlayerDistance = distance;
            }

            return nearestPlayer;
        }


        public static int GetPlayerNumber(GameObject playerGameObject)
        {
            Player player = playerGameObject.GetComponent<Player>();

            return players.IndexOf(player);
        }


        public static void Remove(Player player)
        {
            if (players.Contains(player))
                players.Remove(player);
        }


        public void AttachToPlayerHud()
        {
            this.playerHud = FindObjectOfType<UIPlayerHud>();

            Health health = GetComponent<Health>();
            //health.OnChanged.AddListener(healthState => this.playerHud.HealthMeter.UpdateMeter(healthState));

            //this.playerHud.SetupExtraLifeMeter(this.extraLifeItemData);
        }


        public void Awake()
        {
            Add(this);
            this.inventory = GetComponent<Inventory>();
            AttachToPlayerHud();
        }


        public void ConsumeExtraLife(int amount = 1)
        {
            ItemEntry extraLives = this.inventory.ConsumeItem(this.extraLifeItemData, amount);
            if (extraLives.Count < 1)
                if (this.onLivesDepleted != null)
                    this.onLivesDepleted.Invoke();
        }


        public void Interact(GameObject interactor)
        {
            if (this.currentInteractable != null)
                this.currentInteractable.Interact(interactor);
        }


        public void OnDestroy()
        {
            Remove(this);
        }


        public void OnSelectedItemChanged(ItemEntry itemEntry)
        {
            this.playerHud.ShowSelectedItem(itemEntry);
        }


        public void SetInteractable(Interactable interactable)
        {
            this.currentInteractable = interactable;
        }


        public void UnsetInteractable(Interactable interactable)
        {
            if (!this.currentInteractable == interactable)
                return;

            this.currentInteractable = null;
        }
    }
}