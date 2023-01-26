using System;
using System.Runtime.Serialization;


[DataContract(Name = "Account", Namespace = "BrowserExtension")]
public class Account
{

	[DataMember(Name = "URL")]
	public string URL { get; set; }


	[DataMember(Name = "Username")]
	public string Username { get; set; }

	// Token: 0x17000023 RID: 35
	// (get) Token: 0x06000149 RID: 329 RVA: 0x00009A19 File Offset: 0x00007C19
	// (set) Token: 0x0600014A RID: 330 RVA: 0x00009A21 File Offset: 0x00007C21
	[DataMember(Name = "Password")]
	public string Password { get; set; }
}
