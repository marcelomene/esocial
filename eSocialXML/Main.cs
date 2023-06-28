using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace eSocialXML
{
	public partial class Main : Form
	{
		public List<Exame> Exames { get; set; }

		public ESocial ESocial { get; set; }

		private string IndRetifOpt1 = "1 - Informe Original";
		private string IndRetifOpt2 = "2 - Informe Retificação";

		private string TpAmbOpt1 = "1 - Produção";
		private string TpAmbOpt2 = "2 - Produção restrita";

		private string tpInscOpt1 = "1 - CNPJ";
		private string tpInscOpt2 = "2 - CPF";

		private string tpExameOcupOpt0 = "0 - Exame médico admissional";
		private string tpExameOcupOpt1 = "1 - Exame médico periódico, conforme NR7 do MTb e/ou planejamento do PCMSO";
		private string tpExameOcupOpt2 = "2 - Exame médico de retorno ao trabalho";
		private string tpExameOcupOpt3 = "3 - Exame médico de mudança de função";
		private string tpExameOcupOpt4 = "4 - Exame médico de monitoração pontual, não enquadrado nos demais casos";
		private string tpExameOcupOpt9 = "9 - Exame médico demissional";

		private string resAsoOpt1 = "1 - Apto";
		private string resAsoOpt2 = "2 - Inapto";

		private string ordExameOpt1 = "1 - Referencial";
		private string ordExameOpt2 = "2 - Sequencial";

#if LZ_PATOLOGIA
		private string CNPJBase = "05991664";
#endif

		public Main()
		{
			Exames = new List<Exame>();
			ESocial = new ESocial();

			InitializeComponent();

			examesLv.Columns.Clear();
			examesLv.Columns.Add("");
			examesLv.View = View.Details;
			examesLv.HeaderStyle = ColumnHeaderStyle.None;
			examesLv.Columns[0].Width = 700;

			IndRetifCb.Items.Add(IndRetifOpt1);
			IndRetifCb.Items.Add(IndRetifOpt2);
			IndRetifCb.SelectedIndex = 0;
			IndRetifCb.DropDownStyle = ComboBoxStyle.DropDownList;

			TpAmbCb.Items.Add(TpAmbOpt1);
			TpAmbCb.Items.Add(TpAmbOpt2);	
			TpAmbCb.SelectedIndex = 0;
			TpAmbCb.DropDownStyle = ComboBoxStyle.DropDownList;

			tpInscCb.Items.Add(tpInscOpt1);
			tpInscCb.Items.Add(tpInscOpt2);
			tpInscCb.DropDownStyle = ComboBoxStyle.DropDownList;
			tpInscCb.SelectedIndex = 0;

#if LZ_PATOLOGIA
			nrInscricaoTb.Text = CNPJBase;
			cnpjBaseTb.Text = CNPJBase;
#endif

			ufCrmRespCb.Items.AddRange(Utils.EstadosBrasileiros);
			ufCrmRespCb.SelectedIndex = Array.IndexOf(Utils.EstadosBrasileiros, "RS");
			ufCrmRespCb.DropDownStyle = ComboBoxStyle.DropDownList;

			tpExameOcupCb.Items.Add(tpExameOcupOpt0);
			tpExameOcupCb.Items.Add(tpExameOcupOpt1);
			tpExameOcupCb.Items.Add(tpExameOcupOpt2);
			tpExameOcupCb.Items.Add(tpExameOcupOpt3);
			tpExameOcupCb.Items.Add(tpExameOcupOpt4);
			tpExameOcupCb.Items.Add(tpExameOcupOpt9);
			tpExameOcupCb.DropDownStyle = ComboBoxStyle.DropDownList;

			resAsoCb.Items.Add(resAsoOpt1);
			resAsoCb.Items.Add(resAsoOpt2);
			resAsoCb.DropDownStyle = ComboBoxStyle.DropDownList;

			ordExameCb.Items.Add(ordExameOpt1);
			ordExameCb.Items.Add(ordExameOpt2);
			ordExameCb.DropDownStyle = ComboBoxStyle.DropDownList;
			ordExameCb.SelectedIndex = 1;

			ufCrm2Cb.Items.AddRange(Utils.EstadosBrasileiros);
			ufCrm2Cb.SelectedIndex = Array.IndexOf(Utils.EstadosBrasileiros, "RS");
			ufCrm2Cb.DropDownStyle = ComboBoxStyle.DropDownList;
		}

		private void AddExameBtn_Click(object sender, EventArgs e)
		{
			try
			{
				if (procRealizadoTb.Text == null || procRealizadoTb.Text.Length == 0 || !int.TryParse(procRealizadoTb.Text, out int val))
					throw new InvalidOperationException("Valor no campo Procedimentos Diagnósticos deve ser numérico e não pode ficar em branco.");

				Exame exame = new Exame()
				{
					DtExm = dataExameTp.Value.ToString("yyyy-MM-dd"),
					ProcRealizado = procRealizadoTb.Text,
					OrdExame = GetOrdExameValue((string)ordExameCb.SelectedItem).ToString(),
				};
				Exames.Add(exame);
				examesLv.Items.Add($"EXAME: PROCEDIMENTO - {exame.ProcRealizado} | DATA DO EXAME: {exame.DtExm} | ORDEM DO EXAME: {(string)ordExameCb.SelectedItem}\n");
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void generateXMLBt_Click(object sender, EventArgs e)
		{
			try
			{
				if (nrInscricaoTb.Text == null || (nrInscricaoTb.Text.Length != 11 && nrInscricaoTb.Text.Length != 8) || !int.TryParse(nrInscricaoTb.Text, out int ret))
					throw new InvalidOperationException("Verifique o valor informado no campo de número de inscrição (CPF ou CNPJ). Deve ser um valor somente numérico, sem " +
						"pontos ou traços. Deve ter 11 números para CPF e 8 números para CNPJ (somente o CNPJ base).");

				if (cpfTrabalhadorTb.Text == null || cpfTrabalhadorTb.Text.Length != 11 || !double.TryParse(cpfTrabalhadorTb.Text, out double ret2))
					throw new InvalidOperationException("Verifique o valor informado no campo de CPF do trabalhador. Deve ser um valor somente numérico, sem " +
						"pontos ou traços. Deve ter 11 números.");

				if (nCrm2Tb.Text == null || !int.TryParse(nCrm2Tb.Text, out int ret3))
					throw new InvalidOperationException("Verifique o valor informado no campo de Nº CRM do médico. Deve ser um valor somente numérico.");

				if (nrCrmRespTb.Text == null || !int.TryParse(nrCrmRespTb.Text, out int ret4))
					throw new InvalidOperationException("Verifique o valor informado no campo de Nº CRM do médico. Deve ser um valor somente numérico.");

				if (cpfRespTb.Text == null || cpfRespTb.Text.Length != 11 || !double.TryParse(cpfRespTb.Text, out double ret5))
					throw new InvalidOperationException("Verifique o valor informado no campo de CPF do responsável médico. Deve ser um valor somente numérico, sem " +
						"pontos ou traços. Deve ter 11 números.");

				//ESocial.Xmlns = "http://www.esocial.gov.br/schema/evt/evtMonit/v_S_01_01_00";
				ESocial.EvtMonit = new EvtMonit()
				{
					Id = Utils.CreateEvtMonitId(GetTpInscValue((string)tpInscCb.SelectedItem), nrInscricaoTb.Text, DateTime.Now),
					IdeEvento = new IdeEvento()
					{
						IndRetif = GetIndRetifValue((string)IndRetifCb.SelectedItem).ToString(),
						TpAmb = GetTpAmbValue((string)TpAmbCb.SelectedItem).ToString(),
						ProcEmi = "1", //Aplicativo do empregador
						VerProc = "S-1.0"
					},
					IdeEmpregador = new IdeEmpregador()
					{
						TpInsc = GetTpInscValue((string)tpInscCb.SelectedItem).ToString(),
						NrInsc = nrInscricaoTb.Text
					},
					IdeVinculo = new IdeVinculo() { CpfTrab = cpfTrabalhadorTb.Text },
					ExMedOcup = new ExMedOcup()
					{
						TpExameOcup = GetTpExameOcupValue((string)tpExameOcupCb.SelectedItem).ToString(),
						Aso = new Aso()
						{
							DtAso = dtAsoTp.Value.ToString("yyyy-MM-dd"),
							ResAso = GetResAsoValue((string)resAsoCb.SelectedItem).ToString(),
							Exame = Exames,
							Medico = new Medico()
							{
								NmMed = nmMedTb.Text,
								NrCRM = nCrm2Tb.Text,
								UfCRM = (string)ufCrm2Cb.SelectedItem
							}
						},
						RespMonit = new RespMonit()
						{
							CpfResp = cpfRespTb.Text,
							NmResp = nmRespTb.Text,
							NrCRM = nrCrmRespTb.Text,
							UfCRM = (string)ufCrmRespCb.SelectedItem
						}
					}
				};
				string fileName = $"S-2220-{ESocial.EvtMonit.Id}.xml";
				string fileDestination = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\{fileName}";

				saveFileDialog1.Title = "Salvar XML eSocial";
				saveFileDialog1.InitialDirectory = fileDestination;
				saveFileDialog1.FileName = fileName;

				if (saveFileDialog1.ShowDialog() == DialogResult.OK)
					fileDestination = saveFileDialog1.FileName;
				else
					throw new OperationCanceledException("Operação cancelada pelo usuário.");

				string defaultNamespace = "http://www.esocial.gov.br/schema/evt/evtMonit/v_S_01_01_00";
				XmlSerializer serializer = new XmlSerializer(typeof(ESocial), defaultNamespace);

				var xmlns = new XmlSerializerNamespaces();
				xmlns.Add("", defaultNamespace);

				//using (StreamWriter writer = new StreamWriter(fileDestination))
				//{
				//	serializer.Serialize(writer, ESocial, xmlns);
				//}
				XmlWriterSettings settings = new XmlWriterSettings() { Indent = false };
				using (XmlWriter writer = XmlWriter.Create(fileDestination, settings))
				{
					serializer.Serialize(writer, ESocial, xmlns);
				}

				MessageBox.Show("Arquivo gerado!", "Sucesso!", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message,"Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		#region Aux
		private int GetResAsoValue(string opt)
		{
			if (opt == resAsoOpt1)
				return 1;
			else
				return 2;
		}
		private int GetTpExameOcupValue(string opt)
		{
			if (opt == tpExameOcupOpt0)
				return 0;
			else if (opt == tpExameOcupOpt1)
				return 1;
			else if (opt == tpExameOcupOpt2)
				return 2;
			else if (opt == tpExameOcupOpt3)
				return 3;
			else if (opt == tpExameOcupOpt4)
				return 4;
			else if (opt == tpExameOcupOpt9)
				return 9;
			else
				return -1;
		}
		private int GetTpInscValue(string opt)
		{
			if (opt == tpInscOpt1)
				return 1;
			else
				return 2;
		}
		private int GetTpAmbValue(string opt)
		{
			if (opt == TpAmbOpt1)
				return 1;
			else
				return 2;
		}
		private int GetIndRetifValue(string opt)
		{
			if (opt == IndRetifOpt1)
				return 1;
			else
				return 2;
		}
		private int GetOrdExameValue(string opt)
		{
			if(opt == ordExameOpt1)
				return 1;
			else
				return 2;
		}
		#endregion

		private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
			=> System.Diagnostics.Process.Start("https://www.valor.srv.br/artigo.php?id=1255&titulo=tabela-27-esocial-procedimentos-diagnosticos");
	}
}
