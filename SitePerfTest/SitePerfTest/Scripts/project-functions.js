

function getSitemaps(url) {
    var validUrl = validateUrl(url);
    if (!validUrl)
    {
        $(".console-body").append("<br>please, enter the valid url").css('color','red');
    }
    else
    {
        var urlsArr = xmlParse(getXml(validUrl));
        $(".console-body").append("<br>xml file received, starting test").css('color','black');
        return urlsArr;
    }
    
};

//Подбирает протокол и пытается послать запрос по переданному адресу.Возвращает валидный адресс или false(ошибку).
//url - адресс введенный юзером
function validateUrl(url) {
    if (testCall("http://" + url + "/sitemap.xml"))
    {
        var validUrl = "http://" + url + "/sitemap.xml";
        return validUrl;
    }
    else if (testCall("https://" + url + "/sitemap.xml"))
    {
        var validUrl = "https://" + url + "/sitemap.xml";
        return validUrl;
    }
    else
    {
        return false;
    }
};
//Обычный ajax(синхронный) с соответствующими колбэками в случае успеха или неудачи.
//url - адресс введенный юзером
function testCall(url) {
    var result = false;
    $.ajax({
        type: "GET",
        async: false,
        url: url,

        success: function () {
            result = true;
        },
        error: function () {
            result = false;
        },
    });
    return result;
};

//Разбирает файл xml, доставая нужные нам адреса и складывая их в массив
//xml - результат работы getXml()
function xmlParse(xml) {
    var sitemapsArray = [];
    $(xml).find("sitemap").each(function () {
        $(this).find("loc").each(function () {
            sitemapsArray.push($(this).text());
        })
    })
    var urlForDdosArray = [];
    sitemapsArray.forEach(function (item) {
        var xml = getXml(item);
        $(xml).find("loc").each(function () {
            urlForDdosArray.push($(this).text());
        })
    })
    return urlForDdosArray;
};

//ajax(синхронный) возвращает запрошенный sitemap.xml
//url- полученный после валидации validateUrl()
function getXml(url) {
    var xml;
    $.ajax({
        type: "GET",
        async: false,   
        url: url,

        success: function (data) {
            xml = data;
        },
        error: function (xhr) {
            $(".console-body").append("<br>get xml"+ xhr.status).css('color','red');
        },
    });
    return xml;
};

//Вызывается on.click, после, функцией setPoint при определенном значении счетчика.
function loop(counter) {
    ddos(arr[counter], counter);
}

//Четыре ajaxa меряют впемя отклика, и передают в массив.Счетчик пробрасывается, для вызова следующей ф-ции.
function ddos(url, count) {
    var pointsArr = [];
    var start = new Date;
    $.when(
    $.ajax({
        type: "GET",
        async: true,
        url: url,

        success: function () {
            var end = new Date;
            var y = (end - start) / 1000;
            pointsArr.push(y);

        },
        error: function (xhr) {
            $(".console-body").append("<br>get xml" + xhr.status).css('color', 'red');
        },
    }),

    $.ajax({
        type: "GET",
        async: true,
        url: url,

        success: function () {
            var end = new Date;
            var y = (end - start) / 1000;
            pointsArr.push(y);

        },
        error: function (xhr) {
            $(".console-body").append("<br>get xml" + xhr.status).css('color', 'red');
        },
    }),

    $.ajax({
        type: "GET",
        async: true,
        url: url,

        success: function () {
            var end = new Date;
            var y = (end - start) / 1000;
            pointsArr.push(y);

        },
        error: function (xhr) {
            $(".console-body").append("<br>get xml" + xhr.status).css('color', 'red');
        },
    }),

    $.ajax({
        type: "GET",
        async: true,
        url: url,

        success: function () {
            var end = new Date;
            var y = (end - start) / 1000;
            pointsArr.push(y);

        },
        error: function (xhr) {
            $(".console-body").append("<br>get xml" + xhr.status).css('color', 'red');
        },
    })
    ).then(function () {
        setPoint(pointsArr, count);
    });
};

//рисует точку на графиках.Выводит сообщение о возможности сохранить в базу результат теста.
function setPoint(pointsArr, count) {
    var min = Math.min.apply(Math, pointsArr);
    var max = Math.max.apply(Math, pointsArr);
    //оставляем 3 знака после запятой
    var point = parseFloat(((max + min) / 2).toFixed(3));
    //Значения складываем в глобальный массив DbDataArr для последующей,возможной передачи в базу.
    DbDataArr.push(point);
    var x = (new Date()).getTime();
    chart.series[0].addPoint([x, point], true, true);
    column.series[1].addPoint(min);
    column.series[0].addPoint(max);
    if (count < 19) {
        count++;
        loop(count);
    }
    else {
        $(".console-body").append("<br>test finished sucsessfuly.Save test?").css('color','black');
        $('.annotation').css({ "opacity": "1", "transform": "scale(1)" });
        $('.test-title-form').css('display', 'inline');
        $('.save').css('display', 'inline');
    }
};

//ajax - записывает результат теста в бд
function storeResultToBd(title, data) {
    data = data.toString();
    $.ajax({
        type: "GET",
        async: true,
        url: '/Home/SaveToBd',
        data: { title: title, data: data },

        success: function (message) {
            $(".console-body").append("<br>"+message).css('color','black');
        },
        error: function (xhr) {
            $(".console-body").append("<br>data store error on cliet side" + xhr.status).css('color', 'red');
        },
    })
};