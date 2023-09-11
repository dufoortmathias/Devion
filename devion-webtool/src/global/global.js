export async function PostDataWithBody(endpoint, FileContents) {
  var myHeaders = new Headers()
  myHeaders.append('Content-Type', 'application/json')
  myHeaders.append('Access-Control-Allow-Origin', '*')
  myHeaders.append('Access-Control-Allow-Headers', '*')

  const raw = JSON.stringify({
    FileContents: FileContents
  })

  var requestOptions = {
    method: 'POST',
    headers: myHeaders,
    body: raw,
    redirect: 'follow'
  }

  return fetch('http://192.168.100.237:5000/api/' + endpoint, requestOptions)
    .then((response) => response.text())
    .then((result) => result)
    .catch((error) => console.error(error))
}

export async function GetData(endpoint) {
  return fetch('http://192.168.100.237:5000/api/' + endpoint)
    .then((response) => response.json())
    .then((data) => data)
    .catch((error) => console.error(error))
}
