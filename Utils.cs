using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace İos_APİ_Suppercell_Valid_Mail_Checker
{
	internal class Utils
	{

		static Color colorm1 = Color.FromArgb(255, 210, 3);
		static Color colorm2 = Color.FromArgb(255, 210, 3);


		public static void Runner()
		{
			for (; ; )
			{
				try
				{
					Utils.updatemenu();
					int amount = Utils.Nthreads();
					Utils.iText[0] = amount.ToString();
					Utils.updatemenu();
					string text = Utils.Protocol();
					Utils.iText[1] = text;
					Utils.updatemenu();
					List<string> list = new List<string>();
					bool flag = text != "NO";
					if (flag)
					{
						foreach (string item in File.ReadAllLines(Utils.GetPath("ProxyList")))
						{
							list.Add(item);
						}
					}
					Utils.iText[2] = list.Count.ToString();
					Utils.updatemenu();
					string path = Utils.GetPath("ComboList");
					ConcurrentQueue<string> concurrentQueue = new ConcurrentQueue<string>();
					foreach (string text2 in File.ReadAllLines(path))
					{
						bool flag2 = text2.Contains(":");
						if (flag2)
						{
							concurrentQueue.Enqueue(text2);
						}
					}
					Utils.iText[3] = concurrentQueue.Count.ToString();
					Utils.updatemenu();
					Colorful.Console.Write("\n\tDevam Etmek İçin 'Enter'e Basınız !!", colorm1);
					Console.ReadLine();
					Utils.updatemenu2();
					Checker_Main check = new Checker_Main(concurrentQueue, list, text);
					check.Create();
					check.Start();
					GlobalData.Working = true;
					check.Threading(amount);
				}
				catch
				{
					continue;
				}
				break;
			}
		}

		private static void updatemenu()
		{
			Logo._Show();
			string format = Utils.infoText;
			Color styledColor = colorm2;
			Color defaultColor = colorm1;
			object[] args = Utils.iText;
			Colorful.Console.WriteLineFormatted(format, styledColor, defaultColor, args);
		}

		private static void updatemenu2()
		{
			Logo._Show();
			string format = Utils.infoText;
			Color styledColor = colorm2;
			Color defaultColor = colorm1;
			object[] args = Utils.iText;
			Colorful.Console.WriteLineFormatted(format, styledColor, defaultColor, args);
		}

		public static int Nthreads()
		{
			int result;
			for (; ; )
			{
				Colorful.Console.Write("\n\t [Bots: (Min : 50 | Max : 200 )]: ", colorm2);
				try
				{
					result = Convert.ToInt32(Colorful.Console.ReadLine());
				}
				catch
				{
					continue;
				}
				break;
			}
			return result;
		}

		public static string Protocol()
		{
			string result;
			for (; ; )
			{
				Colorful.Console.Write("\n\t[NO/HTTP/SOCKS4/SOCKS5]: ", colorm2);
				try
				{
					string text = Colorful.Console.ReadLine().ToUpper();
					bool flag = !(text == "HTTP") && !(text == "NO") && !(text == "SOCKS4") && !(text == "SOCKS5");
					if (flag)
					{
						continue;
					}
					result = text;
				}
				catch
				{
					continue;
				}
				break;
			}
			return result;
		}

		public static string GetPath(string target)
		{
			Colorful.Console.Write("\n\t [Yükle " + target + "] ", colorm2);
			Thread.Sleep(500);
			Colorful.Console.Write(" >> ", colorm1);
			string fileName;
			using (OpenFileDialog openFileDialog = new OpenFileDialog())
			{
				openFileDialog.Filter = "Text Dosyası (*.txt)|*.txt";
				openFileDialog.Multiselect = false;
				openFileDialog.Title = "Load " + target;
				openFileDialog.ShowDialog();
				foreach (string text in File.ReadAllLines(openFileDialog.FileName))
				{
					text.Replace("\t", "");
					text.Replace("\r", "");
					text.Trim();
					text.Replace(" ", "");
				}
				fileName = openFileDialog.FileName;
			}
			return fileName;
		}

		public static void Append(string file, object content)
		{
			try
			{
				object writeLock = Utils.WriteLock;
				object obj = writeLock;
				lock (obj)
				{
					using (FileStream fileStream = File.Open(file, FileMode.Append))
					{
						using (StreamWriter streamWriter = new StreamWriter(fileStream))
						{
							streamWriter.WriteLine(content);
						}
					}
				}
			}
			catch (Exception)
			{
			}
		}

		public static string Date()
		{
			return DateTime.Now.ToString(" HH:mm | ");
		}

		private static readonly object WriteLock = new object();

		private static string infoText = "\n\n\t Bots     : [ {0} ]      \n\t Proxy Tipi : [ {1} ]      \n\t Proxylist  : [ {2} ]      \n\t Combolist  : [ {3} ] \n\n";

		private static string[] iText = new string[]
		{
			"-X-",
			"-X-",
			"-X-",
			"-X-",
			"-X-"
		};
	}
}
