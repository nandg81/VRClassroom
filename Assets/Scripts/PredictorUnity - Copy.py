from time import sleep
import zmq



context = zmq.Context()
socket = context.socket(zmq.REP)
socket.bind("tcp://*:55")

while(1):
  c=50
  while(c<=100):
    s=str(c)
    message = socket.recv()
    print("Sent:",s)
    socket.send_string(s)
    c=c+1
    sleep(56)
input()
