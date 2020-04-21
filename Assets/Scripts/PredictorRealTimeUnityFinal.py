from NeuroPy import NeuroPy
from time import sleep
import csv
import pickle
import numpy as np
import pandas as pd
import zmq
loaded_model = pickle.load(open(r"C:\Users\Nandagopal\Downloads\Unity3D-Python-Communication-master\UnityProject\Assets\NetMQExample\Scripts\final_model.sav", 'rb'))



context = zmq.Context()
socket = context.socket(zmq.REP)
socket.bind("tcp://*:55")

while(1):
    X_test=[]
    counter=0
    neuropy=NeuroPy.NeuroPy("COM4",9600)
    neuropy.start()
    while(counter<=111):
        raw=neuropy.rawValue
        delta=neuropy.delta
        theta=neuropy.theta
        lowalpha=neuropy.lowAlpha
        highalpha=neuropy.highAlpha
        lowbeta=neuropy.lowBeta
        highbeta=neuropy.highBeta
        lowgamma=neuropy.lowGamma
        midgamma=neuropy.midGamma
        X_test.append([raw,delta,theta,lowalpha,highalpha,lowbeta,highbeta,lowgamma,midgamma])
        counter=counter+1
        sleep(0.5)
    X=np.asarray(X_test)
    b = np.reshape(X, (1, 112, 9))
    result = loaded_model.predict(b)
    r=result[0]
    s=r[0]
    s=s*100
    s=round(s,2)
    s=str(s)
    message = socket.recv()
    print("Sent:",s)
    socket.send_string(s)
input()
