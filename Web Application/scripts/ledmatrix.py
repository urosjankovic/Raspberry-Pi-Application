#!/usr/bin/python3
from sense_emu import SenseHat
import json

sense = SenseHat()
file = open("ledmatrix.json", "r")
data = json.loads(file.read())
file.close()

try:
    if data:
        sense.clear()
        for key, value in data.items():
            x = int(key[2])
            y = int(key[3])
            r = int(value[0])
            g = int(value[1])
            b = int(value[2])
            sense.set_pixel( x, y, r, g, b )
    else:
        sense.clear()
except:
    sense.clear()