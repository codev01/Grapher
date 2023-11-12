
using System.Collections.Generic;
using System.Linq;

using Grapher.Controllers;
using Grapher.Data;
using Grapher.Graphs.Controls;

using Reflex.Attributes;

using UnityEngine;

namespace Grapher
{
	public class GrapherManager : MonoBehaviour
	{
		/// <summary>
		/// Частота обновления данных графика
		/// </summary>
		public float TimeUpdateGraphData_Seconds = 1;
		public ViewController ViewController;
		public GraphController GraphController;

		[Inject]
		private Repository _repository;
		private float _timeUpdateGraph_Seconds_Tick;
		private string _nameData;

		public void Start()
		{
			ViewController.Subscribe_ActionButtonsClicked(_view_ButtonClicked);
			_updateNames();
			Point.OnSetPointVisibility += _setPopup;
		}

		void Update()
		{
			_timeUpdateGraph_Seconds_Tick -= Time.deltaTime;
			if (_timeUpdateGraph_Seconds_Tick < 0)
			{
				_timeUpdateGraph_Seconds_Tick = TimeUpdateGraphData_Seconds;
				_updateData(_nameData);
			}
		}

		private void _updateNames()
		{
			List<string> names = _repository.GetGraphNames();
			ViewController.GraphsDropdown.Items = names;
			ViewController.GraphsDropdown.Subscribe_ActionItemSelected(_view_ItemSelected);
			ViewController.GraphsDropdown.Initialize();
		}

		private void _updateData(string nameData)
		{
			List<float> trend = _repository.GetTrend(nameData);

			if (!trend.All(GraphController.LineGraph.TrendData.Contains) && trend.Count != GraphController.LineGraph.TrendData.Count)
			{
				GraphController.LineGraph.TrendData = trend;
				GraphController.LineGraph.ColumnsCount = (uint)trend.Count;
				GraphController.LineGraph.RowsCount = (uint)trend.Distinct().Count();
				GraphController.LineGraph.OffsetData = 0;
				GraphController.LineGraph.FullUpdateGraph();
			}
		}

		private void _updateGraph()
		{
			GraphController.LineGraph.UpdateGraph();
		}

		private void _setPopup(string value, bool visibility)
		{
			ViewController.Popup.SetTextPopup(value);
			ViewController.Popup.SetVisibility(visibility);
		}

		private void _view_ItemSelected(string item)
		{
			_nameData = item;
			_updateData(item);
		}

		private void _view_ButtonClicked(ButtonsType btnType)
		{
			int h = (int)GraphController.LineGraph.RowsCount,
				w = (int)GraphController.LineGraph.ColumnsCount,
				o = GraphController.LineGraph.OffsetData;
			switch (btnType)
			{
				case ButtonsType.HightPlus:
					h++;
					break;
				case ButtonsType.HightMinus:
					h--;
					break;
				case ButtonsType.WidthPlus:
					w++;
					break;
				case ButtonsType.WidthMinus:
					w--;
					break;
				case ButtonsType.Update:
					_updateData(_nameData);
					break;
				case ButtonsType.Left:
					o--;
					break;
				case ButtonsType.Right:
					o++;
					break;
			}

			if (h > 0 && h < GraphController.LineGraph.TrendData.Distinct().Count())
				GraphController.LineGraph.RowsCount = (uint)h;
			if (w > 0 && w < GraphController.LineGraph.TrendData.Count)
				GraphController.LineGraph.ColumnsCount = (uint)w;
			if (o >= 0 && o < GraphController.LineGraph.TrendData.Count - GraphController.LineGraph.ColumnsCount)
				GraphController.LineGraph.OffsetData = o;

			if (GraphController.LineGraph.ColumnsCount + GraphController.LineGraph.OffsetData > GraphController.LineGraph.TrendData.Count)
				GraphController.LineGraph.OffsetData--;

			_updateGraph();
		}
	}
}
