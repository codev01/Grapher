using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Grapher.Abstracts;
using Grapher.Graphs.Controls;

using TMPro;

using UnityEngine;

namespace Grapher.Graphs
{
	public class LineGraph : Graph
	{
		#region Public fields
		/// <summary>
		/// �������� ��������
		/// </summary>
		public float SpeedAnimation = 60f;
		/// <summary>
		/// ������� ����� �����
		/// </summary>
		public float GridLineThickness = 0.01f;
		/// <summary>
		/// ������, �������������� ����� �� �������
		/// </summary>
		public LineRenderer Line;
		/// <summary>
		/// ������, �������������� ����� �� �������
		/// </summary>
		public Point Point;
		/// <summary>
		/// ������, �������������� ����� �������
		/// </summary>
		public TextMeshPro IndexText;
		/// <summary>
		/// ������, �������������� ������������ �������������� �����
		/// </summary>
		public GameObject LineGrid_X;
		/// <summary>
		/// ������, �������������� ������������ ������������ �����
		/// </summary>
		public GameObject LineGrid_Y;
		/// <summary>
		/// �������� ����� �����
		/// </summary>
		public Material GridMaterial;
		#endregion

		#region Private fields
		/// <summary>
		/// ������������ ��������� ��� ������������ ��������
		/// </summary>
		private GameObject _rootContainer;
		/// <summary>
		/// ��������� ��� �������������� �������� 
		/// </summary>
		private GameObject _drawContainer;

		float step_animation;
		float step_width;
		float step_height;
		float step_value;
		Vector3[] pointRowPositions;
		Vector3[] pointColumnPositions;
		#endregion

		public void Awake()
		{
			_initRootContainer();
		}

		private void _updateFields()
		{
			step_animation = SpeedAnimation * Time.deltaTime;
			step_width = LineGrid_X.transform.localScale.x / ColumnsCount;
			step_height = LineGrid_Y.transform.localScale.y / RowsCount;
			step_value = (LineGrid_Y.transform.localScale.y - step_height) / TrendData.Max();
		}

		protected override IEnumerator InitializeDraw()
		{
			_updateFields();
			Vector3 currentLineGrid_X_Position = LineGrid_X.transform.position;

			// ���������� ����� ����������� �� �����
			pointRowPositions = new Vector3[RowsCount];

			#region ������ � ��������� ��������� �������������� ����� �����
			for (int row = 0; row < RowsCount; row++)
			{
				GameObject newLineGrid_X = Instantiate(LineGrid_X, currentLineGrid_X_Position, Quaternion.identity);
				_addRootContainer(newLineGrid_X.transform);
				newLineGrid_X.GetComponent<Renderer>().material = GridMaterial;

				// ������� �������������� ����� �����
				Vector3 newLineGrid_X_Scale = newLineGrid_X.transform.localScale;
				newLineGrid_X.transform.localScale = new Vector3(newLineGrid_X_Scale.x, GridLineThickness, newLineGrid_X_Scale.z);

				// ������� ����� �������������� �����
				Vector3 newLineGrid_X_newPosition = new Vector3(currentLineGrid_X_Position.x, currentLineGrid_X_Position.y + step_height, currentLineGrid_X_Position.z);
				//newLineGrid_X.transform.position = Vector3.MoveTowards(newLineGrid_X.transform.position, newLineGrid_X_newPosition, step_height);

				// �������� ����������� �������������� �����
				while (Vector3.Distance(newLineGrid_X.transform.position, newLineGrid_X_newPosition) > 0)
				{
					newLineGrid_X.transform.position = Vector3.MoveTowards(newLineGrid_X.transform.position, newLineGrid_X_newPosition, step_animation * RowsCount);
					yield return null;
				}

				// ����� ������� �����
				Vector3 newTmpPosition = new Vector3(IndexText.transform.position.x, newLineGrid_X_newPosition.y, IndexText.transform.position.z);
				TextMeshPro tmp = Instantiate(IndexText, newTmpPosition, Quaternion.identity);
				_addRootContainer(tmp.transform);
				tmp.gameObject.SetActive(true);
				tmp.text = row.ToString();

				// ���������� ����� ����������� ��������
				pointRowPositions[row] = newLineGrid_X_newPosition;

				// ����� �������������� ����� ����� ������� �������������� ������
				currentLineGrid_X_Position = newLineGrid_X.transform.position;
			}
			#endregion
		}

		protected override IEnumerator Draw()
		{
			_updateFields();
			Points = new List<Point>((int)ColumnsCount);
			pointColumnPositions = new Vector3[ColumnsCount];
			Vector3 currentLineGrid_Y_Position = LineGrid_Y.transform.position;

			#region ������ � ��������� ��������� ������������ ����� �����
			for (int col = 0; col < ColumnsCount; col++)
			{
				GameObject newLineGrid_Y = Instantiate(LineGrid_Y, currentLineGrid_Y_Position, Quaternion.identity);
				_addDrawContainer(newLineGrid_Y.transform);
				newLineGrid_Y.GetComponent<Renderer>().material = GridMaterial;

				// ������� ������������ ����� �����
				Vector3 newLineGrid_Y_Scale = newLineGrid_Y.transform.localScale;
				newLineGrid_Y.transform.localScale = new Vector3(GridLineThickness, newLineGrid_Y_Scale.y, newLineGrid_Y_Scale.z);

				// ������� ����� ������������ �����
				Vector3 newLineGrid_Y_newPosition = new Vector3(currentLineGrid_Y_Position.x + step_width, currentLineGrid_Y_Position.y, currentLineGrid_Y_Position.z);
				//newLineGrid_Y.transform.position = Vector3.MoveTowards(newLineGrid_Y.transform.position, newLineGrid_Y_newPosition, step_width);

				// �������� ����������� ������������ �����
				while (Vector3.Distance(newLineGrid_Y.transform.position, newLineGrid_Y_newPosition) > 0)
				{
					newLineGrid_Y.transform.position = Vector3.MoveTowards(newLineGrid_Y.transform.position, newLineGrid_Y_newPosition, step_animation * ColumnsCount);
					yield return null;
				}

				// ����� ������� ��������
				Vector3 newTmpPosition = new Vector3(newLineGrid_Y_newPosition.x, IndexText.transform.position.y, IndexText.transform.position.z);
				TextMeshPro tmp = Instantiate(IndexText, newTmpPosition, Quaternion.identity);
				_addDrawContainer(tmp.transform);
				tmp.gameObject.SetActive(true);
				tmp.text = col.ToString();

				// ���������� ����� ����������� ��������
				pointColumnPositions[col] = newLineGrid_Y_newPosition;

				// ����� ������������ ����� ����� ������� ������������ ������
				currentLineGrid_Y_Position = newLineGrid_Y.transform.position;
			}
			#endregion

			#region ���������� ����� � ������ ����� ������� �� ���������
			for (int col = OffsetData; col < ColumnsCount + OffsetData; col++)
			{
				float start_position_Y = pointRowPositions[0].y;
				// ���������� ��������� ����� ����� �������
				Vector3 firstPointPosition_Y = new Vector3(pointColumnPositions[col - OffsetData].x, start_position_Y + TrendData[col] * step_value, Point.transform.position.z);
				Vector3 lastPointPosition_Y = firstPointPosition_Y;
				// ���������� ��� ��������� ��������� �����
				if (col < ColumnsCount + OffsetData - 1)
					lastPointPosition_Y = new Vector3(pointColumnPositions[col + 1 - OffsetData].x, start_position_Y + TrendData[col + 1] * step_value, Point.transform.position.z);

				// ����� ����� �� �������
				Point newPoint = Instantiate(Point, firstPointPosition_Y, Quaternion.identity);
				newPoint.gameObject.SetActive(true);
				newPoint.Value = TrendData[col].ToString();
				_addDrawContainer(newPoint.transform);
				Points.Add(newPoint);

				// ����� ����� �������
				LineRenderer newLine = Instantiate(Line, pointColumnPositions[col - OffsetData], Quaternion.identity);
				_addDrawContainer(newLine.transform);

				// �������� ����� �������
				while (Vector3.Distance(newLine.transform.position, lastPointPosition_Y) > 0)
				{
					newLine.transform.position = Vector3.MoveTowards(newPoint.transform.position, lastPointPosition_Y, step_animation * (Vector3.Distance(firstPointPosition_Y, lastPointPosition_Y) + ColumnsCount));
					newLine.SetPosition(0, firstPointPosition_Y);
					newLine.SetPosition(1, lastPointPosition_Y);
					yield return null;
				}

				newLine.SetPosition(0, firstPointPosition_Y);
				newLine.SetPosition(1, lastPointPosition_Y);
			}
			#endregion

			yield return null;
		}

		protected override void Clear()
		{
			_destroyRootContainer();
			_initRootContainer();
		}

		protected override void DrawClear()
		{
			_destroyDrawContainer();
			_initDrawContainer();
		}

		#region Containers
		private void _addRootContainer(Transform transform)
			=> transform.parent = _rootContainer.transform;

		private void _addDrawContainer(Transform transform)
			=> transform.parent = _drawContainer.transform;

		private void _initRootContainer()
		{
			_rootContainer = new GameObject("RootContainer");
			_initDrawContainer();
		}

		private void _destroyRootContainer()
		{
			_destroyDrawContainer();
			if (_rootContainer)
				Destroy(_rootContainer);
		}

		private void _initDrawContainer()
		{
			_drawContainer = new GameObject("DrawContainer");
			_drawContainer.transform.parent = _rootContainer.transform;
		}

		private void _destroyDrawContainer()
		{
			if (_drawContainer)
				Destroy(_drawContainer);
		}
		#endregion
	}
}
