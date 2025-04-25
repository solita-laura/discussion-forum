import React, {useEffect, useState} from 'react';
import Form from '../components/Form';
import { useNavigate } from 'react-router-dom';
import { GetUserId } from '../functions/GetUserId';

/**
 * Login the user to the discussion forum
 */

function LoginScreen(){

    type loginValues = {
        Username: string;
        Password: string;
    }
    type errorValues = { Error: string;}

    const [loginValues, setLoginValues] = useState<loginValues>({
        Username: '',
        Password: ''
    });
    const [errorValues, setErrorValues] = useState<errorValues>({Error: '' });

    const navigate = useNavigate();

    /**
     * set the login values on change
     * @param event ChangeEvent
     */
    const handleChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        setLoginValues({
            ...loginValues,
            [event.target.name]: event.target.value
        });
    }
    
    /**
     * Login the user to the discussion forum
     * @param event FormEvent
     */
    const loginUser = async (event: React.FormEvent<HTMLFormElement>) => {
        event.preventDefault();

        //Dont continue with empty username or password
        if (!loginValues.Username || !loginValues.Password) {
            return;
        }

        try{
            await fetch('http://localhost:5055/api/Login', {
                method: 'POST',
                credentials: 'include',
                headers: {
                    'Content-Type': 'application/json',
                },         
                body: JSON.stringify({ Username: loginValues.Username, Password: loginValues.Password }),
            })
            .then(response => {
                if (response.ok) {
                    sessionStorage.setItem('role', response.headers.get('role') || '');
                    sessionStorage.setItem('id', response.headers.get('id') || '');

                    setErrorValues({Error: ''});
                    navigate('/dashboard');
                } else {
                    setErrorValues({ Error: "Incorrect password or username." });
                }
            });
        }catch{
            setErrorValues({ Error: "Error logging in." });
        };
     }

     /**
      * Check if the user is already logged in and redirect to the dashboard
      */

     useEffect(() => {
         async function fetchUserId() {
           const id = await GetUserId();
           if (id) {
             navigate('/dashboard');
           } 
         }
         fetchUserId();
       }, [navigate]);

    return (
        <div>
            <h1 className="text-1xl text-amber-700 uppercase p-4">Discussion forum</h1>
            <p>Welcome to the discussion forum!</p>
            <p>Please login to continue.</p>
            <Form 
                username={loginValues.Username} 
                addUsername={handleChange} 
                password={loginValues.Password} 
                addPassword={handleChange}
                logUser={loginUser}
                error = {errorValues.Error}/>
        </div>
    );
}

export default LoginScreen;