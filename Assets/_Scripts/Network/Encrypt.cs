﻿// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 4.0.30319.1
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------
using System;
using System.Text;


namespace MyClient
{
	public class Encrypt
	{
		private IEncrypter encrypter=null;
		public Encrypt():this(new Encrypter_Base64())
		{
			
		}
		public Encrypt(IEncrypter iEncrypter)
		{
			this.encrypter=iEncrypter;
		}
		
		
		public IEncrypter Encrypter
		{
			get 
			{
				return encrypter;
			}
			set 
			{
				encrypter = value;
			}
		}
		
		public byte[] Encode(byte[] data)
		{
			return encrypter.Encode(data);
		}
		public byte[] Decode(byte[] data)
		{
			return encrypter.Decode(data);
		}
	}
	
	public interface IEncrypter
	{
		byte[] Encode(byte[] data);
		byte[] Decode(byte[] data);
	}
	
	public class Encrypter_Base64:IEncrypter
	{
		#region IEncrypter implementation
		
		public byte[] Encode (byte[] data)
		{
			return Encoding.UTF8.GetBytes( Convert.ToBase64String(data));
		}
		
		public byte[] Decode (byte[] data)
		{
			return Convert.FromBase64String(Encoding.UTF8.GetString(data).TrimEnd('\0'));
		}
		
		#endregion
	}
	
	
	
}

