from PIL import Image
import sys

class ImageResizer:
    def image(self, imageName, modPath, size):
        if size == 'e':
            im = Image.open(modPath + "/DisplayImages/ImageFiles/rawImage/" + imageName + ".png")
            new_size = im.resize((128, 128))
            new_size.save(modPath + "/DisplayImages/ImageFiles/128x/" + imageName + ".png")
        if size == 'h':
            im = Image.open(modPath + "/DisplayImages/ImageFiles/rawImage/" + imageName + ".png")
            new_size = im.resize((96, 96))
            new_size.save(modPath + "/DisplayImages/ImageFiles/96x/" + imageName + ".png")
        if size == 'b':
            im = Image.open(modPath + "/DisplayImages/ImageFiles/rawImage/" + imageName + ".png")
            new_size = im.resize((64, 64))
            new_size.save(modPath + "/DisplayImages/ImageFiles/64x/" + imageName + ".png")
        if size == 's':
            im = Image.open(modPath + "/DisplayImages/ImageFiles/rawImage/" + imageName + ".png")
            new_size = im.resize((32, 32))
            new_size.save(modPath + "/DisplayImages/ImageFiles/1/" + imageName + ".png")

classObj = ImageResizer()
imageName = str(sys.argv[1])
modPath = str(sys.argv[2])
size = str(sys.argv[3])
classObj.image(imageName, modPath, size)