#!/usr/bin/python3
from sense_emu import SenseHat

import sys
import getopt
import json
import time

sense = SenseHat()
class DataPoint:
	def __init__(self, data1):
		self.data1 = data1
        
sense = SenseHat()


try:
    while True:
        press = sense.get_pressure()
        dp = DataPoint(press)
       
        # get json string
        jsonStr = json.dumps(dp.__dict__)
        
        #save to file
        try:
            datafile = open("pressValues.json","w")
            datafile.write(jsonStr)
        except:
            print("Write Error")
        finally:
            datafile.close()
        
        print(jsonStr)
        
        time.sleep(0.1)
        
except KeyboardInterrupt:
    pass
    
sysarg = sys. argv[1:]