import requests
import json
import sys
import os
import time
import datetime
import argparse
import pandas as pd
import numpy as np

endpoint = "http://192.168.100.237:5001/api/devion/cebeo/articles"
endpoint2 = "http://192.168.100.237:5001/api/devion/cebeo/updatearticleprice"
start = datetime.datetime.now()

def get_articles():
    r = requests.get(endpoint)
    if r.status_code == 200:
        return r.json()
    else:
        return None
    

def get_price(id):
    r = requests.get(endpoint2+f"?articleReference={id}")
    if r.status_code == 200:
        return r.json()
    else:
        return None


ids = get_articles()

niet_bestaan = 0
niet_verkocht = 0
veranderd = 0

print("Niet bestaand \t Niet verkocht \t Veranderd")
for id in ids:
    price = get_price(id)
    if price == None:
        niet_bestaan += 1
    elif price == 0:
        niet_verkocht += 1
    elif price >= 0:
        veranderd += 1
    
    print(f'\r{niet_bestaan} \t\t {niet_verkocht} \t\t {veranderd}', end='')
end = datetime.datetime.now()
print("Time elapsed: ", end - start)