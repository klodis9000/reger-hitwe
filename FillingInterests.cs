Random r = new Random();//новый объект Random
ListHelp lh = new ListHelp(project, instance);//объект дл€ работы со списками
WebHelp wh = new WebHelp(project, instance);//объект дл€ работы с web
Tab tab = instance.ActiveTab;//делаю активной вкладку

//объ€вл€ю коллекцию Html элементов
HtmlElementCollection colInterest = tab.FindElementsByXPath("//div[contains(@class, 'interests-check')]");

project.SendInfoToLog(String.Format("{0}", colInterest.Count()));

if(colInterest.Count() > 0)
{
	//переменна€ дл€ генерации процента дл€ проставновки интереса
	int random;
	
	//перебор элементов
	foreach(HtmlElement he in colInterest)
	{
		//генерираци€ числа от 0 до 100
		random = r.Next(0, 100);
		
		//если выпавшее число меньше 80, то отмечаю интерес
		if(random < 80)
			he.Click();
		
	}	
}