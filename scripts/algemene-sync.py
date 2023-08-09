import requests
import json
import os
import datetime

# Init vars
## Start time script
start_time = datetime.datetime.now()
## Base URL API
base_URL = "http://localhost:5142/api/"
## Name of data file that stores information about last time this script 
date_filename = "data.json"
## Defines format of how datetime objects should be tranlated to a String
dateformat = "%d/%m/%Y %H:%M:%S"


# Reads data from the json file, if the file doesn't exist it will create one
global json_data
if os.path.exists(date_filename):
    with open(date_filename, 'r') as json_file:
        json_data = json.loads(json_file.read())
else:
    print("Failed to load json")

    output = "{}"
    json_data = json.loads(output)
    json_data["last_sync"] = "01/01/2000 00:00:00"

    with open(date_filename, 'w+') as json_file:
        json_file.write(json.dumps(json_data, indent=2))

    print("New json created")

# Variable that keeps track of all the information that was logged
# Initiliazing it with a title
output = f"Sync TimeChimp log: van {json_data['last_sync']} tot {start_time.strftime(dateformat)}\n\n"


# Logs a message by printing it in the console and saving it to put it in the log file
def log(message):
    global output
    output = f"{output}{message}\n"
    print(message)

# Receives ids from all employees that have changed in ETS after last sync
def get_employees_to_sync():
    response = requests.get(f"{base_URL}ets/employeeids?dateString={json_data['last_sync']}")
    if response.ok:
        return response.json()

# Receives ids from all customers that have changed in ETS after last sync
def get_customers_to_sync():
    response = requests.get(f"{base_URL}ets/customerids?dateString={json_data['last_sync']}")
    if response.ok:
        return response.json()
   
# Receives ids from all contacts that have changed in ETS after last sync 
def get_contacts_to_sync():
    response = requests.get(f"{base_URL}ets/contactids?dateString={json_data['last_sync']}")
    if response.ok:
        return response.json()
    
# Receives ids from all projects that have changed in ETS after last sync 
def get_projects_to_sync():
    response = requests.get(f"{base_URL}ets/projectids?dateString={json_data['last_sync']}")
    if response.ok:
        return response.json()

# Updates or creates employees with a specific id in TimeChimp with the data from ETS
# Returns list of employees that have been successfully synchronized
def sync_employees(employee_ids):
    synced = []
    for employee_id in employee_ids:
        response = requests.post(f"{base_URL}ets/syncemployee?employeeId={employee_id}")
        if response.ok:
            synced.append(employee_id)
            log(f"Syncronisation employee ({employee_id}) succeeded")
        else:
            log(f"Syncronisation employee ({employee_id}) failed!")
    return synced

# Updates or creates customers with a specific id in TimeChimp with the data from ETS
# Returns list of customers that have been successfully synchronized
def sync_customers(customer_ids):
    synced = []
    for customer_id in customer_ids:
        response = requests.post(f"{base_URL}ets/synccustomer?customerId={customer_id}")
        if response.ok:
            synced.append(customer_id)
            log(f"Syncronisation customer ({customer_id}) succeeded")
        else:
            log(f"Syncronisation customer ({customer_id}) failed!")
    return synced

# Updates or creates contacts with a specific id in TimeChimp with the data from ETS
# Returns list of contacts that have been successfully synchronized
def sync_contacts(contact_ids):
    synced = []
    for contact_id in contact_ids:
        response = requests.post(f"{base_URL}ets/synccontact?contactId={contact_id}")
        if response.ok:
            synced.append(contact_id)
            log(f"Syncronisation contact ({contact_id}) succeeded")
        else:
            log(f"Syncronisation contact ({contact_id}) failed!")
    return synced

# Updates or creates projects with a specific id in TimeChimp with the data from ETS, together with its subprojects
# Returns list of projects that have been successfully synchronized
def sync_projects(project_ids):
    synced = []
    for project_id in project_ids:
        response = requests.post(f"{base_URL}ets/syncproject?projectId={project_id}")
        if response.ok:
            synced.append(project_id)
            log(f"Syncronisation project ({project_id}) succeeded")
        else:
            log(f"Syncronisation project ({project_id}) failed!")
    return synced

# Sync Employees
## Checks if json already contains data from previous failed employee syncs, creates this field with empty list if false
if "failed_employees" not in json_data:
    json_data["failed_employees"] = []
## Retrieves employees to sync and adds employees where previous sync attempt failed
employee_ids = get_employees_to_sync() + json_data["failed_employees"]
## Syncs these employees and receives employees where sync was successful
synced_employee_ids = sync_employees(employee_ids)
## Updates json to contain new failed employee sync attempts
json_data["failed_employees"] = list(set(employee_ids) - set(synced_employee_ids))
## Log total results sync employees
log("")
log("Employees:")
log(f"Total syncronisations succeeded: {len(synced_employee_ids)}")
log(f"Total syncronisations failed: {len(employee_ids)-len(synced_employee_ids)}")
log("")

# Sync uurcodes
# TODO

# def get_uurcodes_data():
#     response = requests.get(endpoint + f"ets/uurcodes")

#     if response.status_code == 200:
#         return response.json()
#     else:
#         return None

# Sync customers
## Checks if json already contains data from previous failed customer syncs, creates this field with empty list if false
if "failed_customers" not in json_data:
    json_data["failed_customers"] = []
## Retrieves customers to sync and adds customers where previous sync attempt failed
customer_ids = get_customers_to_sync() + json_data["failed_customers"]
## Syncs these customers and receives customers where sync was successful
synced_customer_ids = sync_customers(customer_ids)
## Updates json to contain new failed customer sync attempts
json_data["failed_customers"] = list(set(customer_ids) - set(synced_customer_ids))
## Log total results sync customers
log("")
log("Customers:")
log(f"Total syncronisations succeeded: {len(synced_customer_ids)}")
log(f"Total syncronisations failed: {len(customer_ids)-len(synced_customer_ids)}")
log("")

# Sync contacts
## Checks if json already contains data from previous failed contact syncs, creates this field with empty list if false
if "failed_contacts" not in json_data:
    json_data["failed_contacts"] = []
## Retrieves contacts to sync and adds contacts where previous sync attempt failed
contact_ids = get_contacts_to_sync() + json_data["failed_contacts"]
## Syncs these contacts and receives contacts where sync was successful
synced_contact_ids = sync_contacts(contact_ids)
## Updates json to contain new failed contact sync attempts
json_data["failed_contacts"] = list(set(contact_ids) - set(synced_contact_ids))
## Log total results sync contacts
log("")
log("Contacts:")
log(f"Total syncronisations succeeded: {len(synced_contact_ids)}")
log(f"Total syncronisations failed: {len(contact_ids)-len(synced_contact_ids)}")
log("")

# Sync projects
## Checks if json already contains data from previous failed project syncs, creates this field with empty list if false
if "failed_projects" not in json_data:
    json_data["failed_projects"] = []
## Retrieves projects to sync and adds contacts where previous sync attempt failed
project_ids = get_projects_to_sync() + json_data["failed_projects"]
## Syncs these projects and receives projects where sync was successful
synced_project_ids = sync_contacts(project_ids)
## Updates json to contain new failed project sync attempts
json_data["failed_projects"] = list(set(project_ids) - set(synced_project_ids))
## Log total results sync projects
log("")
log("Projects:")
log(f"Total syncronisations succeeded: {len(synced_contact_ids)}")
log(f"Total syncronisations failed: {len(contact_ids)-len(synced_contact_ids)}")
log("")

# Sync times
# TODO

# def get_uren_data():
#     response = requests.get(endpoint + f"ets/times")

#     if response.status_code == 200:
#         return response.json()
#     else:
#         return None

# # json_uren = get_uren_data()

# # if json_uren is not None:
# #     response = requests.post(endpoint + f"ets/times")

# Sync mileages
# TODO

# def get_mileages_data():
#     response = requests.get(endpoint + f"timechimp/mileages")

#     if response.status_code == 200:
#         return response.json()
#     else:
#         return None

# # json_mileages = get_mileages_data()

# # if json_mileages is not None:
# #     response = requests.get(endpoint + f"ets/mileage")

# Determine runtime script
end_time = datetime.datetime.now()
duration = end_time - start_time
log(f"Duration: {duration}")

# Create log file
with open(f"log-{start_time.strftime('%d%m%y-%H%M%S')}.txt", 'w') as log_file:
    log_file.write(output)

# Update the json data file
with open("data.json", 'w+') as json_file:
    # Update last sync field in json to date script started
    json_data["last_sync"] = start_time.strftime(dateformat)

    # Write data to the json file
    json_file.write(json.dumps(json_data, indent=2))