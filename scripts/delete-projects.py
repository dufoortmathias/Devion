import requests

headers = {
  'Content-Type': 'application/json',
  'Authorization': 'Bearer 9H4VZsZ8r-LwF_Oy2VK-aa4YUVlATiRb-wsVSG9OLZtFAGDYDuMbVsNjfTN_ywZtwSW4oA67SHlzlGrXhG_-9J8njrQ9K665qkNI9kYGX1_Sy0BH82dPRBc80X04ZDbwnqz3KK6nIoDwkjl0hejTOKutvVboXzHU0Vy3yohdZMGplhvlC3lRf0pXBGz43a0h2A3YJ1HsiZQJCcMhlmoqAHdKf4qTSIqqybySeR4hcupAg1HwlgOzDPHLuk4mF6LpFmnvW5W-ayLsvKSzuv0JAtyGSW4LBnXzuv2nnbJZEB4SZQdh',
  'Cookie': 'ARRAffinity=1ebd96f80492c1bdfec9d1149d39d5045505e105b7fdd3a5d9c6f4870b184d18; ARRAffinitySameSite=1ebd96f80492c1bdfec9d1149d39d5045505e105b7fdd3a5d9c6f4870b184d18'
}

response = requests.request("GET", "https://api.timechimp.com/v2/projects", headers=headers)

for project in response.json():
    if project["code"] and project["code"][-4] == "2":
        response = requests.request("DELETE", f"https://api.timechimp.com/v1/projects?id={project['id']}", headers=headers)
        if response.ok:
            print("Project deleted:", project["code"])
        else:
            print("Project not deleted:", project["code"])