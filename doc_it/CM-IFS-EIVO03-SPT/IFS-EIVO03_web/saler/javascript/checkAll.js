//第1組勾選欄
    function Select1(e)
    {
	if (e.checked) {
	    document.dataList.toggleAll.checked = AllChecked();
	}
	else {
	    document.dataList.toggleAll.checked = false;
	}
    }

    function ToggleAll(e)
    {
	if (e.checked) {
	    CheckAll();
	}
	else {
	    ClearAll();
	}
    }

    function Check(e)
    {
	e.checked = true;
    }

    function Clear(e)
    {
	e.checked = false;
    }

    function CheckAll()
    {
	var ml = document.dataList;
	var len = ml.elements.length;
	for (var i = 0; i < len; i++) {
	    var e = ml.elements[i];
	    if (e.name == "Mid") {
		Check(e);
	    }
	}
	ml.toggleAll.checked = true;
    }

    function ClearAll()
    {
	var ml = document.dataList;
	var len = ml.elements.length;
	for (var i = 0; i < len; i++) {
	    var e = ml.elements[i];
	    if (e.name == "Mid") {
		Clear(e);
	    }
	}
	ml.toggleAll.checked = false;
    }

    function AllChecked()
    {
	ml = document.dataList;
	len = ml.elements.length;
	for(var i = 0 ; i < len ; i++) {
	    if (ml.elements[i].name == "Mid" && !ml.elements[i].checked) {
		return false;
	    }
	}
	return true;
    }
    
//第2組勾選欄

   function Select2(e)
    {
	if (e.checked) {
	    document.dataList.toggleAll2.checked = AllChecked2();
	}
	else {
	    document.dataList.toggleAll2.checked = false;
	}
    }

    function ToggleAll2(e)
    {
	if (e.checked) {
	    CheckAll2();
	}
	else {
	    ClearAll2();
	}
    }

    function CheckAll2()
    {
	var ml = document.dataList;
	var len = ml.elements.length;
	for (var i = 0; i < len; i++) {
	    var e = ml.elements[i];
	    if (e.name == "Mid2") {
		Check(e);
	    }
	}
	ml.toggleAll2.checked = true;
    }

    function ClearAll2()
    {
	var ml = document.dataList;
	var len = ml.elements.length;
	for (var i = 0; i < len; i++) {
	    var e = ml.elements[i];
	    if (e.name == "Mid2") {
		Clear(e);
	    }
	}
	ml.toggleAll2.checked = false;
    }

    function AllChecked2()
    {
	ml = document.dataList;
	len = ml.elements.length;
	for(var i = 0 ; i < len ; i++) {
	    if (ml.elements[i].name == "Mid2" && !ml.elements[i].checked) {
		return false;
	    }
	}
	return true;
    }