using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static System.Windows.Forms.LinkLabel;

namespace TrabalhoFinal
{
	public partial class Form1 : Form
	{
		private readonly Stopwatch stopwatch = new Stopwatch();
		private UdpClient usocketConexaoUDP;
		private IPEndPoint ipConexaoRecebimentoUDP;
		private IPEndPoint ipConexaoEnvioUDP;
		private string menssagemRecebida;
		private readonly SemaphoreSlim mutex = new SemaphoreSlim(1);
		private byte[] bytes = new byte[1024];
		private bool shouldStopRecebimento = false;
		int numeroPacote = 1;
		int numeroPacoted = 1;
		int flagA = 0;
		int flagB = 0;
		int flagC = 0;


		public Form1()
		{
			InitializeComponent();
			Control.CheckForIllegalCrossThreadCalls = false;

		}

		private void Iniciar_Click(object sender, EventArgs e)
		{
			IniciaConexoesUDP();
			GeraraPacotes();
		}

		private void Parar_Click(object sender, EventArgs e)
		{
			FechaConexoesUDP();
		}

		private void IniciaConexoesUDP()
		{
			usocketConexaoUDP = new UdpClient(12345);
			ipConexaoRecebimentoUDP = new IPEndPoint(IPAddress.Any, 12345);
			ipConexaoEnvioUDP = new IPEndPoint(IPAddress.Parse("255.255.255.255"), 12345);

			shouldStopRecebimento = false;

			Thread threadRecebimentosPacotes = new Thread(() => RecebimentoPacotesUDP());

			threadRecebimentosPacotes.Start();
		}

		private void FechaConexoesUDP()
		{
			shouldStopRecebimento = true;

			if (usocketConexaoUDP != null)
			{
				usocketConexaoUDP.Close();
			}
		}

		private async void GeraraPacotes()
		{
			int i = 0;
			while (i < 30)
			{
				var jsonData = new
				{
					id = "1", // Substitua pelo ID real do dispositivo
					numeroPacote = numeroPacote,
					IA = 150,
					IB = 150,
					IC = 190,
				};
				string correntes = JsonConvert.SerializeObject(jsonData);
				// Converte a string JSON em bytes e envia por UDP broadcast
				byte[] bytes = Encoding.ASCII.GetBytes(correntes);
				await usocketConexaoUDP.SendAsync(bytes, bytes.Length, ipConexaoEnvioUDP);
				numeroPacote += 1;
				i++;
			}
		}

		private async void RecebimentoPacotesUDP()
		{
			while (!shouldStopRecebimento)
			{
				try { 
					stopwatch.Start();
					UdpReceiveResult result = await usocketConexaoUDP.ReceiveAsync();
					bytes = result.Buffer;
					menssagemRecebida = Encoding.ASCII.GetString(bytes);

					await ProcessarPacoteAsync();

					stopwatch.Stop();

					textboxCurto.Text += $"Tempo decorrido: {stopwatch.ElapsedMilliseconds}ms{Environment.NewLine}{Environment.NewLine}";

					stopwatch.Reset();
				}
				catch (SocketException ex)
				{

					if (shouldStopRecebimento)
					{
						break;
					}

					if (ex.SocketErrorCode != SocketError.Interrupted)
					{
						throw;
					}
				}
				catch (JsonException ex)
				{
					// Manipula exceções de desserialização JSON
					textBoxAvisos.Text += $"Erro de desserialização JSON: {ex.Message}{Environment.NewLine}";
				}
			}
		}

		private async Task ProcessarPacoteAsync()
		{
			await mutex.WaitAsync();

			try
			{
				JObject jsonData = JsonConvert.DeserializeObject<JObject>(menssagemRecebida);

				if (jsonData != null)
				{
					var id = jsonData["id"]?.ToString();
					var numeroPacote = jsonData["numeroPacote"]?.ToString();
					var IA = jsonData["IA"]?.Value<int>() ?? 0;
					var IB = jsonData["IB"]?.Value<int>() ?? 0;
					var IC = jsonData["IC"]?.Value<int>() ?? 0;
					var tipo = jsonData["tipo"]?.ToString();

					if(tipo == "desligamento")
					{
						var mensagemDesligar = jsonData["mensagem"]?.ToString();
						textBoxAvisos.Text += $"JSON Enviado: {Environment.NewLine}ID: {id}{Environment.NewLine}Número do Pacote: {numeroPacote}Mensagem: {mensagemDesligar}{Environment.NewLine}";
					}
					else
					{
						textBoxRecepcaoConexao.Text += $"ID: {id}{Environment.NewLine}Número do Pacote: {numeroPacote}{Environment.NewLine}IA :{IA}{Environment.NewLine}IB :{IB}{Environment.NewLine}IC :{IC}{Environment.NewLine}{Environment.NewLine}";

						if (IA < 0)
						{
							IA = IA * -1;
						}
						if (IB < 0)
						{
							IB = IB * -1;
						}
						if (IC < 0)
						{
							IC = IC * -1;
						}
						SobrecorrenteProtection(IA, IB, IC);
					}
				}
				else
				{
					textBoxAvisos.Text += "Erro ao desserializar JSON recebido." + Environment.NewLine;
				}
			}
			catch (JsonException ex)
			{
				textBoxAvisos.Text += $"Erro de desserialização JSON: {ex.Message}{Environment.NewLine}";
			}
			finally
			{
				mutex.Release();
			}
		}

		private void SobrecorrenteProtection(int IAtualA, int IAtualB, int IAtualC)
		{
			int correnteDePickup = 181; // Limiar de ativação da proteção
			
			// Monitoramento da corrente IA
			if (IAtualA > correnteDePickup)
			{
				if (flagA == 0)
				{
					textBoxAvisos.Text += $"Iniciando temporizador para IA...{Environment.NewLine}";
					flagA++;
				}
				else {
					flagA++;
				}
				if (flagA == 25)
				{
					textboxCurto.Text += $"Tempo limite atingido para IA! A em curto, enviando mensagem de desligamento...{Environment.NewLine}{Environment.NewLine}";

					DesligarSistema("A");
				}
			}
			else
			{
				// Se a corrente voltou ao normal, pare o temporizador IA
				if (flagA != 0)
				{
					textBoxAvisos.Text += $"Parando temporizador para IA...{Environment.NewLine}";
					flagA = 0;
				}
				else
				{
					textBoxAvisos.Text += $"Nenhum curto encontrado em IA!{Environment.NewLine}";
				}
			}

			// Monitoramento da corrente IB
			if (IAtualB > correnteDePickup)
			{
				if (flagB == 0)
				{
					textBoxAvisos.Text += $"Iniciando temporizador para IB...{Environment.NewLine}";
					flagB++;
				}
				else
				{
					flagB++;
				}

				if (flagB == 25)
				{
					textboxCurto.Text += $"Tempo limite atingido para IB! B em curto, enviando mensagem de desligamento...{Environment.NewLine}{Environment.NewLine}";
					DesligarSistema("B");
				}
			}
			else
			{
				// Se a corrente voltou ao normal, pare o temporizador IB
				if (flagB != 0)
				{
					textBoxAvisos.Text += $"Parando temporizador para IB...{Environment.NewLine}";
					flagB = 0;
				}
				else
				{
					textBoxAvisos.Text += $"Nenhum curto encontrado em IB!{Environment.NewLine}";
				}
			}

			if (IAtualC > correnteDePickup)
			{
				if (flagC == 0)
				{
					textBoxAvisos.Text += $"Iniciando temporizador para IC...{Environment.NewLine}";
					flagC++;
				}
				else
				{
					flagC++;
				}

				if (flagC == 25)
				{
					textboxCurto.Text += $"Tempo limite atingido para IC! C em curto, enviando mensagem de desligamento...{Environment.NewLine}{Environment.NewLine}";
					DesligarSistema("C");
				}
			}
			else
			{
				// Se a corrente voltou ao normal, pare o temporizador IC
				if (flagC != 0)
				{
					textBoxAvisos.Text += $"Parando temporizador para IC...{Environment.NewLine}";
					flagC = 0;
				}
				else
				{
					textBoxAvisos.Text += $"Nenhum curto encontrado em IC!{Environment.NewLine}";
				}
			}
		}

		private async void DesligarSistema(string linha)
		{
			try
			{
				var jsonData = new
				{
					id = "1", // Substitua pelo ID real do dispositivo
					numeroPacote = numeroPacoted,
					mensagem = $"Desligar linha {linha}{Environment.NewLine}.",
					tipo = "desligamento"
				};

				// Serialize o objeto para uma string JSON
				string mensagemDesligar = JsonConvert.SerializeObject(jsonData);

				// Converte a string JSON em bytes e envia por UDP broadcast
				byte[] bytes = Encoding.ASCII.GetBytes(mensagemDesligar);
				await usocketConexaoUDP.SendAsync(bytes, bytes.Length, ipConexaoEnvioUDP);
				numeroPacoted += 1;
			}
			catch (Exception ex)
			{
				textBoxAvisos.Text += $"{ex.Message}";
			}
		}
	}
}