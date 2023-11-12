using System.Collections;
using System.Collections.Generic;

using Grapher.Graphs.Controls;

using UnityEngine;

namespace Grapher.Abstracts
{
	public abstract class Graph : MonoBehaviour
	{
		public uint RowsCount;
		public uint ColumnsCount;
		/// <summary>
		/// Смещение данных
		/// </summary>
		public int OffsetData = 0;
		public List<float> TrendData;
		public List<Point> Points { get; protected set; }

		public void UpdateGraph()
		{
			DrawClear();
			StartCoroutine(Draw());
		}
		public void FullUpdateGraph()
		{
			Clear();
			StartCoroutine(InitializeDraw());
			UpdateGraph();
		}
		protected abstract IEnumerator InitializeDraw();
		protected abstract IEnumerator Draw();
		protected abstract void Clear();
		protected abstract void DrawClear();
	}
}
