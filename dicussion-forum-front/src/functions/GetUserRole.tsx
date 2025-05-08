/**
 * get the user role from the server
 * @returns Number | null
 */

export async function GetUserRole(): Promise<string | null> {

    try{
      const response = await fetch('http://localhost:5055/api/Auth/get-userrole', {
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