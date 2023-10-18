import librosa
import matplotlib.pyplot as plt
import IPython.display as ipd
import os
import json
import math
import requests
import torch
from torch import nn
from torch.nn import functional as F
from torch.utils.data import DataLoader
import commons
import utils
from data_utils import TextAudioLoader, TextAudioCollate, TextAudioSpeakerLoader, TextAudioSpeakerCollate
from models import SynthesizerTrn
from text.symbols import symbols
from text import text_to_sequence
import pyaudio
import numpy as np
from numpy.fft import fft, ifft
from scipy.io.wavfile import write
import re
from scipy import signal
import socket
import matplotlib.font_manager as fm
import testreader

plt.rcParams['font.family'] = 'MS Gothic'


def get_text(text, hps):
    text_norm = text_to_sequence(text, hps.data.text_cleaners)
    if hps.data.add_blank:
        text_norm = commons.intersperse(text_norm, 0)
    text_norm = torch.LongTensor(text_norm)
    return text_norm


def vcinit(inputstr):
    stn_tst = get_text(inputstr, hps)
    with torch.no_grad():
        x_tst = stn_tst.cuda().unsqueeze(0)
        x_tst_lengths = torch.LongTensor([stn_tst.size(0)]).cuda()
        audio = net_g.infer(x_tst, x_tst_lengths, noise_scale=.667, noise_scale_w=0.8, length_scale=1)[0][0, 0].data.cpu().float().numpy()


def vcfft(inputstr):
    fltstr = re.sub(r"[\[\]\(\)\{\}]", "", inputstr)
    stn_tst = get_text(fltstr, hps)
    
    with torch.no_grad():
        x_tst = stn_tst.cuda().unsqueeze(0)
        x_tst_lengths = torch.LongTensor([stn_tst.size(0)]).cuda()
        audio = net_g.infer(x_tst, x_tst_lengths, noise_scale=.667, noise_scale_w=0.8, length_scale=1)[0][
            0, 0].data.cpu().float().numpy()

    p = pyaudio.PyAudio()
    stream = p.open(format=pyaudio.paInt16, channels=1, rate=22050, output=True)
    stream.start_stream()
    audio = (audio * 32767).astype(np.int16)
    chunk_size = 1024
    x = np.arange(0, chunk_size) * 22050 / (chunk_size)
    data_into = np.zeros(1024, dtype=np.int16)
    
    for i in range(0, len(audio), chunk_size):
        chunk = audio[i:i + chunk_size].astype(np.int16).tobytes()
        ele = fft(data_into)
        xele = len(ele)
        n = np.arange(xele)
        T = xele / 22050
        slip = testreader.spectralm2(np.abs(ele))
        target_server = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        target_server.connect(('127.0.0.1', 17469))
        target_server.sendall(slip.encode('utf-8'))
        target_server.close()
        stream.write(chunk)
        
    stream.stop_stream()
    stream.close()
    p.terminate()


hps = utils.get_hparams_from_file("./configs/arona.json")
net_g = SynthesizerTrn(
    len(symbols),
    hps.data.filter_length // 2 + 1,
    hps.train.segment_size // hps.data.hop_length,
    **hps.model).cuda()
_ = net_g.eval()

_ = utils.load_checkpoint("./models/arona/G_129000.pth", net_g, None)

input=""
vcinit(input)

HOST = '127.0.0.1'
PORT = 47965
server_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
server_socket.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
server_socket.bind((HOST, PORT))
server_socket.listen()
global qtls 
global telvar
qtls = ""
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
                q = q.rstrip()
                q.replace("-","")
                vcfft(q)
                qtls = ""
                telvar = 0
                
        except ConnectionResetError as e:
            break
            
    client_socket.close()
server_socket.close()

