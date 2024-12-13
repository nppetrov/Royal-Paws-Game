using UnityEngine;

namespace NinjaChompek {
    [CreateAssetMenu(fileName = "Card", menuName = "CatKings/Card")]
    public class Card : ScriptableObject
	{
		public string Name;
		[TextArea(8, 20)]
		public string Description;
		public bool SpecialCard = false;
		public bool GameOverCard = false;
		public bool ThankYouCard = false;

		public Character Character;
		public CardVisual CardVisual;

		public string SequenceName;
		public int SequenceOrder;

		public string ButtonLeftText;
		public string ButtonRightText;

		[Header("Stat changes on Left choice")]
		public int AuthorityLeft;
		public int MoneyLeft;
		public int ArmyLeft;
		public int PeopleLeft;
		public int SequenceNextLeft;

		[Header("Stat changes on Right choice")]
		public int AuthorityRight;
		public int MoneyRight;
		public int ArmyRight;
		public int PeopleRight;
		public int SequenceNextRight;
	}
}