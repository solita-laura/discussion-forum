/**
 * get the user id from the server
 * @returns Number | null
 */

export async function GetUserId(): Promise<string | null> {

try{

  const response = await fetch('http://localhost:5055/api/Auth/get-userid', {
    method: 'GET',
    credentials: 'include',
    headers: {
      'Accept': 'application/json',
    },
  });
  if (response.ok) {
    return await response.json();
  } else {
    return null;
  }

} catch {
  return null;
}

}