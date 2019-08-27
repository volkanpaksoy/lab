
namespace VP.Connector.TwilioWrapper
{
	using System;
	using System.Configuration;
	using System.Reflection;
	using Twilio;

	public class SmsManager
	{
		/// <summary>
		/// Sends a text message to the provided number from the provided account
		/// </summary>
		/// <param name="to">Target phone number with country code (e.g: +4412345678)</param>
		/// <param name="message">Text message</param>
		/// <param name="from">The number bought from Twilio</param>
		/// <param name="accountSID">Twilio account SID</param>
		/// <param name="accountToken">Twilio account token</param>
		/// <returns></returns>
		public static Sms SendSms(string to, string message, string from, string accountSID, string accountToken)
		{
			TwilioRestClient client = new TwilioRestClient(accountSID, accountToken);
			SMSMessage sms = client.SendSmsMessage(from, to, message);
			return new Sms()
				{
					ApiVersion = sms.ApiVersion, 
					Direction = sms.Direction, 
					Status = sms.Status, 
					From =  sms.From,
					To = sms.To,
					Price =  sms.Price,
					Provider = "Twilio"
				};
		}
		
		/// <summary>
		/// Sends a text message to the provided number. Account info is read from the configuration file.
		/// </summary>
		/// <param name="to">Target phone number with country code (e.g: +4412345678)</param>
		/// <param name="message">Text message</param>
		/// <returns></returns>
		public static Sms SendSms(string to, string message)
		{
			Settings settings = ReadSmsSettings();
			if (settings.AccountSID == null)
			{
				throw new ArgumentNullException("AccountSID cannot be null");
			}

			if (settings.AccountToken == null)
			{
				throw new ArgumentNullException("AccountToken cannot be null");
			}

			if (settings.PhoneNumber == null)
			{
				throw new ArgumentNullException("PhoneNumber cannot be null");
			}

			return SendSms(to, message, settings.PhoneNumber, settings.AccountSID, settings.AccountToken);
		}

		/// <summary>
		/// Reads Twilio account settings from 
		/// </summary>
		/// <returns></returns>
		private static Settings ReadSmsSettings()
		{
			Settings settings = new Settings();
			string dllPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
			Configuration config = ConfigurationManager.OpenExeConfiguration(dllPath);
			if (!config.HasFile)
			{
				Assembly assembly = System.Reflection.Assembly.GetEntryAssembly();
				if (assembly != null)
				{
					string callerPath = assembly.Location;
					config = ConfigurationManager.OpenExeConfiguration(dllPath);
					if (!config.HasFile)
					{
						throw new ArgumentException("Configuration file could not be found.");
					}
				}
			}

			settings.AccountSID = config.AppSettings.Settings["AccountSID"].Value;
			settings.AccountToken = config.AppSettings.Settings["AccountToken"].Value;
			settings.PhoneNumber = config.AppSettings.Settings["PhoneNumber"].Value;

			return settings;
		}

		public void SaveSettings(string accountSID, string accountToken, string phoneNumber)
		{
			string dllPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
			Configuration config = ConfigurationManager.OpenExeConfiguration(dllPath);
			AppSettingsSection app = config.AppSettings;
			app.Settings["AccountSID"].Value = accountSID;
			app.Settings["AccountToken"].Value = accountToken;
			app.Settings["PhoneNumber"].Value = phoneNumber;
			config.Save(ConfigurationSaveMode.Modified);
			ConfigurationManager.RefreshSection("appSettings");
		}

	}
}
