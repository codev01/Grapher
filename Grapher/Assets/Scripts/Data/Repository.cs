using System;
using System.Collections.Generic;
using System.Data;

using Grapher.Data.Database;

namespace Grapher.Data
{
	public class Repository
	{
		private readonly DbApi _dbApi;
		public Repository(DbApi dbApi)
		{
			_dbApi = dbApi;
		}

		/// <returns> Наименования имеющихся данных для графика </returns>
		public List<string> GetGraphNames()
		{
			try
			{
				DataTable dt = _dbApi.ExecuteQuery($"SELECT * FROM GRAPHS");

				List<string> graphs = new List<string>(dt.Rows.Count);

				for (int i = 0; i < dt.Rows.Count; i++)
					graphs.Add(dt.Rows[i]["NAME"].ToString());

				return graphs;
			}
			catch (Exception e)
			{
				throw new Exception("Ошибка при получении имён данных для графика", e);
			}
		}

		/// <returns> Данные для графика </returns>
		public List<float> GetTrend(string nameTrend)
		{
			try
			{
				DataTable dt = _dbApi.ExecuteQuery($"SELECT * FROM TRENDS WHERE NAME = '{nameTrend}'");

				List<float> values = new List<float>(dt.Rows.Count);

				for (int i = 0; i < dt.Rows.Count; i++)
					values.Add(Convert.ToSingle(dt.Rows[i]["VALUE"]));

				return values;
			}
			catch (Exception e)
			{
				throw new Exception("Ошибка при конвертации данных", e);
			}
		}
	}
}
