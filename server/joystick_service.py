#!/usr/bin/python
from sense_emu import SenseHat
from time import sleep
import json
import socket

sense = SenseHat()

counters = {'Counter': 0, 'X': 0, 'Y': 0}

previousJSON = 0

# Create a TCP/IP socket
sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

# Bind the socket to the port 10002
server_address = ('localhost', 10002)
print('starting up on {} port {}'. format(*server_address))
sock.bind(server_address)

# Listen for incoming connections
sock.listen(1)
print('hey')
while True:
    connection, client_address = sock.accept()
    try:
        print('connection from', client_address)

        while True:
            cmd = connection.recv(8)
            if cmd == b'get':
                for event in sense.stick.get_events():
                    if event.action == 'pressed':
                        if event.direction == 'middle':
                            counters['Counter'] += 1
                        elif event.direction == 'up':
                            counters['Y'] += 1
                        elif event.direction == 'down':
                            counters['Y'] -= 1
                        elif event.direction == 'left':
                            counters['X'] -= 1
                        elif event.direction == 'right':
                            counters['X'] += 1

                dataJSON = json.dumps(counters)

                if not previousJSON == dataJSON:
                    previousJSON = dataJSON
                    msg = dataJSON.encode('utf-8') 
                    connection.sendall(msg)
            if cmd == b'rst':
                counters = {'Counter': 0, 'X': 0, 'Y': 0}

                dataJSON = json.dumps(counters)
                
                if not previousJSON == dataJSON:
                    previousJSON = dataJSON
                    msg = dataJSON.encode('utf-8') 
                    connection.sendall(msg)
            else:
                break
    finally:
        # Clean up the connection
        connection.close()
