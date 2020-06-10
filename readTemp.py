#!/usr/bin/python3
from sense_emu import SenseHat

import sys
import getopt
import json
import time

sense = SenseHat()
class DataPoint:
	def __init__(self, data):
		self.data = data

try:
    while True:
        temp = sense.get_temperature()
        dp = DataPoint(temp)
       
        # get json string
        jsonStr = json.dumps(dp.__dict__)
        
        #save to file
        try:
            datafile = open("tempValues.json","w")
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