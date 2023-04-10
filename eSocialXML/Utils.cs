using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSocialXML
{
	public static class Utils
	{
		public static string[] EstadosBrasileiros = new string[] { "AC",
							"AL",
							"AP",
							"AM",
							"BA",
							"CE",
							"DF",
							"ES",
							"GO",
							"MA",
							"MS",
							"MT",
							"MG",
							"PA",
							"PB",
							"PR",
							"PE",
							"PI",
							"RJ",
							"RN",
							"RS",
							"RO",
							"RR",
							"SC",
							"SP",
							"SE",
							"TO" };
		public static string CreateEvtMonitId(int type, string uniqueIdBase, DateTime timestamp)
			=> type == 1 ? $"ID{type}{uniqueIdBase}000000{timestamp.ToString("yyyyMMddHHmmss")}00001" :
				           $"ID{type}{uniqueIdBase}0000{timestamp.ToString("yyyyMMddHHmmss")}00001";
	}
}
