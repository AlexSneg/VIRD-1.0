call "%VS90COMNTOOLS%\..\..\VC\vcvarsall.bat" x86
svcutil.exe /reference:bin\Debug\Entity.dll /reference:bin\Debug\Locking.dll /reference:bin\Debug\TechnicalServices.Common.dll /reference:bin\Debug\Interfaces.dll /reference:bin\Debug\DesignCommon.dll net.tcp://localhost:789/DesignService/mex
pause