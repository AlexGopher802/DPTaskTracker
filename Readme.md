# TaskTracker

## Как запустить приложение?

* На всякий случай - скачать 7-ой дотнет (рантайм) `https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/runtime-aspnetcore-7.0.18-windows-x64-installer`
* Запустите docker
* Введите в консоль следующую команду: `docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=AMS25051980s34!" -p 1434:1433 --name sqlserver -h sqlserver -d alexgopher/sqlserver`
* Запустите файл по пути: `ApiBin/DPTaskTracker.Api.exe`
* Запустите файл по пути: `DesktopBin/DPTaskTracker.Desktop.exe`
* Учетка админа: `Alex` `123`