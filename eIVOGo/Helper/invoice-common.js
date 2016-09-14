function showInvoiceModal(docID) {
    var element = event.target;
    $.post('<%= VirtualPathUtility.ToAbsolute("~/Helper/ShowInvoiceModal.aspx")%>', 'docID=' + docID, function (html) {
        //            $(element).after($(html).find("#mainContent"));
        $(html).find("#mainContent").dialog({ width: 640 });
    });
}

function showAllowanceModal(docID) {
    var element = event.target;
    $.post('<%= VirtualPathUtility.ToAbsolute("~/Helper/ShowAllowanceModal.aspx")%>', 'docID=' + docID, function (html) {
        //            $(element).after($(html).find("#mainContent"));
        $(html).find("#mainContent").dialog({ width: 640 });
    });
}
