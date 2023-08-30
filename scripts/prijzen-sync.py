# python -m venv .venv
# .\.venv\Scripts\activate.bat
# pip install -r requirements.txt

import requests
import datetime
import json
import os
import smtplib
import ssl
import sys

from email import encoders
from email.mime.multipart import MIMEMultipart
from email.mime.text import MIMEText
from email.mime.base import MIMEBase

from dotenv import dotenv_values

current_dir = os.path.dirname(__file__)
env_file = os.path.join(current_dir, ".env")
config = dotenv_values(dotenv_path=env_file)

start_time = datetime.datetime.now()

if __name__ == "__main__":
    try:
        company = sys.argv[1].lower()
    except:
        company = ""

endpoint = f"{config.get('BASE_URL_API')}/{company}/cebeo"
## Defines format of how datetime objects should be tranlated to a String
dateformat = config.get('DATEFORMAT')
## Create a secure SSL context
context = ssl.create_default_context()

sender_email = config.get("SENDER_EMAIL")
password = config.get("SENDER_PASSWORD")
notify_emails = config.get("LIST_RECEIVER_EMAILS").split(", ")
send_mail = False

max_price_diff = int(config.get("PERCENTAGE_MAX_PRICE_DIFF"))/100

# Variable that keeps track of all the information that was logged
# Initiliazing it with a title
output = f"Sync cebeo article prices log: {start_time.strftime(dateformat)}\n\n"

# Logs a message by printing it in the console and saving it to put it in the log file
def log(message):
    global output
    output = f"{output}{message}"
    print(message, end="", flush=True)

def get_article_ids():
    response = requests.get(f"{endpoint}/articles")
    if response.ok:
        return ["1594000"]
    else:
        return None
    
amount_failed = 0
amount_succeeded = 0

def update_price(article_id, max_price_diff):
    log(f"{article_id} ")
    response = requests.get(f"{endpoint}/updatearticleprice?articleNumberETS={article_id}&maxPriceDiff={max_price_diff}")
    if response.status_code == 200:
        global amount_succeeded
        amount_succeeded += 1
        log(f"succeeded ({json.loads(response.text)})\n")
    else:
        global amount_failed
        amount_failed += 1
        log(f"failed! ({json.loads(response.text)['detail']})\n")


try:
    article_ids = get_article_ids()

    for (index, article_id) in enumerate(article_ids):
        log(f"{int(index / len(article_ids) * 100)}% ")
        update_price(article_id, max_price_diff)

except Exception as e:
    send_mail = True

    log(e)
    log("\n\n")

# Determine runtime script
end_time = datetime.datetime.now()
duration = end_time - start_time
minutes = int(duration.total_seconds() / 60)
seconds = duration.total_seconds() % 60
log(f"\nDuration: {minutes} min {seconds} sec")

# Create log file
log_filename = f"{current_dir}/logs/{company}-cebeo/{start_time.year}/{start_time.month}/{start_time.day}/log-{start_time.strftime('%d%m%y-%H%M%S')}.txt"
os.makedirs(os.path.dirname(log_filename), exist_ok=True)
with open(log_filename, "w+") as log_file:
    log_file.write(output)

# notify people if something went wrong
if send_mail:
    message = MIMEMultipart()

    message["From"] = sender_email
    message["Subject"] = f"Script sync cebeo prices {company} failed"

    body = f"""
Er ging iets mis met het script om de prijzen van alle cebeo artikelen te updaten!
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
        for email in notify_emails:
            message["To"] = email
            server.sendmail(sender_email, email, message.as_string())