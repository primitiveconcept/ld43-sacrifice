namespace LetsStartAKittyCult
{
    using System.Collections.Generic;
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

        private UIPlayerHud playerHud;
        private Inventory inventory;


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
            health.OnChanged.AddListener(healthState => this.playerHud.HealthMeter.UpdateMeter(healthState));

            this.playerHud.SetupExtraLifeMeter(this.extraLifeItemData);

            OnExtraLivesChanged(this.inventory[this.extraLifeItemData]);
            this.inventory.OnItemPickup.AddListener(OnInventoryChange);
            this.inventory.OnItemRemoval.AddListener(OnInventoryChange);

            foreach (ItemEntry item in this.inventory.Items)
                OnInventoryChange(item);
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


        public void OnDestroy()
        {
            Remove(this);
        }


        public void OnSelectedItemChanged(ItemEntry itemEntry)
        {
            this.playerHud.ShowSelectedItem(itemEntry);
        }


        private void OnExtraLivesChanged(ItemEntry itemEntry)
        {
            this.playerHud.UpdateLives(itemEntry.Count);
            // TODO: Handle gameover
        }


        private void OnInventoryChange(ItemEntry itemEntry)
        {
            if (itemEntry.ItemData == this.scoreItemData)
                OnScoreChanged(itemEntry);

            else if (itemEntry.ItemData == this.extraLifeItemData)
                OnExtraLivesChanged(itemEntry);
        }


        private void OnScoreChanged(ItemEntry itemEntry)
        {
            this.playerHud.UpdateScore(itemEntry);
        }
    }
}