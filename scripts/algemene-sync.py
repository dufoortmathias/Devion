import requests
import json
from datetime import date, datetime

endpoint = "http://localhost:5001/api/"
test_date = date.today().strftime("%Y-%m-%d")
test_date = "2000-01-01"

startTime = datetime.now()


# Get Data's from api
def get_customer_data():
    response = requests.get(endpoint + f"ets/customerids?dateString={test_date}")

    if response.status_code == 200:
        return response.json()
    else:
        return None


def get_contact_data():
    response = requests.get(endpoint + f"ets/contactids?dateString={test_date}")

    if response.status_code == 200:
        return response.json()
    else:
        return None


def get_employee_data():
    response = requests.get(endpoint + f"ets/employeeids?dateString={test_date}")

    if response.status_code == 200:
        return response.json()
    else:
        return None


def get_mileages_data():
    response = requests.get(endpoint + f"timechimp/mileages")

    if response.status_code == 200:
        return response.json()
    else:
        return None


def get_project_data():
    response = requests.get(endpoint + f"ets/projectids?dateString={test_date}")

    if response.status_code == 200:
        return response.json()
    else:
        return None


def get_uren_data():
    response = requests.get(endpoint + f"ets/times")

    if response.status_code == 200:
        return response.json()
    else:
        return None


def get_uurcodes_data():
    response = requests.get(endpoint + f"ets/uurcodes")

    if response.status_code == 200:
        return response.json()
    else:
        return None


# post data to update

json_uurcodes = get_uurcodes_data()

if json_uurcodes is not None:
    response = requests.post(endpoint + f"timechimp/updateUurcodes")


# json_customer = get_customer_data()

# if json_customer is not None:
#     for item in json_customer:
#         response = requests.post(endpoint + f"ets/updateCustomer?customerId={item}")

# json_contact = get_contact_data()

# if json_contact is not None:
#     for item in json_contact:
#         response = requests.post(endpoint + f"ets/updateContact?contactId={item}")


# json_employee = get_employee_data()

# if json_employee is not None:
#     for item in json_employee:
#         response = requests.post(endpoint + f"ets/updateEmployee?employeeId={item}")

json_project = get_project_data()
i = 0

if json_project is not None:
    lengte = len(json_project)
    for item in json_project:
        response = requests.post(endpoint + f"ets/updateProject?projectId={item}")
        print(f"{i}/{lengte}")
        i += 1

# json_uren = get_uren_data()

# if json_uren is not None:
#     response = requests.post(endpoint + f"ets/times")

# json_mileages = get_mileages_data()

# if json_mileages is not None:
#     response = requests.get(endpoint + f"ets/mileage")


endTime = datetime.now()

duration = endTime - startTime

print("duration: " + str(duration))

print("Done")
