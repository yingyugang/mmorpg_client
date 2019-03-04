using UnityEngine;

namespace BlueNoah
{
    public class DoNotDestroyGameObject : MonoBehaviour
    {
		private void Awake()
		{
            DontDestroyOnLoad(gameObject);
		}
	}
}
