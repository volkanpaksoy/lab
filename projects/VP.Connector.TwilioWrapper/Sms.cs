using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VP.Connector.TwilioWrapper
{
	public class Sms
	{
		public string Provider { get; set; }
		public string ApiVersion { get; set; }
		public string From { get; set; }
		public string To { get; set; }
		public string Direction { get; set; }
		public decimal Price { get; set; }
		public string Status { get; set; }
	}
}
