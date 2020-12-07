using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Text.RegularExpressions;
using ZennoLab.CommandCenter;
using ZennoLab.InterfacesLibrary;
using ZennoLab.InterfacesLibrary.ProjectModel;
using ZennoLab.InterfacesLibrary.ProjectModel.Collections;
using ZennoLab.InterfacesLibrary.ProjectModel.Enums;
using ZennoLab.Macros;
using Global.ZennoExtensions;
using ZennoLab.Emulation;
using HtmlAgilityPack;
using System.Diagnostics;
using System.Xml;
using xNet;

namespace ZennoLab.OwnCode
{
	/// <summary>
	/// A simple class of the common code
	/// </summary>
	public class CommonCode
	{
		/// <summary>
		/// Lock this object to mark part of code for single thread execution
		/// </summary>
		public static object SyncObject = new object();

		// Insert your code here
	}
	
	public class Acaptcha
	{
	    public string clientKey { get; set; }
	    public Task task { get; set; }
	}
	
	public class Solution
	{
	    public string text { get; set; }
	    public string url { get; set; }
		 public string gRecaptchaResponse { get; set; }
	}
	
	public class ResponseAcaptcha
	{
	    public int errorId { get; set; }
	    public int taskId { get; set; }
	    public string status { get; set; }
	    public Solution solution { get; set; }
	    public string cost { get; set; }
	    public string ip { get; set; }
	    public int createTime { get; set; }
	    public int endTime { get; set; }
	    public string solveCount { get; set; }
	}
	
	public class Task
	{
	    public string type { get; set; }
	    public string websiteURL { get; set; }
	    public string websiteKey { get; set; }
	    public string proxyType { get; set; }
	    public string proxyAddress { get; set; }
	    public int proxyPort { get; set; }
	    public string proxyLogin { get; set; }
	    public string proxyPassword { get; set; }
	    public string userAgent { get; set; }
	}
}


namespace ZennoHelp
{
	class Settings
	{
		public static int code;//���� ���� ������			
	}

	public class ListHelp
	{
		IZennoPosterProjectModel project;
		Instance instance;

		public ListHelp(IZennoPosterProjectModel _project, Instance _instance)//����������� ������. �������� � ����� instance � project
		{
			project = _project;
			instance = _instance;
		}

		public string GetValueListZp (string nameList, bool randomLine, bool delLine) //������ ������ �� �����
		{
			Random r = new Random();//������ Random ��� ��������� ���������� �����
			IZennoList list = project.Lists[nameList];	//�������� ������
			int rndRow = 0;//���������� ��� ��������� ���������� �����
			string line = String.Empty;//���������� ��� ��������� ������
			
			lock(SyncObjects.ListSyncer)//���������� ��� �����������
			{			
				if (list.Count == 0) throw new Exception(String.Format("������ {0} ����!", nameList));//�������� ���� �� � ������ ���� ���� ������
				
				if (randomLine)//�������� ����� �� ��������� ������ �� �����
					rndRow = r.Next(0, list.Count -1);//����������� ���������� ����� � ��������� ���������� �����
				
				line = list[rndRow];//������� ������ �� �����
				
				if (!delLine)//������ ������?
				{
					list.RemoveAt(rndRow);//������ ������
					list.Add(line);	//������� ������ � ����� ������
				}
			}
			
			return line;
			
		}
		/// <summary>
		/// ����� ������ �� ����� ����� �������� ������� IZennoList
		/// </summary>
		/// <param name="pathToList"></param>
		/// <param name="randomLine"></param>
		/// <param name="delLine"></param>
		/// <returns></returns>
		public string GetValueListLocal (string pathToList, bool randomLine, bool delLine) ��������� ������ �� ����� ����� �������� ������� IZennoList
		{
			Random r = new Random();//������ Random ��� ��������� ���������� �����
			int rndRow = 0;//���������� ��� ��������� ���������� �����
			string line = String.Empty;//���������� ��� ��������� ������
			
			if (!File.Exists(pathToList)) throw new Exception(String.Format("������ {0} �� ������!", pathToList));//�������� ���� �� ���� �� ���������� ����
			
			
			
			lock(SyncObjects.ListSyncer)//���������� ��� �����������
			{
				var listLine = File.ReadAllLines(pathToList).ToList();//������ ����� � ����������
				
				if (listLine.Count() == 0) throw new Exception(String.Format("������ {0} ����!", pathToList));//�������� ���������� �����

				if (randomLine) listLine.Shuffle();//������������� ������
				
				line = listLine.ElementAt(0).Trim();//������ ������ � ����������
				
				if(delLine)	listLine.RemoveAt(0);//�������� ������
				
				File.WriteAllLines(pathToList, listLine);//��������� ������ ������� � ����
			}
			
			return line;
			
		}
	}
	
	public class WebHelp
	{
		
		IZennoPosterProjectModel project;
		Instance instance;

		public WebHelp(IZennoPosterProjectModel _project, Instance _instance)
		{
			project = _project;
			instance = _instance;
		}
		

		public void WaitFor(Tab tab, string tags, string attrName, string attrValue, string searchKind, int number = 0, int timeout = 5000) //�������� ��������
		{		
			Random r = new Random();//������ Random
			Thread.Sleep(r.Next(750, 1500));//�����
			tab.WaitDownloading();//�������� �������� ��������
			
		    for (var i = 0; i < timeout / 100; i++)//���� �������� ��������
		    {
		        if (!tab.FindElementByAttribute(tags, attrName, attrValue, searchKind, number).IsVoid)//�������� ������������� �������� �� ��������
				{
					Settings.code = 1;//���������� ���������� ��� �������� 1 - ��������� �������
					return;	
				}
				//return 1;	        
				
		        Thread.Sleep(100);//�����
		    }
			 Settings.code = -1;//���������� ���������� ��� �������� -1 - �� ��������� �������
		    //return -1;
			
		}
		
		public void ClickMouse(Tab tab, string tags, string attrName, string attrValue, string searchKind, int number = 0, int timeout = 15000) //���� ����� ������� ����
		{
			WaitFor(tab, tags, attrName, attrValue, searchKind, number, timeout = 15000);//������� ����� �������� ��������
			
			if(Settings.code != 1) throw new Exception("������� �� ������");//�������� �������� �� �������
			
			HtmlElement he = tab.FindElementByAttribute(tags, attrName, attrValue, searchKind, number);//�������� Html �������
			//he.ScrollIntoView();//��������� ��������� ���� �� ��������
			tab.FullEmulationMouseMoveToHtmlElement(he);//��������� ���� �� �������
			tab.FullEmulationMouseClick("left", "click");//����
			
			Settings.code = 1;//���������� ���� ����������
		}
		
		public void ClickMouseXPath(Tab tab, string xPath, int number = 0, int timeout = 15000) // ���� ����� ������� ����
		{
			WaitForXPath(tab, xPath, number, timeout = 15000);
			
			if(Settings.code != 1)
				throw new Exception("�� ������ �������");
			
			HtmlElement he = tab.FindElementByXPath(xPath, number);
			//he.ScrollIntoView();
			tab.FullEmulationMouseMoveToHtmlElement(he);
			tab.FullEmulationMouseClick("left", "click");	
		}
		
		public void WaitForXPath(Tab tab, string xPath, int number = 0, int timeout = 5000) // �������� ��������
		{			
			tab.WaitDownloading();
			
		    for (var i = 0; i < timeout / 100; i++)
		    {
		        if (!tab.FindElementByXPath(xPath, 0).IsVoid)
				{
					Settings.code = 1;
					return;	
				}
			
		        Thread.Sleep(100);
		    }
			 Settings.code = -1;
			
		}
		
		public void InputTextXPath(Tab tab, string xPath, string text, int number = 0, int timeout = 15000)
		{
			WaitForXPath(tab, xPath, number, timeout = 15000);
			
			if(Settings.code != 1)
				throw new Exception("�� ������ �������");
			
			HtmlElement he = tab.FindElementByXPath(xPath, number);
			//he.ScrollIntoView();
			tab.FullEmulationMouseMoveToHtmlElement(he);
			tab.FullEmulationMouseClick("left", "click");
            string check = he.GetValue(false);
			
				if (check != String.Empty)
				{
					tab.KeyEvent("A", "press", "ctrl");
					Thread.Sleep(2000);
					tab.KeyEvent("Back", "press", "");
				}
				
			instance.SendText(text, 50);
		}
		
		
		public void InputText(Tab tab, string tags, string attrName, string attrValue, string searchKind, string text, int number = 0, int timeout = 15000)
		{
			WaitFor(tab, tags, attrName, attrValue, searchKind, number, timeout = 15000);//����� ������ �������� ��������
			
			if(Settings.code != 1) throw new Exception("������� �� ������");//�������� �������� �� �������
			
			HtmlElement he = tab.FindElementByAttribute(tags, attrName, attrValue, searchKind, number);//���������� Html �������
			//he.ScrollIntoView();//��������� ��������� ���� �� ��������
			tab.FullEmulationMouseMoveToHtmlElement(he);
			tab.FullEmulationMouseClick("left", "click");//��������� ���� �� �������
            string check = he.GetValue(false);//���������� ���������� ���� � ����������
			
				if (check != String.Empty)//�������� �� ������������� ����
				{
					tab.KeyEvent("A", "press", "ctrl");//������� CTRL+A
					Thread.Sleep(2000);//�����
					tab.KeyEvent("Back", "press", "");//�������� ������ ������� Backspace
				}
				
			instance.SendText(text, 50);//���� ������
				
			Settings.code = 1;//���������� ���� ����������
		}
	
	}
	

	class Proxys // ����� ��� ������
	{
		internal string ProxyType { get; set; }

		internal string ProxyIp { get; set; }

		internal int ProxyPort { get; set; }

		internal string ProxyLogin { get; set; }

		internal string ProxyPassword { get; set; }

		string prox = "";

		internal ProxyInfo Info { get; set; }

		internal string Proxy
		{
			get { return prox; }
			set
			{
				// ������� ������ ��� ���������� ��������������� �����
				prox = value;

				string strProxy = prox;
				if (strProxy == "")
					return;

				var arrProt = Regex.Split(prox, @"\/\/");

				if (arrProt.Length == 2)
				{
					ProxyType = arrProt[0].Trim().TrimEnd(':').Trim();
					strProxy = arrProt[1];
				}
				else
				{
					ProxyType = "http";
				}

				var arrUser = strProxy.Split('@');
				if (arrUser.Length == 2)
				{
					ProxyLogin = arrUser[0].Split(':')[0];
					ProxyPassword = arrUser[0].Split(':')[1];
					strProxy = arrUser[1];
				}
				else
				{
					ProxyLogin = "";
					ProxyPassword = "";
				}

				ProxyIp = strProxy.Split(':')[0];
				ProxyPort = Convert.ToInt32(strProxy.Split(':')[1]);
			}

		}


		public static ProxyInfo CheckIP(string Proxy)
		{
			var prox = new Proxys() { Proxy = Proxy };
			string Response = "";
			var Info = new ProxyInfo();

			string Url = "http://oip.cc/en";

			try
			{
				Stopwatch stopwatch = Stopwatch.StartNew();
				Response = ZennoPoster.HttpGet(Url, prox.Proxy, "utf-8", ZennoLab.InterfacesLibrary.Enums.Http.ResponceType.HeaderAndBody, 10000, "", "").HtmlDecode();
				stopwatch.Stop();
				Info.TimeRequest = stopwatch.Elapsed;
			} catch(Exception ex)
			{
				Info.ErrorMessage = ex.Message;
				Info.ErrorCheck = true;
				return Info;
			}

			// �������� ���� ������ �������
			var CodeResp = Response.ParseCodeResponse();
			if(CodeResp != 200)
			{
				Info.ErrorCheck = true;
				Info.ErrorMessage = "����� �������: " + CodeResp;
				return Info;
			}

			HtmlDocument doc = new HtmlDocument();
			// �������� ���������� � ������ doc
			doc.LoadHtml(Response);

			var elIP = doc.DocumentNode.SelectSingleNode(@"//ul/li[contains(text(),'My IP address')]/following::li");
			if(elIP == null)
			{
				// �� ������� ���������� IP
				Info.ErrorCheck = true;
				Info.ErrorMessage = "�� ����� IP: " + Response;
				return Info;
			}

			Info.Ip = elIP.InnerText.Trim().Split(' ')[0].Trim();

			return Info;
		}
	}
	
	class ProxyInfo
	{

		internal string Ip { get; set; }

		internal bool ErrorCheck { get; set; }

		internal string ErrorMessage { get; set; }

		internal TimeSpan TimeRequest { get; set; }
	}
	
	class Cookies
	{
		IZennoPosterProjectModel project;
		Instance instance;

		public Cookies(IZennoPosterProjectModel _project, Instance _instance)//����������� ������. �������� � ����� instance � project
		{
			project = _project;
			instance = _instance;
		}
		
		internal CookieDictionary colGetCookies = new CookieDictionary();
		internal string lineCookie = "";
		
		internal void GetCookie (CookieDictionary Cookies)
		{
			foreach (var cook in Cookies.ToArray())
			{
				project.SendInfoToLog(cook.Key + "|" + cook.Value);
				if(colGetCookies.ContainsKey(cook.Key))
				{
					colGetCookies[cook.Key] = cook.Value;
				}
				else
				{
					colGetCookies.Add(cook.Key, cook.Value);	
				}
				
				lineCookie += cook.Key + ":" + cook.Value + "|";
			}
		}
		
		internal void SetCookie (string cookies)
		{
			string[] cookie = cookies.Split('|');	
			
			foreach(string cook in cookie)
			{
				string key = cook.Split(':')[0];
				string val = cook.Split(':')[1];
				
				if(colGetCookies.ContainsKey(key))
				{
					colGetCookies[key] = val;					
				}
				else
				{
					colGetCookies.Add(key, val);	
				}
				
			}
			
		}
		
		
	}
	
	public static class Extension
	{

		public static int ParseCodeResponse (this string Text)
		{
			var arrText = Text.Replace("\r\n", " ").Replace("\n", " ").Split(new[] { ' ' }, 4);

			int Code = arrText[1].ParseNumberToInt();
			string CodeMessage = arrText[2].Trim();

			return Code;
		}

		public static int ParseNumberToInt(this string Text)
		{
			var Digit = String.Concat(Text.Trim().Where(char.IsDigit));
			if (Digit == "")
				return -1;

			var res = Convert.ToInt32(Digit);

			return res;
		}

		public static string HtmlDecode(this string LineIn)
		{
			LineIn = LineIn.Replace(@"&nbsp;", " ");
			string res = System.Text.RegularExpressions.Regex.Unescape(LineIn);
			return System.Web.HttpUtility.HtmlDecode(res);
		}
		
		public static string UrlEncode(this string LineIn)
		{
			return System.Web.HttpUtility.UrlEncode(LineIn);
		}
	}
	
	
}