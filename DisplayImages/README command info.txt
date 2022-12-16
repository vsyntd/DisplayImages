this mod uses commands for duckgame console. 
Press the '~' (tilde) key on US English keyboard layouts.

the <image grid> parameter can take numbers 2 or 3 -> 2x2, 3x3
the <image size> parameter can take letters s,b,h
s = small (32x32) 1 image
b = big   (64x64) 4 imges
h = huge  (96x96) 9 images

p = personal -> indicator for custom image Size resizing

after excecuting the bitmap command just click on the screen and the image should be on the cursors location.

Commands:

show -> shows cursor 
hide -> hides cursor 

bitmap <image name> <image pieces> -> to activate image displayer image size= 1/4/9
example: bitmap roll 1      
example: bitmap astley 4

resize <image name> <image size>  -> resize image from rawImage folder
example: resize wisetree b

with the keyword 'p' you can resize the image to the width and height you want, but it wont serve any purpose yet. Those images will be stored in the CustomSize folder
example: resize wisetree p 128 100 (no usage, just for resizing purpose)

split <image name> <image grid> -> cuts a bigger image into smaller images
example: split wisetree 2        

clear -> remove all images