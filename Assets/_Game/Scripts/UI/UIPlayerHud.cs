namespace LetsStartAKittyCult
{
	using UnityEngine;
	using UnityEngine.Serialization;
	using UnityEngine.UI;


	public class UIPlayerHud : MonoBehaviour
	{
		[SerializeField]
		private UIMeter affectionMeter;

		[Header("Score")]
		[SerializeField]
		private Text scoreText;

		[Header("Selected Item")]
		[SerializeField]
		private Image selectedItemIcon;

		[SerializeField]
		private Text selectedItemCount;

		[Header("Lives")]
		[SerializeField]
		[FormerlySerializedAs("livesMeter")]
		private RectTransform extraLifeMeter;
		
		
		public UIMeter AffectionMeter
		{
			get { return this.affectionMeter; }
		}


		public Text ScoreText
		{
			get { return this.scoreText; }
		}
		

		public void UpdateLives(int value)
		{
			int maxLives = this.extraLifeMeter.childCount;

			for (int i = maxLives; i > 0; i--)
			{
				var lifeIcon = this.extraLifeMeter.GetChild(i - 1);
				if (i > value)
					lifeIcon.gameObject.SetActive(false);
				else
					lifeIcon.gameObject.SetActive(true);
			}
		}
		

		public void ShowSelectedItem(ItemEntry itemEntry)
		{
			this.selectedItemIcon.sprite = itemEntry.ItemData.Icon;
			this.selectedItemCount.text = itemEntry.ItemData.IsConsumed
				? itemEntry.Count.ToString()
				: "";
		}


		public void UpdateScore(ItemEntry scoreItemEntry)
		{
			this.scoreText.text = scoreItemEntry.Count.ToString("D4") 
				+ " / " 
				+ scoreItemEntry.ItemData.MaxCount;
		}


		public void SetupExtraLifeMeter(ItemData extraLifeItemData)
		{
			foreach (RectTransform rectTransform in this.extraLifeMeter)
			{
				Destroy(rectTransform.gameObject);
			}

			GridLayoutGroup gridLayout = this.extraLifeMeter.GetComponent<GridLayoutGroup>();

			for (int i = 0; i < extraLifeItemData.MaxCount; i++)
			{
				GameObject imageObject = new GameObject("Extra Life " + (i + 1));
				imageObject.transform.SetParent(this.extraLifeMeter.transform);
				imageObject.transform.localScale = Vector3.one;

				Image extraLifeImage = imageObject.AddComponent<Image>();
				extraLifeImage.sprite = extraLifeItemData.Icon;
				
				extraLifeImage.preserveAspect = true;
			}
		}
		
	}
}