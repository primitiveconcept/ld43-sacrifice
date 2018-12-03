namespace LetsStartAKittyCult
{
	using TMPro;
	using UnityEngine;
	

	public class UIPlayerHud : MonoBehaviour
	{
		[SerializeField]
		private TextMeshProUGUI gloryLabel;
		
		[SerializeField]
		private TextMeshProUGUI daysLeftLabel;

		[SerializeField]
		private TextMeshProUGUI livesLeftLabel;


		public void Start()
		{
			Player player = Player.Get();
			
			Health playerHealth = player.GetComponent<Health>();
			
			playerHealth.OnDepleted.AddListener(() => UpdateLives(playerHealth.Lives));
			player.Cult.OnDayEnded.AddListener(UpdateDaysLeft);
			player.Cult.OnHumanSacrificed.AddListener(UpdateGlory);
			
			UpdateLives(playerHealth.Lives);
			UpdateDaysLeft(player.Cult.CatDaysLeft);
			UpdateGlory(player.Cult.Glory);
		}
		

		public void UpdateGlory(int value)
		{
			this.gloryLabel.text = $"GLORY: {value} / 9";
		}
		
		public void UpdateLives(int value)
		{
			this.livesLeftLabel.text = $"LIVES: {value}";
		}
		
		public void UpdateDaysLeft(int value)
		{
			this.daysLeftLabel.text = $"CAT DAYS LEFT: {value}";
		}
	}
}