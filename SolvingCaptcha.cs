string proxy = "";//строка для прокси

var cap = new Acaptcha();//новый объект для формирования тела запроса
cap.clientKey = "cd3bdc34252969063612aa8e4d115dea";//апи ключ
cap.task = new Task();//ноывй объект Task

cap.task.type = "NoCaptchaTaskProxyless";//тип прокси
cap.task.websiteURL = "http://hitwe.com";//сайт где будет разгадываться капча
cap.task.websiteKey = "6LdCKgkTAAAAAPBzkm2V6PD-mmVkWbNhuPXI3IZG";//сайт-кей капчи

if (proxy != "")//если строка с прокси не пуста добавляем прокси
{
	cap.task.type = "NoCaptchaTask";
	cap.task.proxyType = "http";
	cap.task.proxyAddress = "";
	cap.task.proxyPort = 0;
	cap.task.proxyPassword = "";
	cap.task.proxyLogin = "";
	cap.task.userAgent = "";	
}

//сериализация данных для отправки запроса
string json = JsonConvert.SerializeObject(cap, Formatting.Indented);

//урл для запроса
string url = "https://api.anti-captcha.com/createTask";

//запрос
string response = ZennoPoster.HttpPost(url, json);

//десериализация объекта
ResponseAcaptcha acp = JsonConvert.DeserializeObject<ResponseAcaptcha>(response);

//переменная для ошибки
int errorId = acp.errorId;

//проверка ответа на наличие в нем ошибки
if(errorId != 0)
	throw new Exception("ошибка при отправке запроса на антикапчу");

//получение taskId
int taskId = acp.taskId;

//пауза 20 секунд
Thread.Sleep(20000);

//формирмирование данных для пост запроса с помощью словаря
Dictionary <string, string> dictJson = new Dictionary<string, string>
{
	{"clientKey", "cd3bdc34252969063612aa8e4d115dea"},
	{"taskId", taskId.ToString()}	
};

//сериализация данных
json = JsonConvert.SerializeObject(dictJson, Formatting.Indented);

//хост для запроса
url = "https://api.anti-captcha.com/getTaskResult";

//переменная для ответа
string result = "";

//20 попыток на разгадывание капчи
for(int i = 0; i < 20; i++)
{
	//пост запрос на результат капчи
	response = ZennoPoster.HttpPost(url, json);
	
	project.SendInfoToLog(response);
	
	//десериализация ответа
	acp = JsonConvert.DeserializeObject<ResponseAcaptcha>(response);
	
	//проверка статуса разгадывания капчи
	if(acp.status == "ready")
	{
		//парсинг значение капчи
		result = acp.solution.gRecaptchaResponse;
		break;
	}
	
	//пауза 10 секунд
	Thread.Sleep(10000);
	
}

//проверка получен ли результат
if(result == "")
	throw new Exception("капча не разгадана. Количество попыток превысило 20");

//сохранение результата разгадывания капчи
project.Variables["capres"].Value = result;