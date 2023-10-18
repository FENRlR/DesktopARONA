# -*- coding: utf-8 -*-
from ctypes import WinDLL, c_char_p, c_int, c_wchar_p
from ctypes.wintypes import BOOL
from flask import Flask, request
from gevent.pywsgi import WSGIServer
import re

app = Flask(__name__)

path_dat = "path.txt"
with open(path_dat, 'r') as f:
    path = f.read().strip()

path_j2k = "pathj2k.txt"
with open(path_j2k, 'r') as f:
    pathj2k = f.read().strip()

path_k2j = "pathk2j.txt"
with open(path_k2j, 'r') as f:
    pathk2j = f.read().strip()

class TransJ2K:
    def initialize(self, engine):
        self.start = engine.J2K_InitializeEx
        self.start.argtypes = [c_char_p, c_char_p]
        self.start.restype = BOOL
        self.trans = engine.J2K_TranslateMMNTW
        self.trans.argtypes = [c_int, c_wchar_p]
        self.trans.restype = c_wchar_p
        self.start_obj = self.start(b"CSUSER123455", path.encode('utf-8'))

    def translate_j2k(self, src_text):
        trans_obj = self.trans(0, src_text)
        return trans_obj

engjk = TransJ2K()


class TransK2J:
    def initialize(self, engine):
        self.start = engine.K2J_InitializeEx
        self.start.argtypes = [c_char_p, c_char_p]
        self.start.restype = BOOL
        self.trans = engine.K2J_TranslateMMNTW
        self.trans.argtypes = [c_int, c_wchar_p]
        self.trans.restype = c_wchar_p
        self.start_obj = self.start(b"CSUSER123455", path.encode('utf-8'))

    def translate_k2j(self, src_text):
        trans_obj = self.trans(0, src_text)
        return trans_obj

engkj = TransK2J()


def decode_text(txt):
    chars = "↔◁◀▷▶♤♠♡♥♧♣⊙◈▣◐◑▒▤▥▨▧▦▩♨☏☎☜☞↕↗↙↖↘♩♬㉿㈜㏇™㏂㏘＂＇∼ˇ˘˝¡˚˙˛¿ː∏￦℉€㎕㎖㎗ℓ㎘㎣㎤㎥㎦㎙㎚㎛㎟㎠㎢㏊㎍㏏㎈㎉㏈㎧㎨㎰㎱㎲㎳㎴㎵㎶㎷㎸㎀㎁㎂㎃㎄㎺㎻㎼㎽㎾㎿㎐㎑㎒㎓㎔Ω㏀㏁㎊㎋㎌㏖㏅㎭㎮㎯㏛㎩㎪㎫㎬㏝㏐㏓㏃㏉㏜㏆┒┑┚┙┖┕┎┍┞┟┡┢┦┧┪┭┮┵┶┹┺┽┾╀╁╃╄╅╆╇╈╉╊┱┲ⅰⅱⅲⅳⅴⅵⅶⅷⅸⅹ½⅓⅔¼¾⅛⅜⅝⅞ⁿ₁₂₃₄ŊđĦĲĿŁŒŦħıĳĸŀłœŧŋŉ㉠㉡㉢㉣㉤㉥㉦㉧㉨㉩㉪㉫㉬㉭㉮㉯㉰㉱㉲㉳㉴㉵㉶㉷㉸㉹㉺㉻㈀㈁㈂㈃㈄㈅㈆㈇㈈㈉㈊㈋㈌㈍㈎㈏㈐㈑㈒㈓㈔㈕㈖㈗㈘㈙㈚㈛ⓐⓑⓒⓓⓔⓕⓖⓗⓘⓙⓚⓛⓜⓝⓞⓟⓠⓡⓢⓣⓤⓥⓦⓧⓨⓩ①②③④⑤⑥⑦⑧⑨⑩⑪⑫⑬⑭⑮⒜⒝⒞⒟⒠⒡⒢⒣⒤⒥⒦⒧⒨⒩⒪⒫⒬⒭⒮⒯⒰⒱⒲⒳⒴⒵⑴⑵⑶⑷⑸⑹⑺⑻⑼⑽⑾⑿⒀⒁⒂"
    for c in chars:
        if c in txt:
            txt = txt.replace(c, "\\u" + str(hex(ord(c)))[2:])
    return txt


def encode_text(txt):
    return re.sub(r'(?i)(?<!\\)(?:\\\\)*\\u([0-9a-f]{4})', lambda m: chr(int(m.group(1), 16)), txt)


def main():
    j2k_engine_object = WinDLL(pathj2k)
    engjk.initialize(j2k_engine_object)

    k2j_engine_object = WinDLL(pathk2j)
    engkj.initialize(k2j_engine_object)

    http_server = WSGIServer(('127.0.0.1', 5000), app, log=None)
    http_server.serve_forever()

    # app.run()


@app.route("/")
def home():
    return "ezTranslator J2K & K2J Web Wrapper"


@app.route("/jktranslate")
def webtranslate():
    src_text = request.args.get('text')
    return encode_text(engjk.translate_j2k(decode_text(src_text)))


@app.route("/kjtranslate")
def subtranslate():
    src_text = request.args.get('text')
    return encode_text(engkj.translate_k2j(decode_text(src_text)))


if __name__ == '__main__':
    main()
