<!--//顯示年份//-->
function show_date(){
	var time=new Date(); //宣告日期物件，儲存目前系統時間
	t_year=time.getFullYear(); //取得今年年分
	if(t_year > 2007){
		document.write(" - " + t_year);
	}
}

<!--//顯示今天日期//-->
function show_today(obj){
	var time=new Date(); //宣告日期物件，儲存目前系統時間
		time.setDate(time.getDate()-1);
	get_year=time.getFullYear(); //取得今年年份
	get_month=time.getMonth()+1; //取得月份
	get_day=time.getDate(); //取得日期
		obj.value = get_year +"."+ get_month +"."+ get_day;
	document.addForm.appstartday.value = get_year +"."+ get_month +"."+ get_day;
	//document.addForm.appendday.value = get_year +"/"+ get_month +"/"+ get_day;
}