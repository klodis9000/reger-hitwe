string proxy = "";//������ ��� ������

var cap = new Acaptcha();//����� ������ ��� ������������ ���� �������
cap.clientKey = "cd3bdc34252969063612aa8e4d115dea";//��� ����
cap.task = new Task();//����� ������ Task

cap.task.type = "NoCaptchaTaskProxyless";//��� ������
cap.task.websiteURL = "http://hitwe.com";//���� ��� ����� ������������� �����
cap.task.websiteKey = "6LdCKgkTAAAAAPBzkm2V6PD-mmVkWbNhuPXI3IZG";//����-��� �����

if (proxy != "")//���� ������ � ������ �� ����� ��������� ������
{
	cap.task.type = "NoCaptchaTask";
	cap.task.proxyType = "http";
	cap.task.proxyAddress = "";
	cap.task.proxyPort = 0;
	cap.task.proxyPassword = "";
	cap.task.proxyLogin = "";
	cap.task.userAgent = "";	
}

//������������ ������ ��� �������� �������
string json = JsonConvert.SerializeObject(cap, Formatting.Indented);

//��� ��� �������
string url = "https://api.anti-captcha.com/createTask";

//������
string response = ZennoPoster.HttpPost(url, json);

//�������������� �������
ResponseAcaptcha acp = JsonConvert.DeserializeObject<ResponseAcaptcha>(response);

//���������� ��� ������
int errorId = acp.errorId;

//�������� ������ �� ������� � ��� ������
if(errorId != 0)
	throw new Exception("������ ��� �������� ������� �� ���������");

//��������� taskId
int taskId = acp.taskId;

//����� 20 ������
Thread.Sleep(20000);

//��������������� ������ ��� ���� ������� � ������� �������
Dictionary <string, string> dictJson = new Dictionary<string, string>
{
	{"clientKey", "cd3bdc34252969063612aa8e4d115dea"},
	{"taskId", taskId.ToString()}	
};

//������������ ������
json = JsonConvert.SerializeObject(dictJson, Formatting.Indented);

//���� ��� �������
url = "https://api.anti-captcha.com/getTaskResult";

//���������� ��� ������
string result = "";

//20 ������� �� ������������ �����
for(int i = 0; i < 20; i++)
{
	//���� ������ �� ��������� �����
	response = ZennoPoster.HttpPost(url, json);
	
	project.SendInfoToLog(response);
	
	//�������������� ������
	acp = JsonConvert.DeserializeObject<ResponseAcaptcha>(response);
	
	//�������� ������� ������������ �����
	if(acp.status == "ready")
	{
		//������� �������� �����
		result = acp.solution.gRecaptchaResponse;
		break;
	}
	
	//����� 10 ������
	Thread.Sleep(10000);
	
}

//�������� ������� �� ���������
if(result == "")
	throw new Exception("����� �� ���������. ���������� ������� ��������� 20");

//���������� ���������� ������������ �����
project.Variables["capres"].Value = result;