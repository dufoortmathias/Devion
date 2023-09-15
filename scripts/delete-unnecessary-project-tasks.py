import requests
import pandas as pd

headers = {
  'Content-Type': 'application/json',
  'Authorization': 'Bearer 9H4VZsZ8r-LwF_Oy2VK-aa4YUVlATiRb-wsVSG9OLZtFAGDYDuMbVsNjfTN_ywZtwSW4oA67SHlzlGrXhG_-9J8njrQ9K665qkNI9kYGX1_Sy0BH82dPRBc80X04ZDbwnqz3KK6nIoDwkjl0hejTOKutvVboXzHU0Vy3yohdZMGplhvlC3lRf0pXBGz43a0h2A3YJ1HsiZQJCcMhlmoqAHdKf4qTSIqqybySeR4hcupAg1HwlgOzDPHLuk4mF6LpFmnvW5W-ayLsvKSzuv0JAtyGSW4LBnXzuv2nnbJZEB4SZQdh'
}

# data.csv contains all lines from ETS table J2W_VOPX with all unique combination of "VO_PROJ, VO_SUBPROJ, VO_UUR"
df = pd.read_csv("data.csv", delimiter=';', dtype=str)

projects = [p for p in requests.request("GET", "https://api.timechimp.com/v2/projects", headers=headers).json()]
tasks = requests.request("GET", "https://api.timechimp.com/v1/tasks", headers=headers).json()
project_tasks = [pt for pt in requests.request("GET", "https://api.timechimp.com/v1/projecttasks", headers=headers).json()]


amount_project_tasks = len(project_tasks)
for (index, project_task) in enumerate(project_tasks):
    progress = round(index / amount_project_tasks * 100, 2)
    print(f"{progress}% ", end='')

    project = [project for project in projects if project["id"] == project_task["projectId"]][0]
    task = [task for task in tasks if task["id"] == project_task["taskId"]][0]

    if not ((df["VO_PROJ"] == project["code"][:7]) & (df["VO_SUBPROJ"] == project["code"][7:]) & (df["VO_UUR"] == task["code"])).any():
      if project_task["budgetHours"] and project_task["budgetHours"] > 0:
         print(f"Project task not deleted because someone registered time on it ({project['code']} - {task['code']})")
      else:
        for _ in range(4):
          try:
            response = requests.request("DELETE", f"https://api.timechimp.com/v1/projecttasks/{project_task['id']}", headers=headers)
            if not response.ok:
              # Retry request 
              continue
            print(f"Project task deleted ({project['code']} - {task['code']})")
            break
          except:
            pass
    else:
       print("")