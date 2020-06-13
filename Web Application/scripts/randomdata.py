import random as rnd
import json
import socket

# Create a TCP/IP socket
sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

# Bind the socket to the port
server_address = ('localhost', 10000)
print('starting up on {} port {}'.format(*server_address))
sock.bind(server_address)

# Listen for incoming connections
sock.listen(1)

while True:
    connection, client_address = sock.accept()
    try:
        while True:
            data = [
                {
                    "name": "acceleration",
                    "data": (rnd.random(), rnd.random(), rnd.random()),
                    "unit": "deg"
                },
                {
                    "name": "magnetic",
                    "data": (rnd.random(), rnd.random(), rnd.random())
                },

            ]

            dataJSON = json.dumps(data)

            msg = dataJSON.encode('utf-8') 
            connection.sendall(msg)

    finally:
        # Clean up the connection
        connection.close()
