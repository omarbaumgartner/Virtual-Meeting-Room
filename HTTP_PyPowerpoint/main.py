import flask  # Serveur HTTP
import env
from functions import *

header = '''<!DOCTYPE html>
        <html lang="fr">
        <head>
        <meta charset="utf-8">
        <title>Document HTML</title>
        <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css" integrity="sha384-ggOyR0iXCbMQv3Xipma34MD+dH/1fQ784/j6cY/iJTQUOhcWr7x9JvoRxT2MZw1T" crossorigin="anonymous">
        </head>
        <body>
        <br>
        <div class="d-flex align-items-center">
        <div class="container card text-center ">
        <div class="row justify-content-center">
        <dv class="col ">
        '''
footer = '''
        <hr>
        </div>
        </div>
        </div>
        </div>
        <footer>
            
        </footer>
        <script src="https://code.jquery.com/jquery-3.3.1.slim.min.js" integrity="sha384-q8i/X+965DzO0rT7abK41JStQIAqVgRVzpbzo5smXKp4YfRvH+8abtTE1Pi6jizo" crossorigin="anonymous"></script>
        <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.7/umd/popper.min.js" integrity="sha384-UO2eT0CpHqdSJQ6hJty5KVphtPhzWj9WO1clHTMGa3JDZwrnQq4sF86dIHNDz0W1" crossorigin="anonymous"></script>
        <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/js/bootstrap.min.js" integrity="sha384-JjSmVgyd0p3pXB1rRibZUAYoIIy6OrQ6VrjIEaFf/nJGzIxFDsf4x0xIM+B07jRM" crossorigin="anonymous"></script>
        <br>
        </body>
        </html>'''

print("Initialisation...")
init()
print("Listage des fichiers et pointage du dernier élément")
files = getSharedFiles()
if len(files)>0 :
    env.filePointer = files[-1] 
else:
    env.filePointer = "Aucun fichier sélectionné"
app = flask.Flask(__name__)


@app.route('/', methods=['GET', 'POST'])
def home():
    if flask.request.method == 'GET':
        files = getSharedFiles()
        body = '''
            <hr>
            <div class="container">
                <div class="row">
                    <div class="col">
                        <form method="POST" action="/" enctype="multipart/form-data">
                        <label class="form-label" for="customFile">Insérer un nouveau fichier PPT</label>
                        <div class="container">
                            <div class="row">
                                <div class="col">
                                    <input type="file" class="form-control" id="customFile" name="file"/>
                                </div>
                                <div class="col-auto">
                                    <button class="btn btn-dark" type="submit">Submit</button>
                                </div>
                            </div>
                        </div>
                        </form>
                        <br>
                        <p class="h6">Fichier actuellement partagé : <span class="text-success">''' + env.filePointer + '''</span>
                    </div>           
                    <div class="col">
                        <div class="container">
                            <div class="row d-flex justify-content-center">
                                <p class="h4"> Fichiers disponibles sur le serveur : </p>
                                <br>
                                <ul class="list-group">
            '''
        if len(files) > 0:
            for fName in files:
                if env.filePointer == fName:
                    body += '<li class="list-group-item active"><a class="text-dark" href="/changeto/'+fName+'">'+fName+'</a></li>'
                else:
                    body += '<li class="list-group-item"><a class="text-dark" href="/changeto/'+fName+'">'+fName+'</a></li>'
        else:
            body += '<li class="list-group-item"><a class="text-dark"> Pas de fichiers disponibles </a></li>'
        body += '''
                                </ul>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            '''
        return header+body+footer

    elif flask.request.method == 'POST':
        uploaded_file = flask.request.files['file']
        if uploaded_file.filename != '':
            uploaded_file.save(uploaded_file.filename)
            print("Convesion PPT --> PDF")
            pdfPath = PPTtoPDF(uploaded_file.filename)
            print("Convesion PDF --> IMGS")
            images_list = PDFtoImgs(pdfPath, poppler_path)
            print("Fusion des images --> Image")
            mergeImgs(uploaded_file.filename, images_list)
            return flask.redirect(flask.url_for('home'))


@app.route('/shared', methods=['GET'])
def returnSharedFile():
    return flask.send_file(toShareFolder+'\\'+env.filePointer, mimetype='image/gif')


@app.route('/changeto/<filename>', methods=['GET'])
def changeShareFile(filename):
    print("Changing pointer to ",filename)
    env.filePointer = filename
    print("filepoiner",env.filePointer)
    return flask.redirect(flask.url_for('home'))


if __name__ == '__main__':
    app.run(host=env.serverIp,port=env.serverPort)
