Random r = new Random();//новый объект Random
ListHelp lh = new ListHelp(project, instance);//объект для работы со списками
WebHelp wh = new WebHelp(project, instance);//объект для работы с web
Tab tab = instance.ActiveTab;//делаю активной вкладку


string listEmail = project.Variables["set_path_email"].Value;//путь к списку с почтами
string names = project.Variables["set_names"].Value;//строка с именами
string ages = project.Variables["set_age"].Value;//возраст в дипазоне
string sexs = project.Variables["set_sex"].Value;//пол для регистрации
string selectPassword = project.Variables["set_password"].Value;//откуда беру пароль
string password = "";//переменная для пароля
string capresult = project.Variables["capres"].Value;//результат капчи

string lineMailPass = lh.GetValueListLocal(listEmail, true, false);//получение строки из файла
project.SendInfoToLog(String.Format("Получили строку из файла {0}  -  {1}", listEmail, lineMailPass));

//получение имени
string[] arrNames = names.Split('\n');
string name = arrNames[r.Next(0, arrNames.Length)];
project.SendInfoToLog(String.Format("Получили имя {0}", name));

//получение возраста
string[] arrAges = ages.Split('-');
int ageStart = Convert.ToInt32(arrAges[0]);
int ageEnd = Convert.ToInt32(arrAges[1]);
int age = r.Next(ageStart, ageEnd);
project.SendInfoToLog(String.Format("Получили возраст {0}", age));

//получание пола
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

project.SendInfoToLog(String.Format("Получили пол {0}", sex));

//получение пароль
switch (selectPassword)
{
	case "Из профиля":
		password = project.Profile.Password;
		break;
		
	case "Из строки с почтой":
		password = lineMailPass.Split(':')[1];
		break;	
}
project.SendInfoToLog(String.Format("Получили пароль {0}", password));	
	
tab.Navigate("https://hitwe.com", "https://google.com");//переход на сайт hitwe.com
tab.WaitDownloading();//ожидание загрузки

wh.ClickMouseXPath(tab, "//a[contains(@class, 'log-tabs')]", 2);//клик по указанному элементу

wh.InputTextXPath(tab, "//input[contains(@name, 'name')]", name, 0, 10000);//ввод имени
wh.InputTextXPath(tab, "//input[contains(@name, 'email')]", lineMailPass.Split(':')[0], 0, 10000);//ввод email
wh.InputTextXPath(tab, "//input[contains(@name, 'password')]", password, 0, 10000);//ввод пароля

tab.FindElementByXPath("//select[contains(@name, 'gender')]", 0).SetValue(sex, "middle");//установка пола

//установка возраста
string sAge = (age - 18).ToString();
tab.FindElementByXPath("//select[contains(@name, 'age')]", 0).SetValue(sAge, "middle");

tab.FindElementByXPath("//textarea", 0).SetValue(capresult, "middle");//установка результата капчи

wh.ClickMouseXPath(tab, "//button[contains (@class, 'log-btn green')]", 0);//клик по указанному элементу
tab.WaitDownloading();//ожидание загрузки страницы
wh.ClickMouseXPath(tab, "//a[contains(@class, 'interstial-close')]", 0);//клик по указанному элементу
	
	
	