# DisplayImages Mod
+ A mod for DuckGame where you can display / resize / and crop images.
+ Integrated UI - press F7 to open the menu.
+ If you want to compile the .dll from source you need to create the missing folders
    1. inside DisplayImages create the 'ImageFiles' folder
    2. inside ImageFiles create the following empty folders: 'CustomSize', 'rawImages', '1', '4', '9', '64x', '96x'
       
+ Compatible with DuckGameRebuilt.  
## Commands for DevConsole:
1. ```bitmap <image name> <image pieces>```
    + example: ```bitmap image 3```  -> displays an (3x3) grid image
3. ```resize <image name> <image size>```
    + image sizes:
      + s = 32x32 pixel
      + b = 64x64 pixel
      + h = 96x96 pixel
      + leave blank in order to resize the image automatically to the next possible 32x32 grid
      + p = custom size. Example: resize image p 300 300  -> resizes 'image' to the specified width and height.
> [!NOTE]
> when using dev console commands, images have to be located inside the DisplayImages/ImageFiles/rawImages folder
3. ```split <image name> <image grid> (optional)<image grid>```
  + example: ```split image 3``` -> splits image into a 3x3 grid
