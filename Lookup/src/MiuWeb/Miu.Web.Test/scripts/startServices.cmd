@echo off 
::set apiRoot=C:\Users\enian\source\repos\MIU_ServiceProvider\src
set apiRoot=C:\Users\enianga\source\repos\MIU_ServiceProvider\src
::set webRoot=C:\Users\enian\source\repos\MIUS_SUAM\src
set webRoot=C:\Users\enianga\source\repos\MIUS_SUAM\src


::set webRoot=D:\source\repos\MIUS_SUAM\src


::echo "%apiRoot%\Services\MIU\Miu.Api"

start /d "%apiRoot%\Services\MIU\Miu.Api" dotnet run

start /d "%apiRoot%\ApiGateways\MiuApiGateway\Miu.ApiGw" dotnet run

start /d "%apiRoot%\Services\Identity\Identity.API" dotnet run

start /d "%apiRoot%\Services\VRQS\VRQS.Api" dotnet run

start /d "%webRoot%\\MiuWeb\Miu.Web" dotnet run

::PAUSE