#!/usr/bin/python3
from sense_emu import SenseHat

import sys
import getopt
import json
import time

sense = SenseHat()
# class DataPoint:
	# def __init__(self, data):
		# self.data = data

try:
    while True:
        angles = sense.get_orientation()
        # dp = DataPoint(angles)
       
        
        data = json.dumps(angles)
        
        #save to file
        try:
            datafile = open("angleValues.json","w")
            datafile.write(data)
        except:
            print("Write Error")
        finally:
            datafile.close()
        
        print(data)
        
        time.sleep(0.1)
        
except KeyboardInterrupt:
    pass
    
sysarg = sys. argv[1:]