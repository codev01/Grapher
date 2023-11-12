using Grapher.Data;
using Grapher.Data.Database;

using Reflex.Core;

using UnityEngine;

public class GapherInstaller : MonoBehaviour, IInstaller
{
	public void InstallBindings(ContainerDescriptor descriptor)
	{
		descriptor.AddSingleton(typeof(DbApi));
		descriptor.AddSingleton(typeof(Repository));
	}
}
