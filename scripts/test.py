import fdb

con = fdb.connect(host='DEVIONSERVERETS', database="d:/ets/Dossier/MET/Winfact.fdb", user='WOUTER', password='WOUTER')

cur = con.cursor()

cur.execute("SELECT * FROM CONTACT")

headers = [desc[0] for desc in cur.description]

print(headers)

rows = cur.fetchall()

for row in rows:
    print(list(row))

con.close