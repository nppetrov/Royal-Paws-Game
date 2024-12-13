using UnityEngine;

namespace NinjaChompek
{
	public class Manager<T> : MonoBehaviour where T : MonoBehaviour
	{
		private static T _instance;

		public static T Instance
		{
			get
			{
				return _instance;
			}
		}

		protected virtual void Awake()
		{
			if (_instance == null)
			{
				_instance = this as T;
			}
			else if (_instance != this)
			{
				Destroy(gameObject);
			}
		}
	}
}
