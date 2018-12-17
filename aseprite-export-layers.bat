D:
cd \SteamLibrary\steamapps\common\Aseprite
:: dir /b /o:gn
@set ASEPRITE=Aseprite.exe
@set INPUT_NAME=level8
@set OUTPUT_NAME=level8
%ASEPRITE% -b --split-layers %INPUT_NAME%.aseprite --save-as %OUTPUT_NAME%-{layer}.png
timeout /t -1