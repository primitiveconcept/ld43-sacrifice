namespace LetsStartAKittyCult
{
	using System.Collections;
	using UnityEngine;
	using LetsStartAKittyCult.Exensions.Physics;
	

	public class ItemPickup : MonoBehaviour,
							INestedSprite
	{
		private static GameObject poolObject;

		[SerializeField]
		private ItemEntry item;

		[SerializeField]
		private SpriteRenderer spriteRenderer;


		#region Properties
		public SpriteRenderer SpriteRenderer
		{
			get { return this.spriteRenderer; }
			set { this.spriteRenderer = value; }
		}
		#endregion


		public static ItemPickup CreateFromItemEntry(ItemEntry itemEntry)
		{
			if (poolObject == null)
			{
				Debug.Log("Creating ItemPickup blueprint.");
				poolObject = new GameObject("Item Pickup");
				poolObject.AddComponent<SpriteRenderer>();
				poolObject.AddComponent<CameraVisibleTrigger>();
				poolObject.SetupRigidbody();
				poolObject.AddComponent<ItemPickup>();
				poolObject.SetActive(false);
			}

			GameObject itemPickupObject = PoolManager.Spawn(poolObject);
			ItemPickup itemPickup = itemPickupObject.GetComponent<ItemPickup>();
			itemPickup.item = itemEntry;
			itemPickup.SpriteRenderer.sprite = itemEntry.ItemData.Icon;
			Destroy(itemPickupObject.GetComponent<Collider2D>());
			BoxCollider2D collider = itemPickupObject.AddComponent<BoxCollider2D>();
			collider.isTrigger = true;

			return itemPickup;
		}


		public void Awake()
		{
			this.spriteRenderer = GetComponent<SpriteRenderer>();
		}


		public void OnTriggerEnter2D(Collider2D other)
		{
			Actor actor = other.GetComponent<Actor>();
			if (actor == null)
				return;

			Inventory actorInventory = actor.Inventory;
			if (actorInventory == null)
				return;

			if (actorInventory.AcquireItem(this.item))
			{
				if (this.item.ItemData.PickupClip != null)
					AudioPlayer.Play(this.item.ItemData.PickupClip);
				PoolManager.Despawn(this.gameObject);
			}
		}


		public void Start()
		{
			StartCoroutine(SetupInvisibleTrigger());
		}


		private IEnumerator SetupInvisibleTrigger()
		{
			yield return null;

			CameraVisibleTrigger cameraVisibleTrigger = GetComponent<CameraVisibleTrigger>();
			if (cameraVisibleTrigger != null)
			{
				cameraVisibleTrigger.WhileInvisible.AddListener(
					() =>
						{
							Debug.Log("Despawned");
							PoolManager.Despawn(this.gameObject);
						});
			}
		}
	}
}