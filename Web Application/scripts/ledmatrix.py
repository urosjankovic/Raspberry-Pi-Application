#!/usr/bin/python3
from sense_emu import SenseHat
import json
import socket

sense = SenseHat()

# Create a TCP/IP socket
sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

# Bind the socket to the port
server_address = ('localhost', 10001)
print('starting up on {} port {}'. format(*server_address))
sock.bind(server_address)

# Listen for incoming connections
sock.listen(1)

while True:
    connection, client_address = sock.accept()
    try:
        while True:
            msg = connection.recv(4096)
            data = msg.decode()
            if data:
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
                    msg = "Success"
                    msg = msg.encode('utf-8')
                    connection.sendall(msg)
                except:
                    sense.clear()
            
    finally:
        connection.close()