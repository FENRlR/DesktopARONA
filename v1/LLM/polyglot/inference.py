import requests
import socket
import torch
import transformers
from transformers import AutoTokenizer, AutoModelForCausalLM, BitsAndBytesConfig
from peft import prepare_model_for_kbit_training, LoraConfig, get_peft_model, PeftModel

print(torch.cuda.is_available())

model_id = "EleutherAI/polyglot-ko-1.3b"

bnb_config = BitsAndBytesConfig(
    load_in_8bit = True,
    llm_int8_threshold = 6.0,
    llm_int8_skip_modules = None,
    llm_int8_enable_fp32_cpu_offload = False,
    llm_int8_has_fp16_weight = False

    #load_in_4bit=True,
    #bnb_4bit_use_double_quant=True,
    #bnb_4bit_quant_type="nf4",
    #bnb_4bit_compute_dtype=torch.bfloat16
)

tokenizer = AutoTokenizer.from_pretrained(model_id)
model = AutoModelForCausalLM.from_pretrained(model_id, quantization_config=bnb_config, device_map={"":0})
model.gradient_checkpointing_enable()
model = prepare_model_for_kbit_training(model)

config = LoraConfig(
    r=8,
    lora_alpha=32,
    target_modules=["query_key_value"],
    lora_dropout=0.05,
    bias="none",
    task_type="CAUSAL_LM"
)

model = PeftModel.from_pretrained(model, "./model/checkpoint-500")
tokenizer.pad_token = tokenizer.eos_token
model.config.use_cache = False 
model.eval()


def gen(x):
    gened = model.generate(
        **tokenizer(
            f"### 질문: {x}\n\n### 답변:",
            return_tensors='pt',
            return_token_type_ids=False,
        ).to(0),
        #max_new_tokens=256,
        #max_new_tokens=128,
        #max_new_tokens=16,
        #max_new_tokens=8,
        max_length=36,
        early_stopping=True,
        do_sample= True,
        eos_token_id=2,
        repetition_penalty=1.2,
        max_time = 2.5
    )
    return tokenizer.decode(gened[0]).replace('#','')


def gen2(x):
    fstring = f"### 질문: {x}\n\n### 답변:"
    input_ids = tokenizer(fstring, return_tensors="pt", return_token_type_ids=False)
    input_ids = input_ids.input_ids
    gened = model.generate(
        input_ids.to(0),
        max_new_tokens=256,
        early_stopping=True,
        do_sample=True,
        eos_token_id=2,
        max_time = 0.1
    )
    return tokenizer.decode(gened[0])


def loadchecker():
    gen2("load")
    print("Load complete")
    return 1


comswit = 1
ipcsendt = 1
if comswit == 1:
    loadchecker()
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
                        qtout = gen(q).split("답변: ", 2)[1].replace('<usr> ', '') + "\n"
                        qtls = ""
                        telvar = 2

                        # if q == '!이제 그만 종료할게':
                        #    break
                        # q = ""

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
