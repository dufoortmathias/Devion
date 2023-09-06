export async function GetData(endpoint) {
    return fetch('http://192.168.100.237:5000/api/' + endpoint)
        .then(response => response.json())
        .then(data => data)
        .catch(error => console.log(error));
}