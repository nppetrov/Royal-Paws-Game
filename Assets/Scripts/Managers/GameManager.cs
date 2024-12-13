using UnityEngine;

namespace NinjaChompek
{
	[DefaultExecutionOrder(500)]
	public class GameManager : Manager<GameManager>
	{
		public enum StatType
		{
			Authority,
			Money,
			Army,
			People
		}

		public string PlayerName;
		public string PlayerGender;

		public int Authority = 50;
		public int Money = 50;
		public int Army = 50;
		public int People = 50;

		public int CurrentTurn = 0;

		public bool GameOver = false;

		public void StartNewGame()
		{
			GameOver = false;
			CurrentTurn = 0;
			Authority = 50;
			Money = 50;
			Army = 50;
			People = 50;

			UIManager.Instance.UpdateStats();

			CardManager.Instance.ResetAll();

			CardManager.Instance.LoadNextCard();

			AudioManager.Instance.PlayMusicClip("MusicLoop");
		}

		public void ChooseLeft()
		{
			AudioManager.Instance.PlaySound("Click");

			if (CardManager.Instance.CurrentCard.GameOverCard)
			{
				CardManager.Instance.LoadThankYouCard();
				return;
			}

			if (CardManager.Instance.CurrentCard.ThankYouCard)
			{
				StartNewGame();
				return;
			}

			CurrentTurn++;

			Army += CardManager.Instance.CurrentCard.ArmyLeft;
			Authority += CardManager.Instance.CurrentCard.AuthorityLeft;
			Money += CardManager.Instance.CurrentCard.MoneyLeft;
			People += CardManager.Instance.CurrentCard.PeopleLeft;

			NormalizeStats();

			UIManager.Instance.UpdateStats();

			if (!GameOver)
			{
				CardManager.Instance.LoadNextCard(CardManager.Instance.CurrentCard.SequenceNextLeft);
			}
		}

		public void ChooseRight()
		{
			AudioManager.Instance.PlaySound("Click");

			if (CardManager.Instance.CurrentCard.GameOverCard)
			{
				CardManager.Instance.LoadThankYouCard();
				return;
			}

			if (CardManager.Instance.CurrentCard.ThankYouCard)
			{
				StartNewGame();
				return;
			}

			CurrentTurn++;

			Army += CardManager.Instance.CurrentCard.ArmyRight;
			Authority += CardManager.Instance.CurrentCard.AuthorityRight;
			Money += CardManager.Instance.CurrentCard.MoneyRight;
			People += CardManager.Instance.CurrentCard.PeopleRight;

			NormalizeStats();

			UIManager.Instance.UpdateStats();

			if (!GameOver)
			{
				CardManager.Instance.LoadNextCard(CardManager.Instance.CurrentCard.SequenceNextRight);
			}
		}

		private void NormalizeStats()
		{
			Army = Mathf.Clamp(Army, 0, 100);
			Authority = Mathf.Clamp(Authority, 0, 100);
			Money = Mathf.Clamp(Money, 0, 100);
			People = Mathf.Clamp(People, 0, 100);

			if (Authority == 0)
			{
				CardManager.Instance.GameOver(StatType.Authority);
			}

			if (Money == 0)
			{
				CardManager.Instance.GameOver(StatType.Money);
			}

			if (Army == 0)
			{
				CardManager.Instance.GameOver(StatType.Army);
			}

			if (People == 0)
			{
				CardManager.Instance.GameOver(StatType.People);
			}
		}
	}
}
