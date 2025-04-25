/**
 * get the user id from the server
 * @returns Number | null
 */

export async function GetUserId(): Promise<number | null> {

        const response = await fetch('http://localhost:5055/api/Login', {
          method: 'GET',
          credentials: 'include',
          headers: {
            'Content-Type': 'application/json',
          },
        });
        if (response.ok) {
          return await response.json();
        } else {
          return Promise.reject('Error fetching user info');
        }
}