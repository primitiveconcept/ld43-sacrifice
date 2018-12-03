namespace LetsStartAKittyCult
{
    using System.Collections.Generic;
    using UnityEngine;


    public class Player : MonoBehaviour
    {
        private static List<Player> players = new List<Player>();

        [SerializeField]
        private KittyCult cult;

        private Interactable currentInteractable;
        private UIPlayerHud playerHud;


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
        }


        public void Awake()
        {
            Add(this);
            AttachToPlayerHud();
        }


        public void CancelInteract(GameObject interactor)
        {
            if (this.currentInteractable != null)
                this.currentInteractable.CancelInteract(interactor);
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


        public void Update()
        {
            this.cult.DecreaseTime(GameTime.DeltaTime);
        }
    }
}