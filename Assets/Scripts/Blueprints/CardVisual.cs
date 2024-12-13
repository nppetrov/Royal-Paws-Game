using UnityEngine;

namespace NinjaChompek {
    [CreateAssetMenu(fileName = "Card", menuName = "CatKings/CardVisual")]
    public class CardVisual : ScriptableObject
    {
        public string Name;
		public string CardTitle;
		public Sprite CardImage;
		public Color BackgroundColor;

		private void OnValidate()
		{
			Name = name;
		}
	}
}
