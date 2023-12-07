const baseUrl = "http://192.168.100.211:5000/api/"

export async function PostDataWithBody(endpoint, data) {
  var myHeaders = new Headers()
  myHeaders.append('Content-Type', 'application/json')
  myHeaders.append('Access-Control-Allow-Origin', '*')
  myHeaders.append('Access-Control-Allow-Headers', '*')

  var requestOptions = {
    method: 'POST',
    headers: myHeaders,
    body: JSON.stringify(data),
    redirect: 'follow'
  }

  return fetch(baseUrl + endpoint, requestOptions)
    .then((response) => response.text())
    .then((result) => result)
    .catch((error) => console.error(error))
}

export async function PutDataWithBody(endpoint, data) {
  var myHeaders = new Headers()
  myHeaders.append('Content-Type', 'application/json')
  myHeaders.append('Access-Control-Allow-Origin', '*')
  myHeaders.append('Access-Control-Allow-Headers', '*')
  var requestOptions = {
    method: 'PUT',
    headers: myHeaders,
    body: JSON.stringify(data),
    // body: data,
    redirect: 'follow'
  }

  return fetch(baseUrl + endpoint, requestOptions)
    .then((response) => response.text())
    .then((result) => result)
    .catch((error) => console.error(error))
}

export async function GetData(endpoint) {
  return fetch(baseUrl + endpoint)
    .then((response) => response.json())
    .then((data) => data)
    .catch((error) => console.error(error))
}
