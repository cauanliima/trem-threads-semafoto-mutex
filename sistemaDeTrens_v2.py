import threading
from tkinter import *
import time

#criação / inicialização dos mutex
mutexYB = threading.Lock()
mutexYR = threading.Lock()
mutexGB = threading.Lock()
mutexGR = threading.Lock()

#criação / inicalização das semaforos
sem = threading.Semaphore(3) #primeiro conjunto de trilhos (L2, L7 e 13)

def visualizarPosicaoDosTrens(window):
    window.geometry("400x400")    

    canvas.create_rectangle(20, 20, 190, 120, outline="yellow", width="10")
    canvas.create_rectangle(210, 20, 380, 120, outline="blue", width="10")
    canvas.create_rectangle(20, 140, 190, 240, outline="red", width="10")
    canvas.create_rectangle(210, 140, 380, 240, outline="green", width="10")


def painelDeControle(window):
    global amarelo
    global vermelho
    global azul
    global verde

    botaoAumentarVelocTremAmarelo = Button(window, text="+ 0.1", bg='yellow', command=lambda: aumentarVelocidade(amarelo))
    botaoDiminuirVelocTremAmarelo = Button(window, text=" - 0.1 ", bg='yellow', command= lambda: diminuirVelocidade(amarelo))
    botaoAumentarVelocTremAmarelo.place(x = 20, y = 300)
    botaoDiminuirVelocTremAmarelo.place(x = 20, y = 350)

    botaoAumentarVelocTremazul= Button(window, text="+ 0.1", bg='blue', command=lambda: aumentarVelocidade(azul))
    botaoDiminuirVelocTremazul = Button(window, text=" - 0.1 ", bg='blue', command= lambda: diminuirVelocidade(azul))
    botaoAumentarVelocTremazul.place(x = 120, y = 300)
    botaoDiminuirVelocTremazul.place(x = 120, y = 350)

    botaoAumentarVelocTremvermelho= Button(window, text="+ 0.1", bg='red', command= lambda: aumentarVelocidade(vermelho))
    botaoDiminuirVelocTremvermelho = Button(window, text=" - 0.1 ", bg='red', command=lambda: diminuirVelocidade(vermelho))
    botaoAumentarVelocTremvermelho.place(x = 220, y = 300)
    botaoDiminuirVelocTremvermelho.place(x = 220, y = 350)

    botaoAumentarVelocTremverde= Button(window, text="+ 0.1", bg='green', command=lambda: aumentarVelocidade(verde))
    botaoDiminuirVelocTremverde = Button(window, text=" - 0.1 ", bg='green', command=lambda: diminuirVelocidade(verde))
    botaoAumentarVelocTremverde.place(x = 320, y = 300)
    botaoDiminuirVelocTremverde.place(x = 320, y = 350)

def aumentarVelocidade(trem):
    global velocidadeTremamarelo
    global velocidadeTremazul
    global velocidadeTremvermelho
    global velocidadeTremverde
    global amarelo
    global vermelho
    global azul
    global verde

    if(trem == amarelo):
        velocidadeTremamarelo = round(velocidadeTremamarelo - 0.1, 1) if velocidadeTremamarelo > 0.1 else 0.04
    elif (trem == vermelho):   
        velocidadeTremvermelho = round(velocidadeTremvermelho - 0.1, 1) if velocidadeTremvermelho > 0.1 else 0.04
    elif (trem == azul):
            velocidadeTremazul = round(velocidadeTremazul - 0.1, 1) if velocidadeTremazul > 0.1 else 0.04
    elif (trem == verde):
            velocidadeTremverde = round(velocidadeTremverde - 0.1, 1) if velocidadeTremverde > 0.1 else 0.04
    else:
        print("cor inválida!")


def diminuirVelocidade(trem):
    global velocidadeTremamarelo
    global velocidadeTremazul
    global velocidadeTremvermelho
    global velocidadeTremverde
    global amarelo
    global vermelho
    global azul
    global verde

    if(trem == amarelo):
        velocidadeTremamarelo = round(velocidadeTremamarelo + 0.1, 1) if velocidadeTremamarelo < 2 else 2
    elif (trem == vermelho):
        velocidadeTremvermelho = round(velocidadeTremvermelho + 0.1, 1) if velocidadeTremvermelho < 2 else 2
    elif (trem == azul):
        velocidadeTremazul = round(velocidadeTremazul + 0.1, 1) if velocidadeTremazul < 2 else 2
    elif (trem == verde):
        velocidadeTremverde = round(velocidadeTremverde + 0.1, 1) if velocidadeTremverde < 2 else 2
    else:
        print("cor inválida!")


def trilhoamarelo(window, canvas):
    l1 = 0
    
    global velocidadeTremamarelo
    
    tremamarelo = canvas.create_rectangle(10, 10, 30, 30, fill="white")

    while(1):
        if(l1<17):
            canvas.move(tremamarelo, 10, 0)   
            l1 = l1+1
        elif(l1>=17 and l1<27):
            if(l1==17):
                sem.acquire()
                mutexYB.acquire()
            canvas.move(tremamarelo,0, 10)   
            l1 = l1+1
        elif(l1>=27 and l1<44):
            if(l1==27):
                mutexYR.acquire()
                mutexYB.release()
            canvas.move(tremamarelo,-10, 0)   
            l1 = l1+1
        elif(l1>=44 and l1<54):
            if(l1==44):
                mutexYR.release()
                sem.release()
            canvas.move(tremamarelo,0, -10)   
            l1 = l1+1
        else:
            l1=0
        
        time.sleep(velocidadeTremamarelo)

def trilhoazul(window, canvas):
    l2=0
    global velocidadeTremazul

    tremazul = canvas.create_rectangle(200, 10, 220 ,30, fill="white")
    while(1):
        if(l2<17):
            canvas.move(tremazul, 10, 0)   
            l2 = l2+1
        elif(l2>=17 and l2<27):
            canvas.move(tremazul,0, 10)   
            l2 = l2+1
        elif(l2>=27 and l2<44):
            if(l2==27):
                sem.acquire()
                mutexGB.acquire()
            canvas.move(tremazul,-10, 0)   
            l2 = l2+1
        elif(l2>=44 and l2<54):
            if(l2==44):
                mutexYB.acquire()
                mutexGB.release()
            canvas.move(tremazul,0, -10)   
            l2 = l2+1
        else:
            mutexYB.release()
            sem.release()
            l2=0
        time.sleep(velocidadeTremazul)
       
def trilhovermelho(window, canvas):
    l3=0
    tremvermelho = canvas.create_rectangle(10, 130, 30, 150, fill="white")

    sem.acquire()
    mutexYR.acquire()
    while(1):
        if(l3<17):
            canvas.move(tremvermelho, 10, 0)   
            l3 = l3+1
        elif(l3>=17 and l3<27):
            if(l3==17):
                mutexGR.acquire()
                mutexYR.release()
            canvas.move(tremvermelho,0, 10)   
            l3 = l3+1
        elif(l3>=27 and l3<44):
            if(l3==27):
                mutexGR.release()
                sem.release()           
            canvas.move(tremvermelho,-10, 0)   
            l3 = l3+1
        elif(l3>=44 and l3<54):
            canvas.move(tremvermelho,0, -10)   
            l3 = l3+1
        else:
            sem.acquire()
            mutexYR.acquire()
            l3=0
        time.sleep(velocidadeTremvermelho)
    

def trilhoverde(window, canvas):
    l4=27

    global velocidadeTremverde

    tremverde = canvas.create_rectangle(370, 230, 390, 250, fill="white")
    
    while(1):
        if(l4<17):
            canvas.move(tremverde, 10, 0)   
            l4 = l4+1
        elif(l4>=17 and l4<27):
            if(l4==17):
                mutexGB.release()
                sem.release()
            canvas.move(tremverde,0, 10)   
            l4 = l4+1
        elif(l4>=27 and l4<44):
            canvas.move(tremverde,-10, 0)   
            l4 = l4+1
        elif(l4>=44 and l4<54):
            if(l4==44):
                sem.acquire()
                mutexGR.acquire()
            canvas.move(tremverde,0, -10)   
            l4 = l4+1
        else:
            mutexGB.acquire()
            mutexGR.release()
            l4=0    
        time.sleep(velocidadeTremverde)
   
   
# variaveis que identificam qual o trem atraves de um valor int
amarelo = 1 
azul = 2
vermelho = 3
verde = 4

#variaveis que representam os trilhos laterais de cada trem
l1 = 0
l2 = 0
l3 = 0
l4 = 0
l5 = 0
l6 = 0
l7 = 0
l8 = 0
l9 = 0
l10 =0
l11 =0
l12 =0
l13 =0
l14 =0
l15 =0
l16 =0
l17 =0
l18 =0

#variaveis que controlam a velocidade dos trens
velocidadeTremamarelo = 1
velocidadeTremazul = 1
velocidadeTremvermelho = 1
velocidadeTremverde = 1


window = Tk()
canvas = Canvas(window, width=400, height=400)
canvas.pack()

#criação das threads
t1 = threading.Thread(target=visualizarPosicaoDosTrens, args=[window] )
t2 = threading.Thread(target=trilhoamarelo, args=[window, canvas] )
t3 = threading.Thread(target=trilhoazul, args=[window, canvas] )
t4 = threading.Thread(target=trilhovermelho, args=[window, canvas] )
t5 = threading.Thread(target=trilhoverde, args=[window, canvas] )
t6 = threading.Thread(target=painelDeControle, args=[window] )

# inicia as threads
t1.start()
t2.start()
t3.start()
t4.start()
t5.start()
t6.start()

window.mainloop()

t1.join()
t2.join()
t3.join()
t4.join()
t5.join()
t6.join()