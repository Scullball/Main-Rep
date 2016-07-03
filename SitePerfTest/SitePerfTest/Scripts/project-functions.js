

function getSitemaps(url) {
    var validUrl = validateUrl(url);
    if (!validUrl)
    {
        $(".console-body").append("<br>please, enter correct url").css('color', 'red');
    }
    else if (validUrl.indexOf("sitemap.xml") >= 0)
    {
        var urlsArr = xmlParse(getXml(validUrl));
        if (urlsArr)
        {
            $(".console-body").append("<br>xml file received, starting test").css('color', 'black');
            return urlsArr;
        }
    }
    else
    {      
        var urlsArr = getSiteUrls(validUrl);
        if (urlsArr)
        {
            $(".console-body").append("<br>sitemap create, starting test").css('color', 'black');
            return urlsArr;
        }
    }
    
};

//Подбирает протокол и пытается послать запрос по переданному адресу.Возвращает валидный адресс или false(ошибку).
//url - адресс введенный юзером
function validateUrl(url) {
    if (testCall("http://" + url + "/sitemap.xml"))
    {
        return "http://" + url + "/sitemap.xml";
    }
    else if (testCall("https://" + url + "/sitemap.xml"))
    {
        return "https://" + url + "/sitemap.xml";
    }
    else if (testCall("http://" + url))
    {
        return "http://" + url;
    }
    else if (testCall("https://" + url))
    {
        return "https://" + url;
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
        url: '/Home/TestCall',
        data: { url: url },

        success: function (data) {
            result = true;
        },
        error: function (data) {
            result = false;
        },
    });
    return result;
};

//Разбирает файл xml, доставая нужные нам адреса и складывая их в массив
//xml - результат работы getXml()
function xmlParse(xml) {
    if (xml) {
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
    }
    else {
        return false;
    }
    
};

//ajax(синхронный) возвращает запрошенный sitemap.xml
//url- полученный после валидации validateUrl()
function getXml(url) {
    var xml;
    $.ajax({
        type: "GET",
        async: false,   
        url: '/Home/GetXml',
        data: { url: url },

        success: function (data) {
            xml = data;
        },
        error: function () {
            $(".console-body").append("<br>get xml fail").css('color', 'red');
            xml = false;
        },
    });
    return xml;
};

//Вызывается on.click, после, функцией setPoint при определенном значении счетчика.
//установлен таймаут(1 секунду), для менее агрессивного ddosa.
function loop(counter) {
    setTimeout(ddos,1000,arr[counter], counter);
}

//Четыре ajaxa меряют впемя отклика, и передают в массив.Счетчик пробрасывается, для вызова следующей ф-ции.
function ddos(url, count) {
    var pointsArr = [];
    var start = new Date;
    $.when(
    $.ajax({
        type: "GET",
        async: true,
        url: '/Home/FirstRequest',
        data: { url: url },

        success: function (data) {
            var end = new Date;
            var y = (end - start) / 1000;
            pointsArr.push(y);

        },
        error: function (data) {
            $(".console-body").append("<br>ddos(1)-" + data).css('color', 'red');
        },
    }),

    $.ajax({
        type: "GET",
        async: true,
        url: '/Home/SecondRequest',
        data: { url: url },

        success: function (data) {
            var end = new Date;
            var y = (end - start) / 1000;
            pointsArr.push(y);

        },
        error: function (data) {
            $(".console-body").append("<br>ddos(2)-" + data).css('color', 'red');
        },
    }),


    $.ajax({
        type: "GET",
        async: true,
        url: '/Home/ThirdRequest',
        data: { url: url },

        success: function (data) {
            var end = new Date;
            var y = (end - start) / 1000;
            pointsArr.push(y);

        },
        error: function (data) {
            $(".console-body").append("<br>ddos(3)-" + data).css('color', 'red');
        },
    }),


    $.ajax({
        type: "GET",
        async: true,
        url: '/Home/FourthRequest',
        data: { url: url },

        success: function (data) {
            var end = new Date;
            var y = (end - start) / 1000;
            pointsArr.push(y);

        },
        error: function (data) {
            $(".console-body").append("<br>ddos(4)-" + data).css('color', 'red');
        },
    })
    ).then(function () {
        setPoint(pointsArr,url ,count);
    });
};

//рисует точку на графиках, заполняет таблицу.Выводит сообщение о возможности сохранить в базу результат теста.
function setPoint(pointsArr, url, count) {
    var min = Math.min.apply(Math, pointsArr);
    var max = Math.max.apply(Math, pointsArr);
    //оставляем 3 знака после запятой
    var point = parseFloat(((max + min) / 2).toFixed(3));
    //Значения складываем в глобальный массив DbDataArr для последующей,возможной передачи в базу.
    DbDataArr.push(point);
    //вывод значений в таблицы
    if (count < 10) {
        $('.compare-data-table-1').css('display', 'inline-block');
        $('.compare-data-table-body-1 tr:last').after('<tr><td>' + count + '</td><td>' + url + '</td><td>' + min + '</td><td>' + max + '</td><td>' + point + '</td></tr>');
    } else {
        $('.compare-data-table-2').css('display', 'inline-block');
        $('.compare-data-table-body-2 tr:last').after('<tr><td>' + count + '</td><td>' + url + '</td><td>' + min + '</td><td>' + max + '</td><td>' + point + '</td></tr>');
    }
    var time = (new Date()).getTime();
    // рисуем точку на графике
    chart.series[0].addPoint({x:time, y:point, url:url}, true, true);
    //определяем min,max для колонок.
    column.series[1].addPoint({y:min,url:url});
    column.series[0].addPoint({y:max,url:url});
    if (count < 19) {
        count++;
        loop(count);
    }
    else {
        $(".console-body").append("<br>test finished sucsessfuly.Save test?").css('color','black');
        $('.annotation').css({"opacity": "1", "transform": "scale(1)" });
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

//срабатывает вслучае, если программа не находит sitemap.xml.
//возвращает массив url-ов
function getSiteUrls(url) {
    var retArr = [];
    $.ajax({
        type: "GET",
        async: false,
        url: '/Home/GetSiteUrls',
        data: { siteUrl: url },

        success: function (data) {
            retArr = data.split(',');
        },
        error: function (data) {
            $(".console-body").append("<br>getSiteUrls bad request").css('color', 'red');
            retArr = false;
        }
    })
    return retArr;
};