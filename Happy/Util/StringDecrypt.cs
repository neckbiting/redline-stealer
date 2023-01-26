using System;
using System.Text;


public static class StringDecrypt
{

	private static string Xor(string input, string stringKey)
	{
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < input.Length; i++)
		{
			stringBuilder.Append(input[i] ^ stringKey[i % stringKey.Length]);
		}
		return stringBuilder.ToString();
	}


	private static string FromBase64(string base64str)
	{
		return StringDecrypt.BytesToStringConverted(Convert.FromBase64String(base64str));
	}


	private static string BytesToStringConverted(byte[] bytes)
	{
		return Encoding.UTF8.GetString(bytes);
	}

	public static string Decrypt(string b64, string stringKey)
	{
		string result;
		try
		{
			if (string.IsNullOrWhiteSpace(b64))
			{
				result = string.Empty;
			}
			else
			{
				result = StringDecrypt.FromBase64(StringDecrypt.Xor(StringDecrypt.FromBase64(b64), stringKey));
			}
		}
		catch
		{
			result = b64;
		}
		return result;
	}
}
