using System;
using System.Runtime.Serialization;


[DataContract(Name = "SystemHardware", Namespace = "BrowserExtension")]
public class SystemHardware
{

	[DataMember(Name = "Name")]
	public string Name { get; set; }


	[DataMember(Name = "Counter")]
	public string Counter { get; set; }


	[DataMember(Name = "HardType")]
	public HardwareType HardType { get; set; }
}
