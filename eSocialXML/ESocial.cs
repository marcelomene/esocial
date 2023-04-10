using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace eSocialXML
{
	[XmlRoot(ElementName = "ideEvento")]
	public class IdeEvento
	{
		[XmlElement(ElementName = "indRetif")]
		public string IndRetif { get; set; }
		[XmlElement(ElementName = "tpAmb")]
		public string TpAmb { get; set; }
		[XmlElement(ElementName = "procEmi")]
		public string ProcEmi { get; set; }
		[XmlElement(ElementName = "verProc")]
		public string VerProc { get; set; }
	}

	[XmlRoot(ElementName = "ideEmpregador")]
	public class IdeEmpregador
	{
		[XmlElement(ElementName = "tpInsc")]
		public string TpInsc { get; set; }
		[XmlElement(ElementName = "nrInsc")]
		public string NrInsc { get; set; }
	}

	[XmlRoot(ElementName = "ideVinculo")]
	public class IdeVinculo
	{
		[XmlElement(ElementName = "cpfTrab")]
		public string CpfTrab { get; set; }
	}

	[XmlRoot(ElementName = "exame")]
	public class Exame
	{
		[XmlElement(ElementName = "dtExm")]
		public string DtExm { get; set; }
		[XmlElement(ElementName = "procRealizado")]
		public string ProcRealizado { get; set; }
		[XmlElement(ElementName = "ordExame")]
		public string OrdExame { get; set; }
	}

	[XmlRoot(ElementName = "medico")]
	public class Medico
	{
		[XmlElement(ElementName = "nmMed")]
		public string NmMed { get; set; }
		[XmlElement(ElementName = "nrCRM")]
		public string NrCRM { get; set; }
		[XmlElement(ElementName = "ufCRM")]
		public string UfCRM { get; set; }
	}

	[XmlRoot(ElementName = "aso")]
	public class Aso
	{
		[XmlElement(ElementName = "dtAso")]
		public string DtAso { get; set; }
		[XmlElement(ElementName = "resAso")]
		public string ResAso { get; set; }
		[XmlElement(ElementName = "exame")]
		public List<Exame> Exame { get; set; }
		[XmlElement(ElementName = "medico")]
		public Medico Medico { get; set; }
	}

	[XmlRoot(ElementName = "respMonit")]
	public class RespMonit
	{
		[XmlElement(ElementName = "cpfResp")]
		public string CpfResp { get; set; }
		[XmlElement(ElementName = "nmResp")]
		public string NmResp { get; set; }
		[XmlElement(ElementName = "nrCRM")]
		public string NrCRM { get; set; }
		[XmlElement(ElementName = "ufCRM")]
		public string UfCRM { get; set; }
	}

	[XmlRoot(ElementName = "exMedOcup")]
	public class ExMedOcup
	{
		[XmlElement(ElementName = "tpExameOcup")]
		public string TpExameOcup { get; set; }
		[XmlElement(ElementName = "aso")]
		public Aso Aso { get; set; }
		[XmlElement(ElementName = "respMonit")]
		public RespMonit RespMonit { get; set; }
	}

	[XmlRoot(ElementName = "evtMonit")]
	public class EvtMonit
	{
		[XmlAttribute]
		public string Id { get; set; }
		[XmlElement(ElementName = "ideEvento")]
		public IdeEvento IdeEvento { get; set; }
		[XmlElement(ElementName = "ideEmpregador")]
		public IdeEmpregador IdeEmpregador { get; set; }
		[XmlElement(ElementName = "ideVinculo")]
		public IdeVinculo IdeVinculo { get; set; }
		[XmlElement(ElementName = "exMedOcup")]
		public ExMedOcup ExMedOcup { get; set; }

	}

	[XmlRoot(ElementName = "eSocial")]
	public class ESocial
	{
		[XmlElement(ElementName = "evtMonit")]
		public EvtMonit EvtMonit { get; set; }
		//[XmlAttribute(AttributeName = "xmlns")]
		//public string Xmlns { get; set; }
	}
}
