import requests
import json
import base64

with open("220050-005-0072M-REV1_BOM2.xlsx", "rb") as f:
    bytes = f.read()

headers = {
    'Content-Type': 'application/json'
}

jsonData = json.dumps({
    "fileContents": base64.b64encode(bytes).decode("utf-8")
})

response = requests.post("http://localhost:5142/api/devion/ets/transformbomexcel", headers=headers, data=jsonData)

print(response.json())