using System;
using System.Data;
using System.IO;

using Mono.Data.Sqlite;

using UnityEngine;

namespace Grapher.Data.Database
{
	public class DbApi
	{
		private const string _fileName = "Graphs.db";
		private static string _databasePath;
		private static SqliteConnection connection;
		private static SqliteCommand command;
		public DbApi()
		{
			_databasePath = _getDatabasePath();
		}

		private string _getDatabasePath()
		{
			return Path.Combine(Path.GetDirectoryName(Application.streamingAssetsPath), "Database/", _fileName);
		}

		private void _openConnection()
		{
			try
			{
				connection = new SqliteConnection("Data Source=" + _databasePath);
				command = new SqliteCommand(connection);
				connection.Open();
			}
			catch (Exception e)
			{
				throw new Exception("Ошибка при подключение к БД", e);
			}
		}

		private void _closeConnection()
		{
			connection.Close();
			command.Dispose();
		}

		/// <summary>
		/// Выполняет SQL запрос
		/// </summary>
		/// <param name="query"> Строка, представляющая SQL запрос </param>
		/// <returns> Таблица, представляющая результат выборки запроса </returns>
		/// <exception cref="Exception"></exception>
		public DataTable ExecuteQuery(string query)
		{
			try
			{
				_openConnection();
				SqliteDataAdapter adapter = new SqliteDataAdapter(query, connection);

				DataSet DS = new DataSet();
				adapter.Fill(DS);
				adapter.Dispose();
				return DS.Tables[0];
			}
			catch (Exception e)
			{
				throw new Exception($"Ошибка при выполнении запроса {nameof(ExecuteQuery)}", e);
			}
			finally
			{
				_closeConnection();
			}
		}
	}
}
