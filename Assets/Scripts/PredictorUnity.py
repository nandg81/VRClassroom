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
  c=0
  while(c<=100):
    X_test=[]
    eeg_data = r"C:\Users\Nandagopal\Downloads\Unity3D-Python-Communication-master\UnityProject\Assets\NetMQExample\Scripts\EEG_data.csv"
    eeg = pd.read_csv(eeg_data)
    eeg=eeg.drop(['Mediation','Attention','predefinedlabel'],axis=1)
    waves = []
    labels = []
    subids = eeg['SubjectID'].unique()
    vidids = eeg['VideoID'].unique()
    waves = []
    labels = []
    for sid in subids:
      for vid in vidids:
        point = eeg.query('SubjectID ==' + str(sid) + ' & VideoID ==' + str(vid))
        labels.append(point['user-definedlabeln'].unique()[0])
        waves.append(point.iloc[:112, 2:11].values)
    waves = np.asarray(waves)
    labels = np.asarray(labels)
    X_test=waves[c]
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
    c=c+1
    sleep(56)
input()
