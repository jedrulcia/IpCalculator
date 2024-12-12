using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace IpCalculator
{
	public class MainPageViewModel : BindableObject
	{
		private string _ipAddress;
		private string _selectedMask;

		public string IpAddress
		{
			get => _ipAddress;
			set
			{
				_ipAddress = value;
				OnPropertyChanged();
			}
		}

		public string SelectedMask
		{
			get => _selectedMask;
			set
			{
				_selectedMask = value;
				OnPropertyChanged();
			}
		}

		public ICommand CalculateIpCommand { get; }

		public List<string> MaskList {  get; set; }

		private string _ipAddressDecimal = string.Empty;
		public string IpAddressDecimal
		{
			get => _ipAddressDecimal;
			set
			{
				_ipAddressDecimal = value;
				OnPropertyChanged();
			}
		}

		private string _ipAddressBinary = string.Empty;
		public string IpAddressBinary
		{
			get => _ipAddressBinary;
			set
			{
				_ipAddressBinary = value;
				OnPropertyChanged();
			}
		}

		private string _maskDecimal = string.Empty;
		public string MaskDecimal
		{
			get => _maskDecimal;
			set
			{
				_maskDecimal = value;
				OnPropertyChanged();
			}
		}

		private string _maskBinary = string.Empty;
		public string MaskBinary
		{
			get => _maskBinary;
			set
			{
				_maskBinary = value;
				OnPropertyChanged();
			}
		}
		private string _networkAddressDecimal = string.Empty;
		public string NetworkAddressDecimal
		{
			get => _networkAddressDecimal;
			set
			{
				_networkAddressDecimal = value;
				OnPropertyChanged();
			}
		}

		private string _networkAddressBinary = string.Empty;
		public string NetworkAddressBinary
		{
			get => _networkAddressBinary;
			set
			{
				_networkAddressBinary = value;
				OnPropertyChanged();
			}
		}

		private string _broadcastAddressDecimal = string.Empty;
		public string BroadcastAddressDecimal
		{
			get => _broadcastAddressDecimal;
			set
			{
				_broadcastAddressDecimal = value;
				OnPropertyChanged();
			}
		}

		private string _broadcastAddressBinary = string.Empty;
		public string BroadcastAddressBinary
		{
			get => _broadcastAddressBinary;
			set
			{
				_broadcastAddressBinary = value;
				OnPropertyChanged();
			}
		}

		private string _numberOfHostsDecimal = string.Empty;
		public string NumberOfHostsDecimal
		{
			get => _numberOfHostsDecimal;
			set
			{
				_numberOfHostsDecimal = value;
				OnPropertyChanged();
			}
		}

		private string _hostMinDecimal = string.Empty;
		public string HostMinDecimal
		{
			get => _hostMinDecimal;
			set
			{
				_hostMinDecimal = value;
				OnPropertyChanged();
			}
		}

		private string _hostMinBinary = string.Empty;
		public string HostMinBinary
		{
			get => _hostMinBinary;
			set
			{
				_hostMinBinary = value;
				OnPropertyChanged();
			}
		}

		private string _hostMaxDecimal = string.Empty;
		public string HostMaxDecimal
		{
			get => _hostMaxDecimal;
			set
			{
				_hostMaxDecimal = value;
				OnPropertyChanged();
			}
		}

		private string _hostMaxBinary = string.Empty;
		public string HostMaxBinary
		{
			get => _hostMaxBinary;
			set
			{
				_hostMaxBinary = value;
				OnPropertyChanged();
			}
		}


		public MainPageViewModel()
		{
			List<string> maskListHolder = new List<string>();
			maskListHolder.Add("/0 aka 0.0.0.0");

			char[] binaryMask = new char[32];
			for (int i = 0; i < 32; i++)
			{
				binaryMask[i] = '0';
			}

			for (int i = 0; i < 32; i++)
			{
				binaryMask[i] = '1';
				string firstOctet = "";
				string secondOctet = "";
				string thirdOctet = "";
				string fourthOctet = "";

				for (int j = 0; j < 8; j++)
				{
					firstOctet += binaryMask[j];
					secondOctet += binaryMask[j + 8];
					thirdOctet += binaryMask[j + 16];
					fourthOctet += binaryMask[j + 24];
				}

				// Tworzymy nową maskę w formacie /x aka a.b.c.d
				string newMask = $"{Convert.ToInt32(firstOctet, 2)}." +
					$"{Convert.ToInt32(secondOctet, 2)}." +
					$"{Convert.ToInt32(thirdOctet, 2)}." +
					$"{Convert.ToInt32(fourthOctet, 2)}";

				maskListHolder.Add(newMask);
			}
			MaskList = new List<string>(maskListHolder);


			CalculateIpCommand = new Command(() => CalculateIp(IpAddress, SelectedMask));
		}

		private void CalculateIp(string ipAddressDecimal, string maskDecimal)
		{
			IpAddressDecimal = ipAddressDecimal;
			IpAddressBinary = CalculateToBinary(IpAddressDecimal);

			MaskDecimal = maskDecimal;
			MaskBinary = CalculateToBinary(MaskDecimal);

			NetworkAddressBinary = CalculateNetworkAddressBinary(IpAddressBinary, MaskBinary);
			NetworkAddressDecimal = CalculateToDecimal(NetworkAddressBinary);

			BroadcastAddressBinary = CalculateBroadcastAddressBinary(NetworkAddressBinary);
			BroadcastAddressDecimal = CalculateToDecimal(BroadcastAddressBinary);

			NumberOfHostsDecimal = CalculateNumberOfHosts(MaskBinary);

			HostMinBinary = CalculateHostMinBinary(NetworkAddressBinary);
			HostMinDecimal = CalculateToDecimal(HostMinBinary);

			HostMaxBinary = CalculateHostMaxBinary(BroadcastAddressBinary);
			HostMinDecimal = CalculateToDecimal(HostMaxBinary);

		}

		private string CalculateToBinary(string text)
		{
			string binary = "";
			var numbers = text.Split('.');

			foreach (var number in numbers)
			{
				int num = int.Parse(number);
				binary += Convert.ToString(num, 2).PadLeft(8, '0') + "."; 
			}

			return binary.TrimEnd('.');
		}

		private string CalculateToDecimal(string binary)
		{
			string text = "";
			var numbers = binary.Split(".");

			foreach (var number in numbers)
			{
				text += $"{Convert.ToInt32(number, 2)}.";
			}

			return text.TrimEnd('.');
		}

		private string CalculateNetworkAddressBinary (string ipAddressBinary, string maskBinary)
		{
			string networkAddressBinary = "";
			for (int i = 0; i < ipAddressBinary.Length; i++)
			{
				if (ipAddressBinary[i] != '.')
				{
					int firstNumber = Convert.ToInt32(ipAddressBinary[i].ToString());
					int secondNumber = Convert.ToInt32(maskBinary[i].ToString());

					networkAddressBinary += firstNumber & secondNumber;
				}
				else
				{
					networkAddressBinary += '.';
				}
			}
			return networkAddressBinary;	
		}


		// DO POPRAWY - NIE UWZGLEDNIONE KROPKI
		private string CalculateBroadcastAddressBinary (string networkAddressBinary)
		{
			string broadcastAddressBinary = networkAddressBinary;
			broadcastAddressBinary.TrimEnd('0');
			for (int i = broadcastAddressBinary.Length; i <= 32; i++)
			{
				broadcastAddressBinary += '1';
			}
			return broadcastAddressBinary;
		}

		private string CalculateNumberOfHosts (string maskBinary)
		{
			int counter = 0;
			for (int i = maskBinary.Length - 1; i >= 0; i--)
			{
				if (maskBinary[i] == '0')
				{
					counter++;
				}
				else if (maskBinary[i] == '1')
				{
					break;
				}
			}
			double result = Math.Pow(2, counter);

			return result.ToString();
		}

		private string CalculateHostMinBinary(string networkAddressBinary)
		{
			string hostMinBinary = networkAddressBinary.Substring(0, networkAddressBinary.Length - 1);
			hostMinBinary += '1';
			return hostMinBinary;
		}

		private string CalculateHostMaxBinary(string broadcastAddressBinary)
		{
			string hostMaxBinary = broadcastAddressBinary.Substring(0, broadcastAddressBinary.Length - 1);
			hostMaxBinary += '0';
			return hostMaxBinary;
		}
	}

}
