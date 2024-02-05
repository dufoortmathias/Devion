import requests

with open("request.xml", "r") as f:
    data = f.read()

headers = {
    "Content-Type": "application/xml"
}

response = requests.post("http://b2b.cebeo.be/webservices/xml", data=data, headers=headers)


with open("response.xml", "w+") as f:
    f.write(response.text)