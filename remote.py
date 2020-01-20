import RPi.GPIO as GPIO
import time
from AlphaBot import AlphaBot
import socket
import thread

Ab = AlphaBot()

DR = 16
DL = 19

GPIO.setmode(GPIO.BCM)
GPIO.setwarnings(False)
GPIO.setup(DR,GPIO.IN,GPIO.PUD_UP)
GPIO.setup(DL,GPIO.IN,GPIO.PUD_UP)

def move_robot(direction):
    if direction == 'Up':
        Ab.forward()            
    elif direction == 'Down':
        Ab.backward()            
    elif direction == 'Left':
        Ab.left()            
    elif direction == 'Right':
        Ab.right()            
    elif direction == 'Stop':
        Ab.stop()        

def listen_udp():
    UDP_IP = "0.0.0.0"
    UDP_PORT = 20001

    sock = socket.socket(family=socket.AF_INET, type=socket.SOCK_DGRAM)
    sock.bind((UDP_IP, UDP_PORT))

    while True:
        print "start UDP listener: ", UDP_PORT
        data, addr = sock.recvfrom(1024) # buffer size is 1024 bytes
        print "received message:", data
        command = data.strip()
        move_robot(command)

Ab.stop()

try:    
    listen_udp()
	
except KeyboardInterrupt:
	GPIO.cleanup();