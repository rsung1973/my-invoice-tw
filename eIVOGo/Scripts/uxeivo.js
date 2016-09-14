(function (window) {
    if (window.$eivo != undefined)
        return;
    window.$eivo = {
        createJsConfig: function () {

            var config = {
                height: "auto",
                width: "100%",

                autoload: true,
                paging: true,
                pageLoading: true,
                pageSize: 10,
                pageButtonCount: 10,
                pageIndex: 1,
                pagerFormat: "{first} | {prev} | {pages} | {next} | {last} &nbsp;&nbsp; {pageIndex} / {pageCount} &nbsp;&nbsp;&nbsp;&nbsp; 總筆數： {itemCount}",
                pageNextText: "下一頁",
                pagePrevText: "上一頁",
                pageFirstText: "首頁",
                pageLastText: "末頁",
                pageNavigatorNextText: "下10頁",
                pageNavigatorPrevText: "上10頁",
                noDataContent: "查無資料!!",

                controller: {},

                fields: []
            };
            return config;
        },

        preparJsGrid: function (jsGrid, fields, fieldsContainer) {

            var $jsGrid = $(jsGrid);
            var $gridConfig = this.createJsConfig();
            var $fieldIdx = [];
            $gridConfig.fields = fields;

            var $container = $(fieldsContainer);
            $container.addClass("js-fields-container");

            var $header = $('<div>').addClass("js-fields-header")
                    .append($('<span>').text('☰'));
            $container.append($header);

            var $content = $('<div>').addClass("js-fields-content");
            $container.append($content);

            for (var i = 0; i < $gridConfig.fields.length; i++) {
                var chkBox = $fieldIdx[$fieldIdx.length] =
                    $('<input type="checkbox" name="colIdx" checked />').val(i);
                $content.append(chkBox)
                    .append($('<span>').text($gridConfig.fields[i].title + ' '));
            }

            $header.click(function () {

                $this = $(this);
                //getting the next element
                $content = $this.next();
                //open up the content needed - toggle the slide- if visible, slide up, if not slidedown.
                $content.slideToggle(500, function () {
                    if (!$content.is(":visible")) {
                        var $newFields = [];
                        //$('input[name="colIdx"]:checked').each(function () {
                        //    $newFields[$newFields.length] = $gridConfig.fields[parseInt($(this).val())];
                        //});
                        $($fieldIdx).each(function () {
                            if (this.is(':checked')) {
                                $newFields[$newFields.length] = $gridConfig.fields[parseInt(this.val())];
                            }
                        });

                        $jsGrid.jsGrid("option", "fields", $newFields);
                    }
                });
            });

            return {
                jsGrid: $jsGrid,
                gridConfig: $gridConfig
            };
        },

        resetJsGridFields: function (fields) {
            var $newFields = [];
            $('input[name="colIdx"]:checked').each(function () {
                $newFields[$newFields.length] = fields[parseInt($(this).val())];
            });
            return $newFields;
        }
    };
})(window);