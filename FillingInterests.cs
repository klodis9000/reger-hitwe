Random r = new Random();//����� ������ Random
ListHelp lh = new ListHelp(project, instance);//������ ��� ������ �� ��������
WebHelp wh = new WebHelp(project, instance);//������ ��� ������ � web
Tab tab = instance.ActiveTab;//����� �������� �������

//�������� ��������� Html ���������
HtmlElementCollection colInterest = tab.FindElementsByXPath("//div[contains(@class, 'interests-check')]");

project.SendInfoToLog(String.Format("{0}", colInterest.Count()));

if(colInterest.Count() > 0)
{
	//���������� ��� ��������� �������� ��� ������������ ��������
	int random;
	
	//������� ���������
	foreach(HtmlElement he in colInterest)
	{
		//����������� ����� �� 0 �� 100
		random = r.Next(0, 100);
		
		//���� �������� ����� ������ 80, �� ������� �������
		if(random < 80)
			he.Click();
		
	}	
}