import env
from env import toShareFolder,pathToProgram
import comtypes
import comtypes.client  # PPT to PDF
from PIL import Image  # Merge images
from pdf2image import convert_from_path  # PDF to IMG
poppler_path = "C:\\Users\\omari\\Desktop\\PyConverter\\poppler-21.03.0\\Library\\bin"  # PDF to IMG
from os import listdir  # list files in directory
import os  # Directory if not exists
from os.path import isfile, join

# Vérification et création du/des dossiers si nécessaire
def init():
    if not os.path.exists(toShareFolder):
        os.makedirs(toShareFolder)
    
# Lister les fichiers disponibles dans le répertoire Shared
def getSharedFiles():
    filesName = []
    shareFiles = [f for f in listdir(
        toShareFolder) if isfile(join(toShareFolder, f))]
    for shareFile in shareFiles:
        filesName.append(shareFile)
    # Pointage vers le dernier fichier du répertoire
    return filesName

# Conversion PPT > PDF
def PPTtoPDF(filename, formatType=32):
    comtypes.CoInitialize()
    powerpoint = comtypes.client.CreateObject("Powerpoint.Application")
    print("Fichier à convertir :", filename)
    outputFilePath = pathToProgram+filename[:-5]
    powerpoint.Visible = 1
    if outputFilePath[-3:] != 'pdf':
        outputFileName = outputFilePath + ".pdf"
    print("Ouverture du powerpoint : ",pathToProgram+filename)
    deck = powerpoint.Presentations.Open(pathToProgram+filename)
    print("Enregistrement du PDF : ",outputFileName)

    deck.SaveAs(outputFileName, formatType)  # formatType = 32 for ppt to pdf
    deck.Close()
    print("Destination : ", outputFileName)
    powerpoint.Quit()
    print("Suppression du fichier",filename)
    os.remove(filename)
    
    return filename[:-5]+ ".pdf"

# Conversion PDF > Imgs
def PDFtoImgs(filename, poppler_path):
    # Store Pdf with convert_from_path function
    images = convert_from_path(
        pdf_path=filename, poppler_path=poppler_path)
    images_list = []
    for (i, page) in enumerate(images):
        page.save(filename+str(i+1)+'.png', 'PNG')
        images_list.append(filename+str(i+1)+'.png')
    print("Suppression du fichier",filename)
    os.remove(filename)
    return images_list

# Fusion des images
def mergeImgs(filename, images_list):
    imgs = [Image.open(i) for i in images_list]
    min_img_width = min(i.width for i in imgs)
    total_height = 0
    for (i, img) in enumerate(imgs):
        # If the image is larger than the minimum width, resize it
        if img.width > min_img_width:
            imgs[i] = img.resize(
                (min_img_width, int(img.height / img.width * min_img_width)), Image.ANTIALIAS)

        total_height += imgs[i].height

    # Now that we know the total height of all of the resized images, we know the height of our final image
    img_merge = Image.new(imgs[0].mode, (min_img_width, total_height))
    y = 0
    for (i,img) in enumerate(imgs):
        img_merge.paste(img, (0, y))
        y += img.height
        print("Suppression du fichier",images_list[i])
        os.remove(images_list[i])
    img_merge.save(toShareFolder+'/'+filename+'_merged.jpg')


