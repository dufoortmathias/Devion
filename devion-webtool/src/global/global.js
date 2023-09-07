export function PostDataWithBody(endpoint, body) {
  var myHeaders = new Headers()
  myHeaders.append('Content-Type', 'application/json')
  console.log(body, endpoint)
  var raw = JSON.stringify(';')

  var requestOptions = {
    method: 'POST',
    headers: myHeaders,
    body: raw,
    redirect: 'follow'
  }

  fetch('http://192.168.100.237:5142/api/devion/ets/createpurchasefile?id=230879', requestOptions)
    .then((response) => response.text())
    .then((result) => result)
    .catch((error) => console.log('error', error))
}

export async function GetData(endpoint) {
  return fetch('http://192.168.100.237:5142/api/' + endpoint)
    .then((response) => response.json())
    .then((data) => data)
    .catch((error) => console.log(error))
}
