import React, {useState} from 'react';
import Form from '../components/Form';
import { useNavigate } from 'react-router-dom';

function LoginScreen(){

    type loginValues = {
        Username: string;
        Password: string;
    }

    type errorValues = {
        Error: string;
    }

    const navigate = useNavigate();

    const [loginValues, setLoginValues] = useState<loginValues>({
        Username: '',
        Password: ''
    });

    const [errorValues, setErrorValues] = useState<errorValues>({Error: '' });

    const handleChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        setLoginValues({
            ...loginValues,
            [event.target.name]: event.target.value
        });
    }
    

    const loginUser = async (event: React.FormEvent<HTMLFormElement>) => {
        event.preventDefault();

        if (!loginValues.Username || !loginValues.Password) {
            return;
        }

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
                console.log(response);
                setErrorValues({Error: ''});
                navigate('/dashboard');
            } else {
                setErrorValues({ Error: "Incorrect password or username." });
            }
        })
     }

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