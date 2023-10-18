import socket
import korpus
from KoBARTchatbot import kobart_chit_chat as csit
import os
from multiprocessing import Process, freeze_support
import subprocess
import sys
import socketcom as sockc
import requests
import threading

lock = threading.Lock()
comswit: int = 1 
ipcsendt: int = 1
loadfin: int = 0


def chat():
    freeze_support()
    parser = csit.parser
    parser = csit.Base.add_model_specific_args(parser)
    parser = csit.ArgsBase.add_model_specific_args(parser)
    parser = csit.ChatDataModule.add_model_specific_args(parser)
    parser = csit.pl.Trainer.add_argparse_args(parser)
    
    args = parser.parse_args()
    args.gradient_clip_val = 1.0
    args.max_epochs = 5
    args.gpus = -1
    
    args.checkpoint_path = './logs/default/version_1/checkpoints/kobart_chitchat-last.ckpt'
    hparams = csit.yaml.load(open('./logs/default/version_1/hparams.yaml'))
    csit.logging.info(args)
    model = csit.KoBARTConditionalGeneration.load_from_checkpoint(args.checkpoint_path, hparams=hparams)
    
    if comswit == 1:
        if args.chat:
            model.model.eval()
            loadfin = loadchecker(model)
            if ipcsendt == 1:
                HOST = '127.0.0.1'
                PORT = 47966
                server_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
                server_socket.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
                server_socket.bind((HOST, PORT))
                server_socket.listen()
                global qtls
                global qtout
                global telvar
                qtls = ""
                qtout = ""
                telvar = 0

                while 1:
                    client_socket, addr = server_socket.accept()
                    while True:
                        try:
                            if telvar == 0:
                                data = client_socket.recv(1024)
                                if not data:
                                    break
                                else:
                                    qtls = data.decode('utf8', errors='replace')
                                    telvar = 1
                            elif telvar == 1:
                                q = qtls.strip()
                                qtout = model.chat(q).replace('<usr> ', '') + "\n"
                                qtls = ""
                                telvar = 2

                                #if q == '!이제 그만 종료할게':
                                #    break
                                #q = ""

                            elif telvar == 2 and qtout != "" and qtls == "":
                                url = 'http://127.0.0.1:5000/kjtranslate?text=' + qtout
                                client_socket.sendall(qtout.encode('utf-8'))
                                qtout = ""
                                telvar = 0
                                transres = requests.get(url).text
                                target_server = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
                                target_server.connect((HOST, 47965))
                                target_server.sendall(transres.encode('utf-8'))
                                target_server.close()
                                
                        except ConnectionResetError as e:
                            break
                            
                    client_socket.close()
                server_socket.close()


def loadchecker(model):
    if loadfin == 0:
        model.chat("load")
        print("Load complete")
        return 1
