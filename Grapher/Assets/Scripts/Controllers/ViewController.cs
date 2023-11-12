using System;

using Grapher.View.Controls;

using UnityEngine;

namespace Grapher.Controllers
{
	/// <summary>
	/// Перечисления кнопок
	/// </summary>
	public enum ButtonsType
	{
		HightPlus,
		HightMinus,
		WidthPlus,
		WidthMinus,
		Update,
		Left,
		Right
	}

	/// <summary>
	/// Отлавливает события и Хранит независимые объекты интерфейса
	/// </summary>
	public class ViewController : MonoBehaviour
	{
		/// <summary>
		/// Объект, представляющий всплывающее окно
		/// </summary>
		public Popup Popup;
		/// <summary>
		/// Объект, представляющий раскрывающийся список
		/// </summary>
		public GraphsDropdown GraphsDropdown;

		private Action<ButtonsType> _actionButtonsClicked;

		/// <summary>
		/// Подписывает на события нажатия кнопок
		/// </summary>
		/// <param name="buttonClicked">
		/// Какая кнопка была нажата
		/// </param>
		public void Subscribe_ActionButtonsClicked(Action<ButtonsType> buttonClicked)
			=> _actionButtonsClicked += buttonClicked;

		public void OnClick_ButtonHightPlus()
		=> _actionButtonsClicked?.Invoke(ButtonsType.HightPlus);
		public void OnClick_ButtonWidthPlus()
			=> _actionButtonsClicked?.Invoke(ButtonsType.WidthPlus);

		public void OnClick_ButtonHightMinus()
			=> _actionButtonsClicked?.Invoke(ButtonsType.HightMinus);
		public void OnClick_ButtonWidthMinus()
			=> _actionButtonsClicked?.Invoke(ButtonsType.WidthMinus);

		public void OnClick_ButtonUpdate()
			=> _actionButtonsClicked?.Invoke(ButtonsType.Update);

		public void OnClick_ButtonLeft()
			=> _actionButtonsClicked?.Invoke(ButtonsType.Left);
		public void OnClick_ButtonRight()
			=> _actionButtonsClicked?.Invoke(ButtonsType.Right);
	}
}
