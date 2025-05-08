export async function LogOutUser(): Promise<boolean | null>{
    try{
        const response = await fetch('http://localhost:5055/api/Auth/logout-user', {
            method: 'POST',
            credentials: 'include',
            headers: {
                'Accept': 'application/json'
            } 
        });
        if(response.ok){
            return true;
        }else{
            return false;
        }
    } catch {
        return false;
    }
}