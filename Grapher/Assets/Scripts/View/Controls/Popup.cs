using TMPro;

using UnityEngine;

namespace Grapher.View.Controls
{
	public class Popup : MonoBehaviour
	{
		public TextMeshProUGUI TextElement;

		public void Update()
		{
			transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y - TextElement.bounds.size.y - 20);
		}

		public void SetTextPopup(string text)
		{
			TextElement.text = text;
		}
		public void SetVisibility(bool visible)
		{
			gameObject.SetActive(visible);
		}
	}
}
