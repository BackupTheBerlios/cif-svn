set PATH=%SystemRoot%\system32;%SystemRoot%;%SystemRoot%\System32\Wbem;C:\Program Files\PVCS\Tracker\nt;C:\Program Files\Subversion\bin;..\Build\nAnt\bin
call "%Compile.DevEnv.vsvars32bat%"
devenv "%ProjectName%.sln"
