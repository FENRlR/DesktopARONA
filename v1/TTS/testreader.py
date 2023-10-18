import math
import os
import numpy as np


def spcalc(target, x, y):
    temp = 0
    for i in range(x, y):
        temp += target[i]
    return temp


lipset = ['20', '31', '31', '20', '02']
comp = [
    [0.03926216583841655, 1.386339420932902, 1.4996536664161828],
    [0.0843672421182286, 1.895435645363557, 4.351767877807027],
    [0.1400492705745224, 1.969161543996581, 6.14922981262986],
    [0.063082679399056, 2.7846027790550476, 0.9803437815185027],
    [0.10214268839349291, 0.9633353297594283, 1.3874174274739213]
]


def spectralm2(target):
    partzero = spcalc(target, 0, 24)
    partone = spcalc(target, 24, 33)
    parttwo = spcalc(target, 33, 65)
    partthree = spcalc(target, 93, 186)
    
    partone = partone /partzero
    parttwo = parttwo /partzero
    partthree = partthree /partzero
    op = [partone, parttwo, partthree]

    sqpos = []
    for i in range(len(comp)):
        sqrtsum = 0
        for j in range(3):
            sqrtsum += (comp[i][j] - op[j]) ** 2
        sqpos.append(math.sqrt(sqrtsum))

    if partone == 0:
        return "01"
    else:
        return lipset[sqpos.index(min(sqpos))]
        
