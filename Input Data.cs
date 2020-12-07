Random r = new Random();//����� ������ Random
ListHelp lh = new ListHelp(project, instance);//������ ��� ������ �� ��������
WebHelp wh = new WebHelp(project, instance);//������ ��� ������ � web
Tab tab = instance.ActiveTab;//����� �������� �������


string listEmail = project.Variables["set_path_email"].Value;//���� � ������ � �������
string names = project.Variables["set_names"].Value;//������ � �������
string ages = project.Variables["set_age"].Value;//������� � ��������
string sexs = project.Variables["set_sex"].Value;//��� ��� �����������
string selectPassword = project.Variables["set_password"].Value;//������ ���� ������
string password = "";//���������� ��� ������
string capresult = project.Variables["capres"].Value;//��������� �����

string lineMailPass = lh.GetValueListLocal(listEmail, true, false);//��������� ������ �� �����
project.SendInfoToLog(String.Format("�������� ������ �� ����� {0}  -  {1}", listEmail, lineMailPass));

//��������� �����
string[] arrNames = names.Split('\n');
string name = arrNames[r.Next(0, arrNames.Length)];
project.SendInfoToLog(String.Format("�������� ��� {0}", name));

//��������� ��������
string[] arrAges = ages.Split('-');
int ageStart = Convert.ToInt32(arrAges[0]);
int ageEnd = Convert.ToInt32(arrAges[1]);
int age = r.Next(ageStart, ageEnd);
project.SendInfoToLog(String.Format("�������� ������� {0}", age));

//��������� ����
string[] arrSex = sexs.Split(',');
string sex = arrSex[r.Next(0, arrSex.Length)];

	if(sex=="M")
	{
		sex = "1";	
	}
	else
	{
		sex = "2";	
	}

project.SendInfoToLog(String.Format("�������� ��� {0}", sex));

//��������� ������
switch (selectPassword)
{
	case "�� �������":
		password = project.Profile.Password;
		break;
		
	case "�� ������ � ������":
		password = lineMailPass.Split(':')[1];
		break;	
}
project.SendInfoToLog(String.Format("�������� ������ {0}", password));	
	
tab.Navigate("https://hitwe.com", "https://google.com");//������� �� ���� hitwe.com
tab.WaitDownloading();//�������� ��������

wh.ClickMouseXPath(tab, "//a[contains(@class, 'log-tabs')]", 2);//���� �� ���������� ��������

wh.InputTextXPath(tab, "//input[contains(@name, 'name')]", name, 0, 10000);//���� �����
wh.InputTextXPath(tab, "//input[contains(@name, 'email')]", lineMailPass.Split(':')[0], 0, 10000);//���� email
wh.InputTextXPath(tab, "//input[contains(@name, 'password')]", password, 0, 10000);//���� ������

tab.FindElementByXPath("//select[contains(@name, 'gender')]", 0).SetValue(sex, "middle");//��������� ����

//��������� ��������
string sAge = (age - 18).ToString();
tab.FindElementByXPath("//select[contains(@name, 'age')]", 0).SetValue(sAge, "middle");

tab.FindElementByXPath("//textarea", 0).SetValue(capresult, "middle");//��������� ���������� �����

wh.ClickMouseXPath(tab, "//button[contains (@class, 'log-btn green')]", 0);//���� �� ���������� ��������
tab.WaitDownloading();//�������� �������� ��������
wh.ClickMouseXPath(tab, "//a[contains(@class, 'interstial-close')]", 0);//���� �� ���������� ��������
	
	
	