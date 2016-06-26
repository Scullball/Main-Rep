Simple code. Main application logic written on javaScript. Server side(c#) used
	only for storing to database and render page.
	Perfectly working on “firefox” with CORSE(cross-origin-requests-enabled),
	rendering beautifully 3d graphs in real-time, as needed.

	 Warning! required Firefox and https://addons.mozilla.org/ru/firefox/addon/cors-everywhere/

	


	Вся основная логика программы находится в двух файлах: 
	~/Views/Shared/_ScriptsPartial.cshtml
	~/Scripts/project-functions.js

	~/Scripts/project-graphics.js, project-tables.js - реализация графиков

	Кросс-доменные запросы разрешены на сервере Global.asax(Application_BeginRequest);
	Если у вас браузер не будет их блокировать, все будет работать. 
	В противном случае Firefox + https://addons.mozilla.org/ru/firefox/addon/cors-everywhere/.
	
	Графики использовал highcharts http://www.highcharts.com/demo/3d-column-stacking-grouping
	
	