<!--//��ܦ~��//-->
function show_date(){
	var time=new Date(); //�ŧi�������A�x�s�ثe�t�ήɶ�
	t_year=time.getFullYear(); //���o���~�~��
	if(t_year > 2007){
		document.write(" - " + t_year);
	}
}

<!--//��ܤ��Ѥ��//-->
function show_today(obj){
	var time=new Date(); //�ŧi�������A�x�s�ثe�t�ήɶ�
		time.setDate(time.getDate()-1);
	get_year=time.getFullYear(); //���o���~�~��
	get_month=time.getMonth()+1; //���o���
	get_day=time.getDate(); //���o���
		obj.value = get_year +"."+ get_month +"."+ get_day;
	document.addForm.appstartday.value = get_year +"."+ get_month +"."+ get_day;
	//document.addForm.appendday.value = get_year +"/"+ get_month +"/"+ get_day;
}