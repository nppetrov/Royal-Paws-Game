using UnityEngine;

namespace NinjaChompek {
    [CreateAssetMenu(fileName = "Card", menuName = "CatKings/Character")]
    public class Character : ScriptableObject
    {
		public string Name;
		public CardVisual CardVisual;
	}
}
