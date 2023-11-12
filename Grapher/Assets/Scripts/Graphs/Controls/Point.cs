using System;

using UnityEngine;

namespace Grapher.Graphs.Controls
{
	public class Point : MonoBehaviour
	{
		public static event Action<string, bool> OnSetPointVisibility;
		public string Value;

		public void OnMouseEnter()
		{
			OnSetPointVisibility?.Invoke(Value, true);
		}
		public void OnMouseExit()
		{
			OnSetPointVisibility?.Invoke(Value, false);
		}
	}
}
