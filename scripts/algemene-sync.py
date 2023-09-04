# python -m venv .venv
# .\.venv\Scripts\activate.bat
# pip install -r requirements.txt

import sys
import json
import os
import datetime
import smtplib
import ssl
import calendar

import requests

from email import encoders
from email.mime.multipart import MIMEMultipart
from email.mime.text import MIMEText
from email.mime.base import MIMEBase

from dotenv import dotenv_values

current_dir = os.path.dirname(__file__)
env_file = os.path.join(current_dir, ".env")
config = dotenv_values(dotenv_path=env_file)

# Create a secure SSL context
context = ssl.create_default_context()

if __name__ == "__main__":
    try:
        company = sys.argv[1].lower()
    except:
        company = ""

# Init vars
## Start time script
start_time = datetime.datetime.now()
## Base URL API
base_URL = f"{config.get('BASE_URL_API')}/{company}/"
## Name of data file that stores information about last time this script 
data_filename = f"data_{company}.json"
## Defines format of how datetime objects should be tranlated to a String
dateformat = config.get('DATEFORMAT')
## Name of the team in ETS to sync employees in TimeChimp
team_name_ETS_sync_TimeChimp = config.get("PLOEG_NAAM_ETS_USERS_SYNC_TIMECHIMP")
## Amout weeks back to sync time and mileages
amount_weeks = int(config.get("AMOUNT_WEEKS_BACK_SYNC"))
sender_email = config.get("SENDER_EMAIL")
password = config.get("SENDER_PASSWORD")
notify_emails = config.get("LIST_RECEIVER_EMAILS").split(", ")
send_mail = False
first_sync_time = config.get("FIRST_SYNC_TIME")
log_dir = config.get("PATH_LOG_DIRECTORY")

# Reads data from the json file, if the file doesn't exist it will create one
global json_data

current_dir = os.path.dirname(__file__)
data_file = os.path.join(current_dir, data_filename)
if os.path.exists(data_file):
    with open(data_file, "r") as json_file:
        try:
            json_data = json.loads(json_file.read())
        except:
            json_data = json.loads("{}")
    if "last_sync" not in json_data:
        json_data["last_sync"] = first_sync_time
else:
    print("Failed to load json")

    output = "{}"
    json_data = json.loads(output)
    json_data["last_sync"] = first_sync_time

    with open(data_file, "w+") as json_file:
        json_file.write(json.dumps(json_data, indent=2))

    print("New json created")

# Variable that keeps track of all the information that was logged
# Initiliazing it with a title
output = f"Sync TimeChimp log: van {json_data['last_sync']} tot {start_time.strftime(dateformat)}\n\n"


# Logs a message by printing it in the console and saving it to put it in the log file
def log(message):
    global output
    output = f"{output}{message}"
    print(message, end="", flush=True)

# Receives ids from all employees that have changed in ETS after last sync
def get_employees_to_sync():
    response = requests.get(
        f"{base_URL}ets/employeeids?dateString={json_data['last_sync']}&teamName={team_name_ETS_sync_TimeChimp}"
    )
    if response.ok:
        return response.json()
    else:
        raise Exception(response.content)


# Receives ids from all uurcodes that have changed in ETS after last sync
def get_uurcodes_to_sync():
    response = requests.get(
        f"{base_URL}ets/uurcodeids?dateString={json_data['last_sync']}"
    )
    if response.ok:
        return response.json()
    else:
        raise Exception(response.content)


# Receives ids from all customers that have changed in ETS after last sync
def get_customers_to_sync():
    response = requests.get(
        f"{base_URL}ets/customerids?dateString={json_data['last_sync']}"
    )
    if response.ok:
        return response.json()
    else:
        raise Exception(response.content)


# Receives ids from all contacts that have changed in ETS after last sync
def get_contacts_to_sync():
    response = requests.get(
        f"{base_URL}ets/contactids?dateString={json_data['last_sync']}"
    )
    if response.ok:
        return response.json()
    else:
        raise Exception(response.content)


# Receives ids from all projects that have changed in ETS after last sync
def get_projects_to_sync():
    response = requests.get(
        f"{base_URL}ets/projectids?dateString={json_data['last_sync']}"
    )
    if response.ok:
        return response.json()
    else:
        raise Exception(response.content)


# Receives ids from all times that have changed in ETS after last sync
def get_times_to_sync():
    response = requests.get(
        f"{base_URL}ets/timeids?dateString={(min(start_time, datetime.datetime.strptime(json_data['last_sync'], dateformat)).date() - datetime.timedelta(7*amount_weeks)).strftime(dateformat)}"
    )
    if response.ok:
        return response.json()
    else:
        raise Exception(response.content)


# Receives ids from all mileages that have changed in ETS after last sync
def get_mileages_to_sync():
    response = requests.get(
        f"{base_URL}ets/mileageids?dateString={(min(start_time, datetime.datetime.strptime(json_data['last_sync'], dateformat)).date() - datetime.timedelta(7*amount_weeks)).strftime(dateformat)}"
    )
    if response.ok:
        return response.json()
    else:
        raise Exception(response.content)


# Updates or creates employees with a specific id in TimeChimp with the data from ETS
# Returns list of employees that have been successfully synchronized
def sync_employees(employee_ids):
    synced = []
    for index, employee_id in enumerate(employee_ids):
        log(f"{int(index/len(employee_ids)*100)}% ")
        log(f"{employee_id} ")
        response = requests.post(
            f"{base_URL}ets/syncemployee?employeeId={employee_id}"
        )
        if response.ok:
            synced.append(employee_id)
            log("succeeded\n")
        else:
            log(
                f"failed! ({response.status_code}{': ' + json.loads(response.text)['detail'] if response.text else ''})\n"
            )
    return synced


# Updates or creates uurcodes with a specific id in TimeChimp with the data from ETS
# Returns list of uurcodes that have been successfully synchronized
def sync_uurcodes(uurcode_ids):
    synced = []
    for index, uurcode_id in enumerate(uurcode_ids):
        log(f"{int(index/len(uurcode_ids)*100)}% ")
        log(f"{uurcode_id} ")
        response = requests.post(f"{base_URL}ets/syncuurcode?uurcodeId={uurcode_id}")
        if response.ok:
            synced.append(uurcode_id)
            log("succeeded\n")
        else:
            log(
                f"failed! ({response.status_code}{': ' + json.loads(response.text)['detail'] if response.text else ''})\n"
            )
    return synced


# Updates or creates customers with a specific id in TimeChimp with the data from ETS
# Returns list of customers that have been successfully synchronized
def sync_customers(customer_ids):
    synced = []
    for index, customer_id in enumerate(customer_ids):
        log(f"{int(index/len(customer_ids)*100)}% ")
        log(f"{customer_id} ")
        response = requests.post(
            f"{base_URL}ets/synccustomer?customerId={customer_id}"
        )
        if response.ok:
            synced.append(customer_id)
            log("succeeded\n")
        else:
            log(
                f"failed! ({response.status_code}{': ' + json.loads(response.text)['detail'] if response.text else ''})\n"
            )
    return synced


# Updates or creates contacts with a specific id in TimeChimp with the data from ETS
# Returns list of contacts that have been successfully synchronized
def sync_contacts(contact_ids):
    synced = []
    for index, contact_id in enumerate(contact_ids):
        log(f"{int(index/len(contact_ids)*100)}% ")
        log(f"{contact_id} ")
        response = requests.post(f"{base_URL}ets/synccontact?contactId={contact_id}")
        if response.ok:
            synced.append(contact_id)
            log("succeeded\n")
        else:
            log(
                f"failed! ({response.status_code}{': ' + json.loads(response.text)['detail'] if response.text else ''})\n"
            )
    return synced


# Updates or creates projects with a specific id in TimeChimp with the data from ETS, together with its subprojects
# Returns list of projects that have been successfully synchronized
def sync_projects(project_ids):
    synced = []
    for index, project_id in enumerate(project_ids):
        log(f"{int(index/len(project_ids)*100)}% ")
        log(f"{project_id} ")
        response = requests.post(f"{base_URL}ets/syncproject?projectId={project_id}")
        if response.ok:
            synced.append(project_id)
            log("succeeded\n")
        else:
            log(
                f"failed! ({response.status_code}{': ' + json.loads(response.text)['detail'] if response.text else ''})\n"
            )
    return synced


# Updates or creates times with a specific id in TimeChimp with the data from ETS
# Returns list of times that have been successfully synchronized
def sync_times(time_ids):
    synced = []
    for index, time_id in enumerate(time_ids):
        log(f"{int(index/len(time_ids)*100)}% ")
        log(f"{time_id} ")
        response = requests.post(f"{base_URL}ets/synctime?timeId={time_id}")
        if response.ok:
            synced.append(time_id)
            log("succeeded\n")
        else:
            log(
                f"failed! ({response.status_code}{': ' + json.loads(response.text)['detail'] if response.text else ''})\n"
            )
    return synced


# Updates or creates mileages with a specific id in mileageChimp with the data from ETS
# Returns list of mileages that have been successfully synchronized
def sync_mileages(mileage_ids):
    synced = []
    for index, mileage_id in enumerate(mileage_ids):
        log(f"{int(index/len(mileage_ids)*100)}% ")
        log(f"{mileage_id} ")
        response = requests.post(f"{base_URL}ets/syncmileage?mileageId={mileage_id}")
        if response.ok:
            synced.append(mileage_id)
            log("succeeded\n")
        else:
            log(
                f"failed! ({response.status_code}{': ' + json.loads(response.text)['detail'] if response.text else ''})\n"
            )
    return synced


try:
    if len(company) == 0: raise Exception("Script was run without an argument for the company name")

    log("Sync employees:\n")
    # Checks if json already contains data from previous failed employee syncs, creates this field with empty list if false
    if "failed_employees" not in json_data:
        json_data["failed_employees"] = []
    # Retrieves employees to sync and adds employees where previous sync attempt failed
    employee_ids = list(set(get_employees_to_sync() + json_data["failed_employees"]))
    # Syncs these employees and receives employees where sync was successful
    synced_employee_ids = sync_employees(employee_ids)
    # Updates json to contain new failed employee sync attempts
    json_data["failed_employees"] = list(set(employee_ids) - set(synced_employee_ids))
    # Log total results sync employees
    if len(employee_ids) == 0:
        log(f"Nothing to syncronize")
    else:
        log("\n")
        log(f"Total amount Synchronize succeeded: {len(synced_employee_ids)}\n")
        log(
            f"Total amount Synchronize failed: {len(employee_ids)-len(synced_employee_ids)}"
        )
    log("\n\n")

    log("Sync uurcodes:\n")
    # Checks if json already contains data from previous failed uurcode syncs, creates this field with empty list if false
    if "failed_uurcodes" not in json_data:
        json_data["failed_uurcodes"] = []
    # Retrieves uurcodes to sync and adds uurcodes where previous sync attempt failed
    uurcode_ids = list(set(get_uurcodes_to_sync() + json_data["failed_uurcodes"]))
    # Syncs these uurcodes and receives uurcodes where sync was successful
    synced_uurcode_ids = sync_uurcodes(uurcode_ids)
    # Updates json to contain new failed uurcode sync attempts
    json_data["failed_uurcodes"] = list(set(uurcode_ids) - set(synced_uurcode_ids))
    # Log total results sync uurcodes
    if len(uurcode_ids) == 0:
        log(f"Nothing to syncronize")
    else:
        log("\n")
        log(f"Total amount Synchronize succeeded: {len(synced_uurcode_ids)}\n")
        log(
            f"Total amount Synchronize failed: {len(uurcode_ids)-len(synced_uurcode_ids)}"
        )
    log("\n\n")

    log("Sync customers:\n")
    # Checks if json already contains data from previous failed customer syncs, creates this field with empty list if false
    if "failed_customers" not in json_data:
        json_data["failed_customers"] = []
    # Retrieves customers to sync and adds customers where previous sync attempt failed
    customer_ids = list(set(get_customers_to_sync() + json_data["failed_customers"]))
    # Syncs these customers and receives customers where sync was successful
    synced_customer_ids = sync_customers(customer_ids)
    # Updates json to contain new failed customer sync attempts
    json_data["failed_customers"] = list(set(customer_ids) - set(synced_customer_ids))
    # Log total results sync customers
    if len(customer_ids) == 0:
        log(f"Nothing to syncronize")
    else:
        log("\n")
        log(f"Total amount Synchronize succeeded: {len(synced_customer_ids)}\n")
        log(
            f"Total amount Synchronize failed: {len(customer_ids)-len(synced_customer_ids)}"
        )
    log("\n\n")

    log("Sync contacts:\n")
    # Checks if json already contains data from previous failed contact syncs, creates this field with empty list if false
    if "failed_contacts" not in json_data:
        json_data["failed_contacts"] = []
    # Retrieves contacts to sync and adds contacts where previous sync attempt failed
    contact_ids = list(set(get_contacts_to_sync() + json_data["failed_contacts"]))
    # Syncs these contacts and receives contacts where sync was successful
    synced_contact_ids = sync_contacts(contact_ids)
    # Updates json to contain new failed contact sync attempts
    json_data["failed_contacts"] = list(set(contact_ids) - set(synced_contact_ids))
    # Log total results sync contacts
    if len(contact_ids) == 0:
        log(f"Nothing to syncronize")
    else:
        log("\n")
        log(f"Total amount Synchronize succeeded: {len(synced_contact_ids)}\n")
        log(
            f"Total amount Synchronize failed: {len(contact_ids)-len(synced_contact_ids)}"
        )
    log("\n\n")

    log("Sync projects:\n")
    # Checks if json already contains data from previous failed project syncs, creates this field with empty list if false
    if "failed_projects" not in json_data:
        json_data["failed_projects"] = []
    # Retrieves projects to sync and adds projects where previous sync attempt failed
    project_ids = list(set(get_projects_to_sync() + json_data["failed_projects"]))
    # Syncs these projects and receives projects where sync was successful
    synced_project_ids = sync_projects(project_ids)
    # Updates json to contain new failed project sync attempts
    json_data["failed_projects"] = list(set(project_ids) - set(synced_project_ids))
    # Log total results sync projects
    if len(project_ids) == 0:
        log(f"Nothing to syncronize")
    else:
        log("\n")
        log(f"Total amount Synchronize succeeded: {len(synced_project_ids)}\n")
        log(
            f"Total amount Synchronize failed: {len(project_ids)-len(synced_project_ids)}"
        )
    log("\n\n")

    log("Sync times:\n")
    # Checks if json already contains data from previous failed time syncs, creates this field with empty list if false
    if "failed_times" not in json_data:
        json_data["failed_times"] = []
    # Retrieves times to sync and adds times where previous sync attempt failed
    time_ids = list(set(get_times_to_sync() + json_data["failed_times"]))
    # Syncs these times and receives times where sync was successful
    synced_time_ids = sync_times(time_ids)
    # Log total results sync times
    if len(time_ids) == 0:
        log(f"Nothing to syncronize")
    else:
        log("\n")
        log(f"Total amount Synchronize succeeded: {len(synced_time_ids)}\n")
        log(f"Total amount Synchronize failed: {len(time_ids)-len(synced_time_ids)}")
    log("\n\n")

    log("Sync mileages:\n")
    # Checks if json already contains data from previous failed mileage syncs, creates this field with empty list if false
    if "failed_mileages" not in json_data:
        json_data["failed_mileages"] = []
    # Retrieves mileages to sync and adds mileages where previous sync attempt failed
    mileage_ids = list(set(get_mileages_to_sync() + json_data["failed_mileages"]))
    # Syncs these mileages and receives mileages where sync was successful
    synced_mileage_ids = sync_mileages(mileage_ids)
    # Log total results sync mileages
    if len(mileage_ids) == 0:
        log(f"Nothing to syncronize")
    else:
        log("\n")
        log(f"Total amount Synchronize succeeded: {len(synced_mileage_ids)}\n")
        log(
            f"Total amount Synchronize failed: {len(mileage_ids)-len(synced_mileage_ids)}"
        )
    log("\n\n")

    # Update the json data file
    with open(data_file, "w+") as json_file:
        # Update last sync field in json to date script started
        json_data["last_sync"] = start_time.strftime(dateformat)

        # Write data to the json file
        json_file.write(json.dumps(json_data, indent=2))
except Exception as e:
    send_mail = True

    log(e)
    log("\n\n")

# Determine runtime script
end_time = datetime.datetime.now()
duration = end_time - start_time
minutes = int(duration.total_seconds() / 60)
seconds = duration.total_seconds() % 60
log(f"Duration: {minutes} min {seconds} sec")
dir = os.path.dirname(__file__)
log_filename = f"{log_dir}/logs/{company}/{start_time.year}/{calendar.month_name[start_time.month]}/{start_time.day}/log-{start_time.strftime('%d%m%y-%H%M%S')}.txt"
# Create log file
os.makedirs(os.path.dirname(log_filename), exist_ok=True)
with open(log_filename, "w+") as log_file:
    log_file.write(output)

# notify people if something went wrong
if send_mail:
    message = MIMEMultipart()

    message["From"] = sender_email
    message["Subject"] = f"Script sync TimeChimp {company} failed"

    body = f"""
Er ging iets mis met het TimeChimp script!
Bekijk bijlage voor meer informatie.
"""

    # Add body to email
    message.attach(MIMEText(body, "plain"))

    # Open log file in binary mode
    with open(log_filename, "rb") as attachment:
        # Add file as application/octet-stream
        # Email client can usually download this automatically as attachment
        part = MIMEBase("application", "octet-stream")
        part.set_payload(attachment.read())

    # Encode file in ASCII characters to send by email
    encoders.encode_base64(part)

    # Add header as key/value pair to attachment part
    part.add_header(
        "Content-Disposition",
        f"attachment; filename= {log_filename.split('/')[-1]}",
    )

    # Add attachment to message and convert message to string
    message.attach(part)

    with smtplib.SMTP_SSL("smtp.gmail.com", 465, context=context) as server:
        server.login(sender_email, password)
        server.sendmail(sender_email, notify_emails, message.as_string())
