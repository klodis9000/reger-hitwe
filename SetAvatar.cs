Random r = new Random();//����� ������ Random
ListHelp lh = new ListHelp(project, instance);//������ ��� ������ �� ��������
WebHelp wh = new WebHelp(project, instance);//������ ��� ������ � web
Tab tab = instance.ActiveTab;//����� �������� �������

string pathToPhoto = @"f:\1.jpg";//���� � ����
instance.SetFilesForUpload(pathToPhoto);
tab.FindElementByXPath("//form[contains(@class, 'file_form')]//input[@id='photo']", 0).Click();//���� �� ���������� ��������
//wh.ClickMouseXPath(tab, "//form[contains(@class, 'file_form')]//input[@id='photo']", 0);