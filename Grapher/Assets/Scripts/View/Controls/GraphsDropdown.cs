using System;
using System.Collections.Generic;
using System.Linq;

using TMPro;

using Unity.VisualScripting;

using UnityEngine;

namespace Grapher.View.Controls
{
	public class GraphsDropdown : MonoBehaviour, IInitializable
	{
		public List<string> Items;

		private Action<string> _actionItemSelected;

		public void Initialize()
		{
			var dropdown = GetComponent<TMP_Dropdown>();

			dropdown.options.Clear();
			foreach (var item in Items)
			{
				dropdown.options.Add(new TMP_Dropdown.OptionData(item));
			}

			_actionItemSelected?.Invoke(dropdown.options.First().text);
			dropdown.onValueChanged.AddListener(delegate { _actionItemSelected?.Invoke(dropdown.options[dropdown.value].text); });
		}

		public void Subscribe_ActionItemSelected(Action<string> actionItemSelected)
			=> _actionItemSelected += actionItemSelected;
	}
}
