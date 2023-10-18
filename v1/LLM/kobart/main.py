from multiprocessing import Process, freeze_support
import threading

if __name__ == '__main__':
    freeze_support()

import chatai
chatai.chat()


