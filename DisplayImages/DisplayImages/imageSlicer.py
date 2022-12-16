import cv2
import sys
import os


class ImageSlicer:
    def function(self, imageName, path, grid):
        mod_entry_path = path

        if grid == '4':
            #0|0
            #0|0
            image64x = cv2.imread(mod_entry_path + "/DisplayImages/ImageFiles/64x/" + imageName + ".png", 1)
            (height, width) = image64x.shape[:2]
            centerX = width // 2
            centerY = height // 2

            topLeft = image64x[0:centerY, 0:centerX]
            topRight = image64x[0:centerY, centerX: width]
            bottomLeft = image64x[centerY:height, 0:centerX]
            bottomRight = image64x[centerY:height, centerX:width]

            os.chdir(mod_entry_path + "/DisplayImages/ImageFiles/4/")
            os.system('mkdir ' + imageName)

            cv2.imwrite(mod_entry_path + "/DisplayImages/ImageFiles/4/" + imageName + "1.png", topLeft)
            cv2.imwrite(mod_entry_path + "/DisplayImages/ImageFiles/4/" + imageName + "2.png", topRight)
            cv2.imwrite(mod_entry_path + "/DisplayImages/ImageFiles/4/" + imageName + "3.png", bottomLeft)
            cv2.imwrite(mod_entry_path + "/DisplayImages/ImageFiles/4/" + imageName + "4.png", bottomRight)

        if grid == '9':
            #0|0|0
            #0|0|0
            #0|0|0
            image96x = cv2.imread(mod_entry_path + "/DisplayImages/ImageFiles/96x/" + imageName + ".png", 1)
            (height, width) = image96x.shape[:2]
            centerX = width // 3
            centerY = height // 3

            #position -> from : to
            topLeft = image96x[0:centerY, 0:centerX]
            topCenter = image96x[0:centerY, centerX:(width-centerX)]
            topRight = image96x[0:centerY, centerX*2:width]

            midLeft = image96x[centerY:(height-centerY), 0:centerX]
            midCenter = image96x[centerY:(height-centerY), centerX:(width-centerX)]
            midRight = image96x[centerY:(height-centerY), centerX*2:width]

            bottomLeft = image96x[centerY*2:height, 0:centerX]
            bottomCenter = image96x[centerY*2:height, centerX:(width-centerX)]
            bottomRight = image96x[centerY*2:height, centerX*2:width]

       
            os.chdir(mod_entry_path + "/DisplayImages/ImageFiles/9/")
            os.system('mkdir ' + imageName)

            cv2.imwrite(mod_entry_path + "/DisplayImages/ImageFiles/9/" + imageName + "/" + imageName + "1.png", topLeft)
            cv2.imwrite(mod_entry_path + "/DisplayImages/ImageFiles/9/" + imageName + "/" + imageName + "2.png", topCenter)
            cv2.imwrite(mod_entry_path + "/DisplayImages/ImageFiles/9/" + imageName + "/" + imageName + "3.png", topRight)

            cv2.imwrite(mod_entry_path + "/DisplayImages/ImageFiles/9/" + imageName + "/" + imageName + "4.png", midLeft)
            cv2.imwrite(mod_entry_path + "/DisplayImages/ImageFiles/9/" + imageName + "/" + imageName + "5.png", midCenter)
            cv2.imwrite(mod_entry_path + "/DisplayImages/ImageFiles/9/" + imageName + "/" + imageName + "6.png", midRight)

            cv2.imwrite(mod_entry_path + "/DisplayImages/ImageFiles/9/" + imageName + "/" + imageName + "7.png", bottomLeft)
            cv2.imwrite(mod_entry_path + "/DisplayImages/ImageFiles/9/" + imageName + "/" + imageName + "8.png", bottomCenter)
            cv2.imwrite(mod_entry_path + "/DisplayImages/ImageFiles/9/" + imageName + "/" + imageName + "9.png", bottomRight)

classObj = ImageSlicer()
# Arguments. API for c# data
imageName = str(sys.argv[1])
path = str(sys.argv[2])
grid = str(sys.argv[3])
classObj.function(imageName, path , grid)
