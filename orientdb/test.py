import pyorient
import time
import sys
import matplotlib.pylab as plt

start_r = 1
max_time = 2
g = lambda x: x + 10

local_connection = {
	'host': 'localhost',
	'port': 2424,
	'login': 'root',
	'password': '' # your root password goes here
}

if len(sys.argv) == 2:
	local_connection['password'] = sys.argv[1];

remote_connection = {
	'host': 'radagast.asuscomm.com',
	'port': 2424,
	'database': 'testdb',
	'login': 'testuser',
	'password': 'test'
}

client = pyorient.OrientDB(local_connection['host'], local_connection['port'])
session_id = client.connect(local_connection['login'], local_connection['password'])
if client.db_exists('testdb'):
	client.db_drop('testdb')
client.db_create('testdb', pyorient.DB_TYPE_GRAPH, pyorient.STORAGE_TYPE_PLOCAL)
query_res = client.query("select date()")
print(query_res[0].oRecordData['date'], " on ", local_connection['host'])

local_times = []
local_counts = []

max_N = 0
r = start_r
while True:
	start_time = time.time()
	for i in range(r):
		client = pyorient.OrientDB(local_connection['host'], local_connection['port'])
		client.db_open('testdb', local_connection['login'], local_connection['password'])
		client.query("select date()")
	elapsed_time = time.time() - start_time
	print("N = ", r, ", time = ", elapsed_time)
	local_times.append(elapsed_time)
	local_counts.append(r)
	if elapsed_time > max_time:
		max_N = r
		break
	r = g(r)

client = pyorient.OrientDB(remote_connection['host'], remote_connection['port'])
client.db_open(remote_connection['database'], remote_connection['login'], remote_connection['password'])
query_res = client.query("select date()")
print(query_res[0].oRecordData['date'], " on ", remote_connection['host'])

remote_times = []
remote_counts = []

r = start_r
while r <= max_N:
	start_time = time.time()
	for i in range(r):
		client = pyorient.OrientDB(remote_connection['host'], remote_connection['port'])
		client.db_open(remote_connection['database'], remote_connection['login'], remote_connection['password'])
		client.query("select date()")
	elapsed_time = time.time() - start_time
	print("N = ", r, ", time = ", elapsed_time)
	remote_times.append(elapsed_time)
	remote_counts.append(r)
	r = g(r)

plt.scatter(local_counts, local_times, label=local_connection['host'])
plt.scatter(remote_counts, remote_times, label=remote_connection['host'], color='red')
plt.legend(bbox_to_anchor=(0., 1.02, 1., .102), loc=3, ncol=2, mode="expand", borderaxespad=0.)
plt.grid()
plt.show()